# Phase 02 — Inventory core port

## Context links
- Parent plan: [plan.md](./plan.md)
- Research: [researcher-01-existing-inventory-analysis.md](./research/researcher-01-existing-inventory-analysis.md)
- Scouter: [scouter-01-codebase-inventory-map.md](./scouter/scouter-01-codebase-inventory-map.md)

## Overview
- Date: 2026-03-15
- Description: Port the reusable inventory core into the FPS repo while preserving behavior.
- Priority: P1
- Implementation status: pending
- Review status: pending

## Key Insights
- `ItemSO`, `Item`, and most of `Slot` are low-risk copies.
- `Inventory.cs` should be brought over in compatibility mode, not re-authored from zero.
- Phase 2 is about preserving rules, not cleaning architecture.

## Requirements
- Port the custom inventory classes, not the Devion package.
- Preserve current stack logic and drag/drop behavior.
- Preserve hotbar selection rules.
- Preserve world drop rules including recent-drop grace window.

## Architecture
### Copy nearly unchanged
- `ItemSO.cs`
- `Item.cs`

### Copy with light edits
- `Slot.cs`
  - optionally replace child-index references with serialized refs

### Copy then adapt
- `Inventory.cs`
  - keep internal flow intact initially
  - allow temporary stubbing of held-item presentation if FPS anchor is not ready

## Related code files
Source:
- `Assets/Scripts/Inventory/Inventory.cs`
- `Assets/Scripts/Inventory/ItemSO.cs`
- `Assets/Scripts/Inventory/Item.cs`
- `Assets/Scripts/Inventory/Slot.cs`

Optional reference only:
- `Assets/Scripts/Missions/CollectibleItem.cs`
- `Assets/Scripts/ConversationUIHotbarCoordinator.cs`

## Implementation Steps
1. Copy `ItemSO.cs`.
2. Copy `Item.cs`.
3. Copy `Slot.cs` and align slot prefab hierarchy or serialize refs.
4. Rebuild or recreate the slot prefab in the target repo.
5. Rebuild the hotbar/inventory roots required by `Inventory.cs`.
6. Copy `Inventory.cs`.
7. Make the smallest changes required to compile in the target repo.
8. Keep all business rules intact.

## Todo list
- [ ] Port `ItemSO.cs`
- [ ] Port `Item.cs`
- [ ] Port `Slot.cs`
- [ ] Recreate slot prefab
- [ ] Recreate `hotbarObj`
- [ ] Recreate `inventorySlotParent`
- [ ] Recreate `container`
- [ ] Recreate `dragIcon`
- [ ] Port `Inventory.cs`
- [ ] Get project compiling before major rewrites

## Success Criteria
- Inventory code compiles in target repo.
- Add/stack works.
- Slots render correctly.
- Drag/drop path still functions.
- Hotbar selection still functions.
- Drop-to-world path still functions.

## Risk Assessment
- Medium risk if target prefab hierarchy does not match `Slot.cs` assumptions.
- High risk if `Inventory.cs` fails to compile due to hard references to third-person packages.

## Security Considerations
- Keep runtime item spawning limited to trusted prefab references from `ItemSO`.
- Do not add arbitrary resource loading during migration.

## Next steps
Once core behavior compiles, move immediately to Phase 03 and replace third-person seams without broad refactoring.