# Phase 01 — Target FPS audit

## Context links
- Parent plan: [plan.md](./plan.md)
- Research: [researcher-01-existing-inventory-analysis.md](./research/researcher-01-existing-inventory-analysis.md)
- Scouter: [scouter-01-codebase-inventory-map.md](./scouter/scouter-01-codebase-inventory-map.md)
- Report: [01-migration-strategy.md](./reports/01-migration-strategy.md)

## Overview
- Date: 2026-03-15
- Description: Inspect the destination FPS repo before porting any code.
- Priority: P1
- Implementation status: pending
- Review status: pending

## Key Insights
- The current source inventory is monolithic; the target repo audit decides how much glue code is needed.
- The most important unknowns are input ownership, look/camera ownership, HUD structure, and held-item anchor.

## Requirements
- Identify the player controller script(s).
- Identify the active input backend.
- Identify how cursor lock is handled.
- Identify whether number keys, `E`, `G`, or `B` are already used.
- Identify or create an FPS held-item anchor.
- Identify where HUD canvases should live.

## Architecture
Create a migration fit matrix with 4 columns:
- source expectation
- target equivalent
- action needed
- risk level

Minimum surfaces to map:
- gameplay input lock
- look/camera lock
- pickup/interact key
- hotbar key ownership
- inventory root UI
- overlay UI
- held-item anchor

## Related code files
Destination repo files to locate first:
- player controller
- input actions / input manager
- mouse look / camera rig
- HUD prefab or UI scene
- interact system
- weapon/item holder or camera anchor

## Implementation Steps
1. Open target repo.
2. Locate player root, controller, and camera scripts.
3. Trace input ownership for move/look/interact/hotbar keys.
4. Locate or create HUD canvases.
5. Locate or create held-item anchor below camera or arms rig.
6. Build migration fit matrix.
7. Freeze phase-1 scope based on audit findings.

## Todo list
- [ ] Find player controller entry point
- [ ] Find active input system
- [ ] Find mouse look/camera ownership
- [ ] Find existing interact key conflicts
- [ ] Find or create HUD roots
- [ ] Find or create held-item anchor
- [ ] Decide if save/load is in scope

## Success Criteria
- Every third-person dependency from source inventory has a known FPS replacement path.
- No unknown runtime owner remains for input, camera, cursor, HUD, or held item anchor.

## Risk Assessment
- High risk if target repo has multiple controllers or multiple camera ownership paths.
- Medium risk if target repo already uses number keys for weapons.
- Medium risk if target HUD is world-space or camera-space only.

## Security Considerations
- None beyond normal safe input handling.
- Keep migration local and reversible until parity is proven.

## Next steps
Proceed to Phase 02 only after the target repo audit resolves the 4 critical seams: input lock, camera lock, pickup targeting, held-item presentation.