# Project records

Copy the appropriate template into the indicated folder when real work begins. Templates are not completed records.

| Record | Location | ID pattern |
| --- | --- | --- |
| Meeting | `records/meetings/` | `MTG-YYYYMMDD-01` |
| Decision | `records/decisions/` | `DEC-0001` |
| Experiment | `records/experiments/` | `RUN-YYYYMMDD-001` |
| Reading note | `references/notes/` | `SRC-0001` |
| Media/release | `media/` | `MEDIA-0001` / `REL-0001` |

Use ISO timestamps with timezone, stable IDs, authors, reviewers, version and status. Link the Notion task and exact inputs. Keep corrections in Git history. Raw media/data stay unchanged; derivatives identify their source hashes and processing revision. Record missing information explicitly.

`templates/manifest.csv` defines one file per row. Hashes must be computed from actual bytes; never invent a hash or replace an original with a processed file under the same identity. Persistent private storage links must work for authorized members and contain no embedded credentials.
