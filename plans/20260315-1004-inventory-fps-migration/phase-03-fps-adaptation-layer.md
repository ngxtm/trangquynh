# Phase 03 — FPS adaptation layer

## Context links
- Parent plan: [plan.md](./plan.md)
- Research: [researcher-01-existing-inventory-analysis.md](./research/researcher-01-existing-inventory-analysis.md)
- Scouter: [scouter-01-codebase-inventory-map.md](./scouter/scouter-01-codebase-inventory-map.md)
- Depends on: [phase-02-inventory-core-port.md](./phase-02-inventory-core-port.md)

## Overview
- Date: 2026-03-15
- Description: Replace the source project’s third-person couplings with first-person equivalents.
- Priority: P1
- Implementation status: pending
- Review status: pending

## Key Insights
- Only 4 seams matter in the first pass: gameplay lock, camera/look lock, pickup targeting, and held-item presentation.
- Everything else should remain as close as possible to source behavior.

## Requirements
- Remove runtime dependency on `StarterAssetsInputs`.
- Remove runtime dependency on `CinemachineFreeLook` and `CinemachineInputProvider` unless the target repo actually uses them.
- Replace third-person pickup tuning with a true FPS interaction origin.
- Replace bone/object-name-based held-item attachment with explicit anchor wiring.

## Architecture
### Seam 1 — gameplay/input lock adapter
Responsibilities:
- disable move
- disable look
- unlock cursor
- restore all 3 on inventory close

Recommended shape:
- one thin adapter around target controller/input stack
- no `FindObjectOfType` spam
- no direct knowledge of source project packages

### Seam 2 — camera/look lock adapter
Responsibilities:
- stop camera look while inventory is open
- avoid camera drift or competing input maps

### Seam 3 — pickup interactor
Recommended FPS rule set:
- primary: center-screen raycast from active FPS camera
- keep range check
- keep line-of-sight check
- reduce or remove third-person compensation logic
- keep nearest/fallback search only if the target game actually needs it

### Seam 4 — equipped-item presenter
Recommended FPS rule set:
- explicit `equippedItemAnchor`
- parent held item under camera or arms rig
- `handItemPrefab` becomes FPS-held prefab
- world `itemPrefab` remains world object
- no lookup by `Wrist_R`
- no lookup by `FREE GREAT SWORD 3 COLOR 2`

## Related code files
Source methods to replace or adapt:
- `ApplyStarterAssetsInputState`
- `ApplyCinemachineInventoryState`
- `Pickup`
- `DetectLookedAtItem`
- `TryGetLookedAtItem`
- `TryGetItemNearScreenCenter`
- `ResolvePickupCamera`
- `GetPickupRayDistance`
- `RefreshEquippedHandItem`
- `ApplyEquippedHandItem`
- `ResolveHandAttachPoint`
- `ResolveDefaultHeldVisual`
- `CacheEquippedHandPose`

## Implementation Steps
1. Replace gameplay lock path with target controller/input calls.
2. Replace look/camera disable path with target camera owner.
3. Simplify pickup ray origin to FPS camera.
4. Keep add/stack side unchanged.
5. Add explicit `equippedItemAnchor`.
6. Route held item spawning to `handItemPrefab` where available.
7. Remove all runtime assumptions tied to `Wrist_R` and default sword object names.

## Todo list
- [ ] Implement gameplay lock adapter
- [ ] Implement camera/look lock adapter
- [ ] Replace pickup origin with FPS camera origin
- [ ] Re-test interact distance and occlusion
- [ ] Add `equippedItemAnchor`
- [ ] Route held visuals to FPS anchor
- [ ] Remove third-person lookup fallbacks

## Success Criteria
- Inventory opens without player movement/look.
- Cursor state behaves correctly.
- Pickup works from camera center in FPS.
- Held item, if enabled, spawns under explicit FPS anchor.
- No runtime dependency remains on source third-person packages or object-name hacks.

## Risk Assessment
- High risk if target project has multiple input maps or multiple camera owners.
- Medium risk if interact key is already claimed by another system.
- Medium risk if held prefabs are not authored for first-person view.

## Security Considerations
- Keep item selection and item spawning sourced from trusted `ItemSO` references only.
- Do not rely on scene string lookups for authoritative gameplay objects.

## Next steps
After FPS seams are stable, move to Phase 04 and make the HUD responsive and resolution-safe.