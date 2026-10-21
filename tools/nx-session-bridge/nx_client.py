"""Local client. A heartbeat alone is not proof of successful CAD operations."""
import argparse
import datetime as dt
import hashlib
import json
from pathlib import Path
import time
import uuid

HOME = Path(__file__).resolve().parent
STATE = HOME / "state"


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("operation", choices=["status", "inspect", "stop", "run_python"])
    parser.add_argument("--script")
    parser.add_argument("--expected-work-part")
    parser.add_argument("--timeout", type=float, default=35)
    args = parser.parse_args()
    heartbeat_path = STATE / "heartbeat.json"
    if not heartbeat_path.exists():
        raise SystemExit("NOT CONNECTED: load 00_start_connection.py once inside NX.")
    heartbeat = json.loads(heartbeat_path.read_text(encoding="utf-8-sig"))
    stamp = dt.datetime.fromisoformat(heartbeat["utc"].replace("Z", "+00:00"))
    if (dt.datetime.now(dt.timezone.utc) - stamp).total_seconds() > 10:
        raise SystemExit("NOT CONNECTED: heartbeat is stale; no command submitted.")
    if heartbeat["status"] != "ready":
        raise SystemExit("NOT CONNECTED: bridge is stopped.")
    if args.operation == "status":
        print(json.dumps(heartbeat, indent=2))
        return
    request = {
        "session_id": heartbeat["session_id"],
        "operation": args.operation,
        "expires_utc": (dt.datetime.now(dt.timezone.utc) + dt.timedelta(seconds=30)).isoformat(),
    }
    if args.operation == "run_python":
        if args.script is None or args.expected_work_part is None:
            parser.error("run_python requires --script and --expected-work-part")
        root = (HOME / "commands").resolve()
        script = (root / args.script).resolve()
        script.relative_to(root)
        if script.suffix.lower() != ".py":
            parser.error("Only Python command scripts are allowed")
        request.update(script=args.script, expected_work_part=args.expected_work_part,
                       sha256=hashlib.sha256(script.read_bytes()).hexdigest())
    request_id = uuid.uuid4().hex
    target = STATE / "queue" / (request_id + ".json")
    temporary = target.with_suffix(".tmp")
    temporary.write_text(json.dumps(request), encoding="utf-8")
    temporary.rename(target)
    result = STATE / "results" / (request_id + ".json")
    deadline = time.monotonic() + args.timeout
    while time.monotonic() < deadline:
        if result.exists():
            response = json.loads(result.read_text(encoding="utf-8-sig"))
            print(json.dumps(response, indent=2))
            raise SystemExit(0 if response["status"] == "PASS" else 1)
        time.sleep(0.25)
    raise SystemExit("No response. Request ID: " + request_id +
                     ". Inspect state/results and queue before retrying; execution may have occurred.")


if __name__ == "__main__":
    main()
