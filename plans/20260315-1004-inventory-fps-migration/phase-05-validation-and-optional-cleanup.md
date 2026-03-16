# Phase 05 — Validation and optional cleanup

## Context links
- Parent plan: [plan.md](./plan.md)
- Depends on: [phase-02-inventory-core-port.md](./phase-02-inventory-core-port.md)
- Depends on: [phase-03-fps-adaptation-layer.md](./phase-03-fps-adaptation-layer.md)
- Depends on: [phase-04-responsive-canvas-and-hud.md](./phase-04-responsive-canvas-and-hud.md)

## Overview
- Date: 2026-03-15
- Description: Validate gameplay parity in the FPS repo and decide whether cleanup/refactor is justified.
- Priority: P1
- Implementation status: pending
- Review status: pending

## Key Insights
- Parity matters more than purity in the first pass.
- Cleanup should happen only after end-to-end behavior is proven.
- The biggest false economy here is refactoring before validation.

## Requirements
- Verify pickup, stack, hotbar, drag/drop, and world drop.
- Verify cursor and gameplay lock behavior.
- Verify responsive UI behavior.
- Verify no source-only third-person dependency remains in runtime path.

## Architecture
### Mandatory validation checklist
1. Pick up 3 item types with `E`.
2. Confirm hotbar fills before regular inventory if that is the preserved behavior.
3. Open inventory with `B` and close with `Escape`.
4. Confirm movement/look stop while inventory is open.
5. Confirm drag/drop supports:
   - move to empty slot
   - swap with occupied slot
   - merge with same-item stack
6. Confirm hotbar `1-6` selection works.
7. Confirm `G` drops equipped item into world.
8. Confirm recent-drop grace prevents instant re-pickup.
9. If held visuals are enabled, confirm they bind to FPS anchor.
10. Confirm UI still works across multiple resolutions.

### Optional cleanup after parity
Only consider after validation passes:
- replace `Slot` child-index lookup with serialized refs
- separate inventory state from UI slot view
- split `Inventory.cs` into smaller services
- rename `itemPrefab` / `handItemPrefab` for clearer semantics

## Related code files
Target repo modified files from prior phases.
Primary source reference remains `Assets/Scripts/Inventory/Inventory.cs` from the source project.

## Implementation Steps
1. Run the full validation checklist.
2. Record failures by category: gameplay, UI, held item, input conflict, resolution issues.
3. Fix only blockers to parity first.
4. Decide if cleanup is worth doing now.
5. If yes, do the smallest safe cleanup.

## Todo list
- [ ] Test pickup
- [ ] Test stacking
- [ ] Test inventory open/close
- [ ] Test move/look lock
- [ ] Test drag/drop
- [ ] Test hotbar select
- [ ] Test world drop
- [ ] Test drop grace window
- [ ] Test held item anchor
- [ ] Test resolutions/aspect ratios
- [ ] Decide whether to refactor now or defer

## Success Criteria
- Inventory behavior matches the source project where intended.
- FPS-specific seams behave naturally in the destination project.
- UI remains stable across tested resolutions.
- No runtime dependency remains on `StarterAssetsInputs`, `CinemachineFreeLook`, `CinemachineInputProvider`, `Wrist_R`, or `FREE GREAT SWORD 3 COLOR 2`.

## Risk Assessment
- High risk if parity is assumed without testing drag/drop and world drop together.
- Medium risk if held item visuals are treated as mandatory too early.
- Medium risk if cleanup starts before keybinding conflicts are solved.

## Security Considerations
- Keep item spawning and held-item instantiation bound to trusted serialized asset references.
- Avoid broad scene searches for authoritative gameplay objects in final cleanup.

## Next steps
If validation passes, either stop and ship the migrated inventory or start a separate follow-up plan for architecture cleanup.