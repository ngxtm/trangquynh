# Phase 04 — Responsive canvas and HUD

## Context links
- Parent plan: [plan.md](./plan.md)
- Report: [01-migration-strategy.md](./reports/01-migration-strategy.md)
- Depends on: [phase-02-inventory-core-port.md](./phase-02-inventory-core-port.md)
- Depends on: [phase-03-fps-adaptation-layer.md](./phase-03-fps-adaptation-layer.md)

## Overview
- Date: 2026-03-15
- Description: Build a resolution-safe inventory UI for the destination FPS repo.
- Priority: P1
- Implementation status: pending
- Review status: pending

## Key Insights
- The source inventory logic only needs 4 references: `hotbarObj`, `inventorySlotParent`, `container`, `dragIcon`.
- The destination UI can look better without changing core inventory behavior.
- Canvas mistakes cause most cross-resolution UI failures, not inventory logic itself.

## Requirements
- Inventory UI must scale correctly across common PC resolutions and aspect ratios.
- Drag icon must always render on top.
- HUD must not shift off-screen when resolution changes.
- Slot visuals must remain legible at smaller scales.

## Architecture
### Recommended canvas split
1. `Canvas_HUD`
   - crosshair
   - interaction prompt
   - `HotbarRoot`
2. `Canvas_Inventory`
   - `InventoryContainer`
   - header
   - inventory grid
   - close button
   - optional item detail panel
3. `Canvas_Overlay`
   - `DragIcon`
   - tooltip root

### Canvas settings
Recommended defaults:
- render mode: `Screen Space - Overlay`
- canvas scaler: `Scale With Screen Size`
- reference resolution: `1920 x 1080`
- screen match: `Match Width Or Height`
- match value: `0.5`

### Anchors
- hotbar: bottom-center
- inventory container: center
- close button: top-right within inventory container
- prompt: center-bottom or near crosshair
- drag icon: overlay canvas, free-positioned

### Layout rules
#### Hotbar
- `HorizontalLayoutGroup`
- consistent slot width/height
- fixed spacing
- padded wrapper panel
- 9-slice background

#### Inventory grid
- centered panel
- `GridLayoutGroup` if fixed column count is acceptable
- script-driven cell size if panel width must remain responsive

#### Drag icon
- not inside any layout group
- `raycastTarget = false`
- highest canvas sort order

### Typography and assets
- use TextMeshPro
- avoid heavy auto-size usage
- use 9-sliced sprites for scalable panels
- keep item icon assets square and consistently padded

## Related code files
Target repo UI surfaces to create or modify:
- HUD canvas prefab/scene
- inventory canvas prefab/scene
- overlay canvas or topmost UI root
- slot prefab
- any tooltip or interaction prompt prefabs

## Implementation Steps
1. Create 3-canvas split.
2. Wire `HotbarRoot`, `InventoryContainer`, `InventoryGrid`, and `DragIcon`.
3. Configure canvas scaler settings.
4. Set anchors and margins correctly.
5. Ensure drag icon and tooltip are on the top overlay canvas.
6. Test common resolutions and aspect ratios.
7. Fix clipping, overlap, and off-screen issues before polish.

## Todo list
- [ ] Create `Canvas_HUD`
- [ ] Create `Canvas_Inventory`
- [ ] Create `Canvas_Overlay`
- [ ] Configure scaler to `1920x1080` / `Match 0.5`
- [ ] Anchor hotbar bottom-center
- [ ] Anchor inventory center
- [ ] Move drag icon to overlay canvas
- [ ] Use 9-slice panels
- [ ] Use TextMeshPro for labels and counts
- [ ] Test multiple resolutions

## Success Criteria
- UI remains centered and legible at multiple PC resolutions.
- Hotbar stays aligned at bottom center.
- Inventory panel stays centered.
- Drag icon never disappears behind other UI.
- No manual transform-scale hacks are required.

## Risk Assessment
- Medium risk if target project already uses mixed canvas modes.
- Medium risk if target repo uses world-space UI conventions.
- Low risk if the UI is isolated to inventory/HUD only.

## Security Considerations
- None beyond standard UI interaction safety.

## Next steps
After the HUD is stable, run end-to-end validation and only then decide whether to refactor or extend.