"""Load the project-local bridge once inside the existing NX session."""
from pathlib import Path
import traceback

import NXOpen


def main():
    home = Path(__file__).resolve().parent
    try:
        NXOpen.Session.GetSession().Execute(
            str(home / "NxSessionBridge_v2.dll"), "NxSessionBridge", "Start", []
        )
    except Exception:
        (home / "connection_load_error.txt").write_text(
            traceback.format_exc(), encoding="utf-8"
        )
        raise


if __name__ == "__main__":
    main()
