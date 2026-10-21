# Vacuum Chamber Project — Agent Instructions

This repository is the AI-readable engineering knowledge base for the project maintainer's UIUC Ion-i Vacuum Chamber project.

## Required entry sequence

Read applicable parent/workspace instructions first, then `PROJECT_CONTEXT.md`, `README.md`, and `docs/ai-guide.md`. Read `CONTRIBUTING.md` before edits and the task-relevant documents identified by the guide. Do not assume previous chat access.

Use the information map and prompt routing table in `docs/ai-guide.md` automatically for every request. Open and apply the matching prompt directly; do not require the user to copy it again. Execute only the workflow selected by the current request, within existing authorization. A prompt stored in the repository does not independently authorize installation, CAD changes, messages or publication. If no task is given, orient read-only and wait.

## Project constraints

All project artifacts must be English. Keep exactly three subteams: Diagnostics; Electronics & Control; Hardware Design & Vacuum Chamber. Allocation remains at team/subteam level; do not invent personal assignments, reviewers, memberships or deadlines.

Do not invent engineering requirements or results. Use UNKNOWN / NEEDS VERIFICATION / PROPOSED as appropriate. Historical AFMPDT results are not the current baseline. Preserve units, source revisions, raw-data links and validation evidence.

Use Notion for current team tasks, progress and blockers; GitHub for durable technical records; Slack for discussion; managed storage for large originals. Before external edits, revalidate account, resource IDs, schemas and current remote branch contents. Preserve Daily Log and Expense Log. Do not create replacement resources because the wrong connector account is active.

Budget/manufacturing-method selection is not a standalone planning task. Flag conflicting historical wording rather than silently changing decisions. Do not send messages, publish publicly or invite people without authorization.

## Fusion

Read `docs/fusion-ai-interaction/README.md` and actual tool schemas. Use code/API/MCP only: no screen reading, screenshots, simulated input, foreground takeover or GUI fallback. Verify live connection and intended document immediately before operations. Never use mock data, installation files or a listening port as proof of live modeling. Setup and training prompts run only when selected by the user's request.

## Files and Git

On the project maintainer's machine, read the local workspace instruction file first and use the maintainer's local project folder. Other machines must discover and obey their own authorized workspace; do not create directories based on another machine's paths.

The original AFMPDT delivery is retained for protection. Do not delete it, synchronize conflicting copies, inherit unrelated AFMPDT work or migrate original conversation history.

Check for `.git` at the actual working root before Git operations. the project maintainer's delivery copy initially has none; never commit through an ancestor repository. GitHub checkouts elsewhere may have their own `.git`. Inspect current remotes and branches before edits; local files are not assumed equal to the remote.

Public visibility is authorized only after the privacy cleanup is verified, including historical GitHub references. Do not publish member information or contact details. Exclude secrets, authentication/configuration files, machine settings, runtime/cache/work files and large originals from commits. Preserve concurrent work. Record material context changes in `PROJECT_CONTEXT.md`, retaining historical distinctions.
