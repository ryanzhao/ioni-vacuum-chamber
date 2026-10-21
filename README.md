# Vacuum Chamber Project

Engineering record and collaboration foundation for the UIUC Ion-i vacuum chamber project, led by the project maintainer.

**Stage:** team onboarding and requirements definition. Current hardware configuration, design approval, operating conditions and measured performance are not established by this repository.

## AI entry point

AI assistants: read [AGENTS.md](AGENTS.md), [PROJECT_CONTEXT.md](PROJECT_CONTEXT.md), then [AI Guide](docs/ai-guide.md). The guide maps questions to files and user intent to exact prompt sections. **Find and apply the matching prompt automatically; the user should not need to paste it again.** Check its prerequisites and stay within the current request.

To start with another AI, send:

> Read this repository's AGENTS.md, PROJECT_CONTEXT.md and docs/ai-guide.md. Use the information map to find evidence and automatically apply the prompt matching my request. My task is: [describe the task].

The repository contains instructions and reusable prompts; it does not itself run an AI or schedule background work.

## Start here

- [Fusion AI Interaction](docs/fusion-ai-interaction/README.md): copy-paste local AI prompt for installing, connecting, and validating Fusion MCP without mouse or keyboard automation.

- Notion Project Hub (link shared privately with members): live team tasks, member information, blockers and record navigation.
- [Team and help routes](docs/team.md): three subteams and communication routes.
- [Team task register](docs/team-tasks.md): subteam deliverables and shared interfaces; no personal allocation.
- [Working agreement](docs/working-agreement.md): progress updates, reviews and communication.
- [Roadmap](docs/roadmap.md): milestones, dependencies and completion gates.
- [Engineering baseline](design/baseline.md): verified facts, unknowns and interfaces.
- [References](references/README.md): BibTeX, source register and reading notes.
- [Record templates](records/README.md): meetings, decisions, experiments and media.
- Existing learning portal on UIUC Box (link shared privately with members): member access must be checked at onboarding.

## Where work belongs

| Tool | Responsibility |
| --- | --- |
| Notion | One live team task per deliverable; subteam(s), review route, next action, blocker, agreed dates and evidence |
| GitHub | Reviewed technical records, references, analysis, software and design snapshots |
| Slack | Questions, discussion and announcements; link the task and summarize the outcome into the formal record |
| Managed shared storage | Original photos, videos, raw instrument data and large native CAD; GitHub stores manifests and persistent links |
| Fusion | Working CAD and simulation; review exports, inputs and revision IDs are recorded here |

Live visibility means updates after each work session. There is no automatic observation of a member's work and no Notion-Slack-GitHub synchronization installed yet.

## Evidence chain

`Requirement -> Notion task -> branch/PR -> design decision -> run -> raw-file manifest -> analysis commit -> figure -> paper/video/website release`

Every item links to its inputs. Templates and proposed assignments are not experimental results or accepted commitments. Public access has been requested for team reading. Verify live visibility. Member information and emails must remain excluded, including commit metadata and historical references. Review results, rights and sensitive material before release. Linked Notion and storage resources retain their own access permissions.

See [CONTRIBUTING.md](CONTRIBUTING.md) to submit work. No open-source license or automatic publication has been selected.
