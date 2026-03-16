---
title: "Port custom inventory to FPS project"
description: "Compatibility-first migration plan for moving the active custom inventory from this third-person Unity project into another first-person Unity project with stable responsive UI."
status: pending
priority: P1
effort: "5 phases"
branch: minh2
tags: [unity, inventory, migration, fps, canvas, ui]
created: 2026-03-15
---

# Goal
Create a portable plan that can be copied into another Unity FPS repo and used to migrate the active custom inventory system from this project without rewriting inventory rules from scratch.

# Locked decisions
- Port the active custom system under `Assets/Scripts/Inventory`, not the Devion package.
- Use a compatibility-first migration: preserve item/stack/hotbar/drag-drop logic first; replace third-person couplings second.
- Treat save/load, mission flow, and dialogue flow as out of first-pass scope unless the target repo forces them in.
- Build the target UI around 3 canvases with `Screen Space - Overlay` and `Scale With Screen Size`.

# Assumptions
- The target repo has not been inspected yet.
- The target game is first-person and has a single main player/controller path.
- Held item visuals can be anchored under camera or FPS arms rig.

# Phases
| Phase | Status | Progress | File |
|---|---|---:|---|
| 01 | pending | 0% | [Target FPS audit](./phase-01-target-fps-audit.md) |
| 02 | pending | 0% | [Inventory core port](./phase-02-inventory-core-port.md) |
| 03 | pending | 0% | [FPS adaptation layer](./phase-03-fps-adaptation-layer.md) |
| 04 | pending | 0% | [Responsive canvas and HUD](./phase-04-responsive-canvas-and-hud.md) |
| 05 | pending | 0% | [Validation and optional cleanup](./phase-05-validation-and-optional-cleanup.md) |

# Supporting docs
- [Research: existing inventory analysis](./research/researcher-01-existing-inventory-analysis.md)
- [Scouter: codebase inventory map](./scouter/scouter-01-codebase-inventory-map.md)
- [Report: migration strategy and UI rules](./reports/01-migration-strategy.md)

# First-pass non-goals
- Full persistence/save redesign
- Mission-specific collectible rewrites
- Dialogue/UI coordinator rewrites
- Full extraction of inventory state out of UI slots
- Viewmodel polish features like sway/bob/custom camera layers

# Exit condition
The target FPS repo can open/close inventory, pick up items, stack items, drag/drop slots, select hotbar entries, drop equipped items, and optionally display a held item with no runtime dependency on `StarterAssetsInputs`, `CinemachineFreeLook`, `CinemachineInputProvider`, `Wrist_R`, or `FREE GREAT SWORD 3 COLOR 2`.

# Unresolved questions
- What input system/controller stack does the target FPS repo use?
- Does the target repo already reserve `B`, `E`, `G`, or `1-6`?
- Does the target repo require save/load in the same migration pass?
- Is there already an existing HUD/inventory canvas to extend?