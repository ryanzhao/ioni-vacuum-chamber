# Contributing

All new project content is written in English. Start with an agreed team-level Notion task and its acceptance criteria. Personal assignments are not set at this planning stage.

## Submit a change

1. Create a branch such as `codex/VC-012-instrument-matrix`; a member may use `work/VC-012-instrument-matrix`.
2. Add the smallest reviewable deliverable. Include its task URL, author, date, status, units, inputs, assumptions and sources.
3. Preserve raw data. Write derived data to a new path. Link large originals through a manifest with SHA-256 hashes.
4. Open a pull request using the template. Request review through the relevant subteam; use the project maintainer when routing is unclear.
5. Address comments. Set the task to In Review while review is pending. Mark Done only after the reviewer accepts the evidence.

Do not overwrite another person's CAD or analysis. Coordinate shared assembly edits; export revisioned review snapshots. Use text diffs for analysis/code and an image/PDF comparison for binary design changes.

## Review criteria

- The deliverable answers the task and cites exact source sections, pages or timestamps.
- Targets, assumptions, predictions and measurements are labelled separately.
- Calculations include units, configuration, software version, uncertainty and relevant checks.
- CAD/FEA results identify geometry revision, constraints, loads, mesh and limitations.
- No passwords, tokens, cookies, credentials, `.env` contents or private keys are included.
- Contributor names are credited accurately; the work does not claim approval, hardware operation or publication that has not occurred.

Correct a record in a new commit with a reason. Supersede decisions by linking the replacement; retain the earlier history. Never rewrite published project history to conceal a mistake.
