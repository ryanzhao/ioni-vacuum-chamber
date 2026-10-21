# NX AI Interaction and Reconnection Runbook

Status: project-local implementation; recorded 2026-10-21. This is an English, privacy-filtered technical handoff, not a transcript or an engineering approval.

## Read this distinction first

| Name | What is established here |
| --- | --- |
| Siemens NX | An existing Windows NX installation was used for visible CAD work. Its original installer, installation date and licensing setup were not captured by this project. Do not claim this record proves a fresh installation. |
| NXOpen / NX Open | Siemens' official programming API, used inside the running NX session. This is the official component in our implementation. |
| Project NX session bridge | Our C# timer, local JSON queue and Python client around NXOpen. Source is included below. This is custom project code, not a Siemens product or an MCP server. |
| NXMCP / NX MCP | A requested integration name, not a verified installed server in the preserved implementation. No Siemens-issued NX modeling MCP endpoint was verified in this work. This is not proof that none exists elsewhere. |
| OpenNX | An ambiguous name from the discussion. No separate installed product of that name was verified. Do not silently equate it with NXOpen or install an unrelated package. |
| Fusion MCP | A different application's integration; see the separate Fusion guide. Its endpoint and configuration do not connect NX. |

Do not advertise this implementation as an official Siemens MCP interface. A future MCP adapter would be another custom layer unless Siemens supplies and verifies it. No working Codex/Claude MCP configuration for NX is claimed by this record.

## Architecture actually used

```text
Codex/Claude local shell tool
  -> Python nx_client.py
  -> local state/queue/<request-id>.json
  -> C# WinForms Timer in the running NX GUI process
  -> Siemens NXOpen Session.ExecuteWithStringArguments
  -> NX embedded Python journal
  -> visible NX model + state/results/<request-id>.json
```

NXOpen work runs on the NX UI thread. The timer polls every 500 ms, with a heartbeat approximately every two seconds while idle. No mouse/keyboard simulation or foreground takeover is involved. Heavy modeling still blocks UI responsiveness temporarily. The bridge is not a background CAD kernel, remote service, HTTP endpoint or authentication boundary.

## Preserved implementation

Sources: [bridge](../../tools/nx-session-bridge/NxSessionBridge.cs), [bootstrap journal](../../tools/nx-session-bridge/00_start_connection.py), [client](../../tools/nx-session-bridge/nx_client.py), [build script](../../tools/nx-session-bridge/build.ps1).

The bridge/client/bootstrap are the sources inspected in the local learning workspace. The build helper is a newly documented reproduction helper. Compiled binaries, proprietary Siemens assemblies, private paths, CAD, runtime queues and full conversation history are deliberately not published. Rediscover all local paths; do not assume another machine has the maintainer's installation layout.

Documentation-task verification: the build helper compiled these sources against the installed NX managed assemblies on 2026-10-21. Its first draft omitted the required `NXOpen.Utilities.dll` reference; that reference was added and compilation passed. The newly built binary was not loaded into NX during this task, and no model was changed. Reinstallation and a clean-machine end-to-end connection remain NOT TESTED.

## Installation and first connection

1. Read repository AGENTS, PROJECT_CONTEXT and AI Guide; discover the authorized local workspace. Ask for installation authorization where needed. Preserve all existing CAD and settings.
2. If NX is absent, obtain the supported NX installer and a legitimate license through Siemens or the institution's software administrator. Follow that release's official installation and licensing instructions. This repository does not distribute NX, license files or a license workaround. Exact clean-install reproduction remains NEEDS VERIFICATION.
3. Verify that NX launches normally and can create/open a disposable Model part manually. If it exits, collect its process exit information and relevant NX system log; do not alter DLLs or license settings speculatively. NXOpen cannot repair a broken NX installation by itself.
4. Find `NXBIN/managed/NXOpen.dll` and the installed version's NXOpen documentation. Use the installed version's API signatures rather than copying arbitrary newer examples. This implementation was used with NX 2306; portability to other releases is not established.
5. Copy the four bridge sources into a private, writable, trusted local folder under the authorized workspace. Keep the checkout's published sources separate from runtime state if practical. Create a `commands` subfolder. Use a compatible .NET Framework C# compiler and run the build helper with the actual `NXBIN` path. Do not use a .NET Core build as an unverified substitute.

```powershell
# Run in the private bridge folder; replace both discovered paths.
.\build.ps1 -NxBin '<actual NXBIN path>' -Compiler '<actual .NET Framework csc.exe path>'
```

6. In NX, set the journal language to Python under User Interface Preferences > Tools > Journal if recording Python journals. Movie Settings is screen recording, not the Journal tool or a connection setting.
7. Use NX's command search/menu to open Journal Manager / Play Journal (wording varies by release). Browse to `00_start_connection.py`, then Run **once in the existing GUI session**. The bootstrap uses `Session.Execute(dll, "NxSessionBridge", "Start", [])`; the earlier string-arguments DLL entry attempt failed with `Symbol not found in library`.
8. From a local shell, run the following. `status` reads local heartbeat data; `inspect` submits a request to NX and is the stronger live check.

```powershell
python .\nx_client.py status
python .\nx_client.py inspect
```

Verify a fresh response, current session/process, Work Part, Display Part, units and component names. Do not reuse an old heartbeat as proof. A documentation-only check on 2026-10-21 returned `NOT CONNECTED: heartbeat is stale; no command submitted`; historical modeling success does not mean the current session remains connected.

No manual journal run is needed for every subsequent operation after the bootstrap works. After restarting NX, load the bootstrap again. Automatic USER_STARTUP loading is not the selected verified production workflow. If starting was stopped with the client's `stop` operation, restarting NX and bootstrapping is the clear recovery: the current `Start()` returns early when its timer object already exists, even if stopped.

## Day-to-day CAD operation

1. Run live `inspect` immediately before a mutation. Confirm the intended **Work Part**, not just the visible assembly. Preserve the original by using a separate training document/revision when appropriate.
2. Put a reviewed Python journal under the private bridge folder's `commands` directory. Use NX embedded Python and `NXOpen.Session.GetSession()`, not a system-Python `import NXOpen` attempt. Include a `main()` and `if __name__ == "__main__": main()` guard.
3. Use explicit units and an NX undo mark. Catch errors and write a domain result with traceback to an authorized local path. Avoid message boxes and unhandled exceptions that create blocking NX dialogs. A caught error must not be reported as a successful model.
4. Execute with the exact Work Part path from inspect:

```powershell
python .\nx_client.py run_python --script 'example.py' --expected-work-part '<exact Work Part full path>' --timeout 180
```

5. Inspect the bridge result **and** the journal's domain output. Bridge PASS means only that the journal returned. Check feature health, dimensions, body counts, assembly occurrences, output file existence and reopen/import evidence as appropriate. A saved file alone is not verification.
6. Explain completed changes and limitations to the user. For stepwise teaching, make one semantic operation per request and pause; for an authorized complete example, build in stages with checkpoints. These rules do not authorize unrelated changes.

The transport checks session ID, a 30-second request expiry, expected Work Part, script SHA-256 and a `.py` path inside `commands`. The bridge rejects reparse-point paths. These checks prevent common accidents, not malicious scripts: a journal can execute arbitrary Python with the NX user's privileges. Allow only trusted writers to the bridge folder, inspect every submitted journal, and never expose the queue over a network or synchronize it between machines. The Work Part guard is not a complete assembly/revision lock.

## Timeout and failure recovery

| Symptom | Action |
| --- | --- |
| Missing/stale heartbeat | Do not submit a mutation. Check NX still runs, whether a modal dialog/long operation blocks its event loop, and `bridge_error.txt`. Bootstrap in the correct session if needed. |
| `Symbol not found in library` | Check the bootstrap uses `Session.Execute` with class and method, the DLL was built from these sources, and NX release/runtime compatibility. Do not change the Python journal call into the failed DLL-loading form. |
| Work Part/session/hash changed | Inspect again; review the new target/script. Do not bypass the guard. |
| Client timeout | Record the request ID, inspect `queue/*.processing`, `results/<id>.json`, journal output and live geometry before any retry. The operation may already have executed. Never blindly resubmit a mutating journal. |
| NX error dialog | Stop submission. Have the user resolve the dialog or use an explicitly authorized, available mechanism; do not assume this guide grants desktop control. Catch the underlying error in later journals. |
| `bridge_error.txt` exists | It may be historical. Compare timestamps and current state. The timer stops after an outer-loop exception; inspect the source error before restarting. |
| PASS but missing model/export | Check domain JSON, NX body/feature state and export logs. Journals may catch failures; exporters may write an empty but syntactically valid file. |

Requests are renamed to `.processing` before execution and retained afterward. This implementation has no automatic crash recovery, cancellation, transaction rollback, exactly-once guarantee or multi-client arbitration. Run only one writer at a time. A timeout is not cancellation. Keep stale runtime records private; do not delete originals merely to make a status check pass.

## Historical approaches and outcomes

This is a curated evidence ledger, not an exhaustive recovered installer log.

| Approach | Evidence / outcome | Decision |
| --- | --- | --- |
| Existing desktop NX | Used for visible parts and assemblies; original installation steps not retained | Reuse legitimate installation; verify before repair |
| Screen recording / Movie Settings | Was opened while looking for automation | Video capture only; not API connection or feature-history reconstruction |
| Python Journal Manager | One-time journal execution is the actual bootstrap route | Retained; no per-operation manual Run afterward |
| Standalone `run_journal.exe` | Earlier investigation identified batch mode, not persistent GUI attachment | Do not substitute for controlling the already-open session |
| USER_STARTUP probe | Earlier investigation recorded `Parts.NewDisplay` error 3600001 / `libpmiqueryui.dll` | Historical limitation, not evidence that today's NX cannot create parts; not selected as current bootstrap |
| DLL via `ExecuteWithStringArguments` | Preserved local bootstrap error: `Symbol not found in library` | Replaced by `Session.Execute` for DLL startup |
| C# timer + JSON queue + Python journals | Actual inspected sources and local modeling checkpoints for sphere, ports, assemblies and reviewed long chamber | Selected custom connection; source included; current session must still be revalidated |
| Desktop mouse/keyboard automation | Discussed/requested during exploration | Not required by selected API workflow; not a default fallback |
| NXMCP / OpenNX product installation | No verified product/version/installer/endpoint identified in this implementation | UNKNOWN; require official provenance and actual live tool schemas before adoption |

Earlier USER_STARTUP details come from a prior investigation record, not a newly reproduced failure. Raw conversation history and personal screenshots stay private; they are not bundled into this public runbook.

## Lessons from the CAD examples

- Start with separate shell, neck/flange, closure, support and equipment parts, and combine component occurrences in a top assembly. Keep repeated parts reusable. Give independent revisions distinct part filenames to avoid NX same-name loading collisions.
- The first example was a spherical hollow chamber with radial ports. The later photograph-inspired example was a roughly two-metre assembly, then a separate reviewed revision. These were visual teaching examples, not verified manufacturing drawings or an approved project baseline.
- `NXOpen.Point3d` arguments should be floats; the cylinder builder used `AxisDiameterAndHeight`. Validate axis transforms before generating many ports; the helper used local axes as matrix rows.
- In the tested Python binding, bounding boxes used `UFSession.ModlGeneral.AskBoundingBox(occurrence.Tag)`. A method appearing in managed XML is not proof it exists in the Python binding.
- Exact interference checks used `AnalysisManager.CreateSimpleInterferenceObject()` and component body occurrences, not bounding-box overlap alone. Distinguish intended weld/overlap envelopes from unexplained interference; a visually clean assembly is not engineering acceptance.
- STEP export required an explicit `LayerMask = '1-256'` and solid selection. An initial export produced zero solids despite returning successfully. Verify the export log, solid entities and reimported geometry. Do not trust file existence or API return alone.
- Session replay is not automatically a recording of every prior API call. A model history/assembly build replay must have been explicitly captured or reconstructed; Movie recording cannot retrospectively recover it.
- Preliminary vacuum analysis did not establish whole-assembly adequacy. Invalid full-geometry meshing results were withdrawn; separate reference-cylinder results do not approve the chamber. Never turn this connection runbook into a claim that the CAD is safe to evacuate.

## Codex / Claude handoff prompt

BEGIN PROMPT

Read AGENTS.md, PROJECT_CONTEXT.md, docs/ai-guide.md and this NX guide. My task is: [specific connection or CAD request]. Discover this machine's authorized workspace and legitimate NX installation. Reuse existing setup; do not assume private runtime files exist in the repository. Identify Siemens NXOpen as official and this session bridge as custom, not an official Siemens MCP server. Do not silently substitute Fusion or an unverified NXMCP/OpenNX package.

For read-only connection, locate the existing bridge and run nx_client.py status then inspect. Verify fresh live response, current Work/Display Part and units. If disconnected, report the exact missing layer and the single bootstrap step. Do not install, change global configuration or manipulate the desktop merely because this prompt mentions setup. For authorized setup, use the bundled sources and build instructions, obtaining installation/licensing prerequisites legitimately.

For authorized CAD changes, inspect again, preserve originals, review a scoped NXOpen journal under commands, use exact expected Work Part and explicit units, and verify the journal's own result plus live geometry. Do not blindly retry timeouts. Do not use mouse/keyboard simulation or foreground takeover. Report separate installation, bootstrap, heartbeat, live inspection, modeling and export statuses as PASS / FAIL / BLOCKED / NOT TESTED. A successful transport is not successful CAD or engineering approval.

END PROMPT

## If official MCP support is requested

Treat that as a separate capability check. Require a Siemens-published product/documentation URL, supported NX version, installation instructions, transport endpoint, authorization model and actual MCP initialize/tools-list/live-document results. The MCP protocol being official does not make a third-party NX server Siemens-official. Do not invent a configuration snippet for an endpoint that has not been verified. Codex and Claude can use the local shell/client route above without an NX MCP server when their local execution tools are available and authorized.

## Official reference entry points

- [Siemens NX programming and customization fact sheet](https://www.plm.automation.siemens.com/en_gb/Images/4988_tcm642-4564.pdf): official background on NXOpen and journaling; historical document, not current API signatures.
- [Siemens Open API Programming course description](https://training.plm.automation.siemens.com/ilt/iltdescription.cfm?pID=TR13110_______NX___7.5____5000): historical official overview; course is no longer offered.
- [Siemens Support Center](https://support.sw.siemens.com/): obtain entitled documentation and installation instructions for the actual installed NX release. Prefer installed NXOpen API references for exact methods and signatures.

No Siemens binaries, licensing material, machine configuration or credentials are included here. Source publication is a technical handoff, not a new license grant for Siemens software or the repository.
