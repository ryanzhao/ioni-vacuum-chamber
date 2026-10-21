# Fusion AI Interaction

English onboarding for the UIUC Ion-i Vacuum Chamber team.

## Automatic use by an AI assistant

Read [the AI routing guide](../ai-guide.md) first. If the user asks for Fusion MCP setup, read and execute the complete setup prompt below within that request; do not require another copy-paste. For connection-only inspection or CAD edits, use the matching inspection/modeling sections and reuse the existing installation. Reading this page alone does not trigger setup or the optional training exercise. Discover machine-specific paths and verify actual tools and live document state.

## How to use this guide

Open a local AI coding agent with terminal access and MCP support on the same computer as Autodesk Fusion. Copy the entire prompt below into that agent. It should inspect the machine and perform the setup, rather than merely explain installation.

This guide uses Autodesk's official **Autodesk Fusion MCP Server**, which is built into Fusion desktop and runs locally on the same computer. No third-party server, cloned repository, Python bridge or manually installed add-in is required. Autodesk also offers a cloud **Fusion Data MCP Server** for hubs, projects and files; it does not perform live modeling and is outside this setup.

Reference: [Autodesk Fusion MCPs Overview](https://help.autodesk.com/view/fusion360/ENU/?guid=FMCP-OVERVIEW).

Setup must be performed on each member's computer. Chat-only assistants without local tools cannot install software or connect to desktop Fusion. A cloud-hosted AI model can work through a local MCP-capable client, subject to the team's data-sharing rules.

## Copy-paste setup prompt

--- BEGIN PROMPT ---

You are my local engineering AI assistant for the UIUC Ion-i Vacuum Chamber project. Establish a working, repeatable connection from this AI client to Autodesk Fusion using MCP and the Fusion Python API. Perform the authorized work, verify it, and explain how I can use it afterward. Do not stop at giving me installation instructions.

### 1. Scope and authorization

I authorize you to inspect relevant local software and existing MCP settings, and to add or merge one Fusion MCP server entry in this AI client's configuration. Back up existing configuration files before changing them. Respect local workspace instructions and platform approval requirements.

Use only Autodesk's official built-in Fusion MCP Server. Do not download, clone or install third-party Fusion MCP servers, bridges or add-ins. Preserve unrelated MCP servers, login state, existing CAD, unsaved work and concurrent edits. Do not reinstall Fusion or change firewall rules, permissions or security policies.

This request authorizes setup and read-only connection validation. Production CAD changes require a subsequent modeling request. Do not infer chamber dimensions, materials, loads, interfaces or approved designs from this onboarding guide.

### 2. Interaction rules

Use terminal commands, configuration files and the MCP tools exposed by Fusion.

Do not use screenshots, screen reading, mouse movement, keyboard simulation, GUI automation, window activation or foreground switching. Do not use UI automation as a fallback.

The MCP server runs inside the Fusion desktop application. Fusion must be running, and API work can still affect its responsiveness.

If Autodesk sign-in, Fusion installation, enabling the MCP preference or a client restart requires me, report the exact manual step. Complete independent setup first, then stop at the genuine dependency. Never claim a connection merely because a configuration entry exists.

### 3. Inspect before changing anything

Read applicable AGENTS.md or equivalent workspace rules. Determine:

- OS, project root and permitted output locations.
- Current AI client, its version, its actual MCP configuration location and supported format, including whether it supports HTTP (Streamable HTTP) MCP servers.
- Whether Fusion is installed and running.
- Whether http://127.0.0.1:27182/mcp responds.
- Existing MCP entries named fusion, fusion360 or similar, without printing credentials or unrelated configuration. If an entry points to a third-party Fusion bridge, report it and propose replacing it with the official endpoint; do not delete it without my approval.

Use discovered paths. Never copy another member's username, paths or configuration.

### 4. Enable the official server in Fusion

I perform this step myself in Fusion:

1. Open **Preferences > General > API**.
2. Enable **Fusion MCP Server (runs locally on this device)**.
3. Keep the default local address http://127.0.0.1:27182/mcp unless Fusion shows a different one.

The server listens on the loopback address only. Do not expose it to the network.

### 5. Register the server with the local AI client

Use the client's actual supported configuration or CLI, checked locally first, and its official documentation if necessary. Merge one entry, preserve unrelated fields and back up the original.

- **Claude Desktop:** Settings > Connectors > Browse connectors, search for **Autodesk Fusion** and install it. No URL entry is needed.
- **VS Code** (.vscode/mcp.json):

```json
{
  "servers": {
    "fusion": { "type": "http", "url": "http://127.0.0.1:27182/mcp" }
  }
}
```

- **Cursor** (mcp.json):

```json
{
  "mcpServers": {
    "fusion": { "url": "http://127.0.0.1:27182/mcp" }
  }
}
```

- **Codex** (config.toml):

```toml
[mcp_servers.fusion]
url = "http://127.0.0.1:27182/mcp"
```

Reference: [Connecting to the Autodesk Fusion MCP Server](https://help.autodesk.com/view/fusion360/ENU/?guid=ADSKMCP_FusionDesktopMcp_connecting_to_the_fusion_mcp_server_html).

If the client cannot reload tools in this conversation, state that clearly and give the minimal restart or new-session instruction.

### 6. Verify each layer independently

Report each item as PASS, FAIL or BLOCKED, with concise evidence:

1. Fusion: the desktop application is running and the MCP preference is enabled.
2. Endpoint: http://127.0.0.1:27182/mcp answers MCP initialize and tools/list.
3. Tools: record the tool names and schemas actually returned. Autodesk discovers tools dynamically and they can change between Fusion releases, so never rely on a fixed list or invent tool names.
4. Live Fusion access: perform one read-only query of the active document using an exposed tool. Handle an empty Fusion session honestly.
5. Client integration: confirm that THIS client can call the tools, or mark client reload as pending.

Do not create, save, rename, close or modify a production design to test connectivity.

### 7. Rules for subsequent CAD requests

Once connected, use the following procedure whenever I request actual modeling:

- Re-identify the active document, design, components, parameters, and unsaved state immediately before changes. Never assume the last active tab is still the target.
- Work on the requested document or an agreed separate copy. If the target is ambiguous, resolve it before mutation.
- Prefer named user parameters, constrained sketches, and meaningful feature/component names. Use the tools Fusion actually exposes; read each schema first.
- Specify units explicitly in parameter expressions and verify the units required by each API. Raw geometric lengths commonly use Fusion's internal centimeters; displayed millimeters do not change that.
- Make small sequential operations. Avoid concurrent mutations and large unbounded scripts. 
- After a timeout or partial failure, query the actual model before retrying; a timed-out mutation may already have changed geometry.
- Check body/component counts, critical dimensions, parameter values, feature health, and exports using API results.
- Preserve the original document and existing edits. Never use delete_all, broad deletion, or undo against unrelated user work.
- Save/export only to an explicitly authorized location and avoid file dialogs. Distinguish Fusion cloud save from a local F3D/STEP export.
- Keep execution scripts and a concise change record with task ID, document/revision identifier where available, input parameters and units, checks, output paths, and remaining uncertainties.
- Treat generated CAD as a proposed design until reviewed. Successful geometry creation does not establish vacuum integrity, structural adequacy, manufacturing readiness, or experimental performance.

A later request to change CAD authorizes the necessary ordinary API operations within its stated scope; do not ask for permission at every sketch or feature.

### 8. Troubleshoot without taking over my computer

- Connection refused: check that Fusion is running, the MCP preference is enabled, and the client uses http://127.0.0.1:27182/mcp.
- Tools listed but CAD queries fail: confirm a document is open and check the Fusion version; a responding endpoint is not proof of live design access.
- Wrong document: query documents/components through the API and resolve the intended target before modifying anything.
- Missing tool: inspect tools/list for the installed Fusion version; do not invent tool names or switch to a third-party server.
- Tool timeout: inspect scene state and feature health before deciding whether a retry is safe.
- No local tools or MCP support in this AI client: report the capability gap and provide the smallest necessary action for a supported local client.
- Permission or authentication block: preserve completed work, report the exact blocked action, and never bypass security or fake success.

### 9. Deliver a usable handoff

Finish with a concise report:

- Overall status: CONNECTED, INSTALLED / ACTIVATION PENDING, CLIENT RELOAD PENDING, or BLOCKED.
- Per-layer verification results and what remains unverified.
- Fusion version, endpoint URL, and modified configuration entry name.
- Backup locations and how to restore only the files/settings changed in this setup.
- Exact minimal manual step, only if necessary.
- A short instruction I can paste into a future session to reconnect using the existing installation.

Store a sanitized machine-specific setup record locally in the permitted project workspace, excluding secrets and private configuration contents. Do not commit or push it automatically. Do not publish CAD or send messages to teammates.

Start now by inspecting this machine and the existing setup.

--- END PROMPT ---

## Practice prompts after connection

**Read-only inspection**

> Connect using the existing Fusion MCP setup. Use API queries only. Identify the active document, list components and user parameters with units, and report connection evidence. Do not modify the design or use screenshots, mouse, or keyboard automation.

**First modeling exercise**

> Using the connected official Fusion MCP tools, create a separate new training document named MCP_Training_Block. Create a parametric block with length 40 mm, width 30 mm, and height 10 mm. These are training dimensions only. Preserve all existing documents. Verify one solid body and its dimensions through API queries. Leave it open and unsaved; do not invoke dialogs, screenshots, or simulated input.

**Update an existing design**

> Inspect the intended document and its current parameters first. In the document I identify, change only the parameter I specify, using my supplied expression and units. Recompute and report affected feature health and verified dimensions. Preserve unrelated geometry and edits. Do not save or export unless I provide the destination.

## Team record

After a modeling session, add a brief English Notion Daily Log entry: related team task, work completed, next step, blocker, and links to the script, design revision, and validation evidence. Link reviewed scripts and records in GitHub. Keep large native CAD in the agreed shared storage. A photo can supplement the log when appropriate; it is not required for the MCP connection test.

Do not edit one shared Fusion document concurrently without coordination. Identify the current editor and target revision through the team's normal communication route before shared changes.

## Sources and maintenance

Official references checked on October 21, 2026. The executing AI must recheck the current Autodesk documentation and its local client schema.

- [Autodesk Fusion MCPs Overview](https://help.autodesk.com/view/fusion360/ENU/?guid=FMCP-OVERVIEW)
- [Connecting to the Autodesk Fusion MCP Server](https://help.autodesk.com/view/fusion360/ENU/?guid=ADSKMCP_FusionDesktopMcp_connecting_to_the_fusion_mcp_server_html)

This guide does not certify deployment or live CAD execution on any member's machine.
