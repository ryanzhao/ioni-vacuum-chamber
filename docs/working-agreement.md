# Working agreement

## Team-level planning

Tasks belong to Diagnostics, Electronics & Control, or Hardware Design & Vacuum Chamber. A task affecting multiple groups carries all relevant Subteam tags and Scope = Cross-team. The current stage does not allocate personal tasks, subteam membership or individual coordinator roles.

Each team task states its deliverable, acceptance criteria, dependencies, review route, next action and evidence. Dates are agreed at a subteam review; none are invented. Personal Assignee and Reviewer fields are reserved for a later allocation stage and remain empty and hidden from current views. Historical planning fields are not current assignments.

Task states: Proposed -> Ready -> In Progress -> In Review -> Done. Use Blocked when an obstacle prevents progress. After each subteam working session, update Status, Last check-in, Next action, Blocker and Evidence. Report concrete results instead of unsupported percentages.

## Communication and help

Keep the existing Slack workspace. Use its existing channels until these suggested routes are mapped:

| Suggested route | Use |
| --- | --- |
| `vc-announcements` | Shared milestones, meeting links and decisions |
| `vc-help` | Questions with unclear ownership; the project maintainer routes them |
| `vc-diagnostics` | Measurement needs, calibration, diagnostic plans and data quality |
| `vc-electronics-control` | Electronics, wiring, PCB requirements, control logic and logging interfaces |
| `vc-hardware-vacuum` | Chamber CAD, structure, ports, pumping, fabrication and installation interfaces |

These are communication recommendations; this change does not create Slack channels. No individual subteam coordinator is assigned. Ask in a task-linked thread with the expected/actual result, what was tried, evidence and help needed. Escalate unanswered or cross-team questions to the project maintainer at the next agreed check-in. Do not wait for a meeting to report a blocker.

Summarize consequential discussion in a GitHub record and link it from the team task. Shared interface changes require review by affected subteams.

## Meetings and onboarding

- Weekly 30-minute project review: team evidence, blockers, shared interfaces and next team deliverables.
- Subteam working sessions as needed; scope reviews establish Ready tasks and required inputs.
- Milestone reviews use the roadmap gates. A meeting taking place does not imply design approval.
- Member onboarding records signup interests, demonstrated skills and tool access. It does not allocate personal work at this stage.

## Source of truth

Notion owns live team-task state. GitHub owns reviewed records and reference metadata. Keep one task backlog; repository issues link the relevant Notion team task when work is required. Managed shared storage holds unmodified large originals, linked through manifests.

Progress visibility depends on subteam updates. No automatic Slack/Notion/GitHub synchronization, invitations or public publishing is installed by this setup.
