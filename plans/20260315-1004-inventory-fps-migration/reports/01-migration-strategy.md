# Report 01 — Migration strategy and UI rules

## Strategy in one line
Port the current custom inventory with minimal logic changes, then swap the third-person integration points for FPS-specific adapters.

## Why this strategy wins
- fastest path to parity
- lowest risk of breaking stack/hotbar/drag-drop behavior
- avoids rewriting a monolithic class before behavior is preserved
- lets target repo decide later whether to refactor storage out of UI slots

## Hard rules
1. Do not migrate the Devion inventory package.
2. Do not rewrite inventory architecture in the first pass.
3. Do not bind FPS held items to third-person bones or object names.
4. Do not mix save/load redesign into the initial port unless forced.
5. Keep the target HUD explicit and inspector-wired.

## Recommended target repo usage
### Runtime ownership
- one main `Inventory`
- one main player/controller authority
- one clear gameplay mode vs inventory mode split

### Item semantics
- `itemPrefab` = world pickup / dropped world object
- `handItemPrefab` = first-person held/viewmodel prefab

### Input ownership
Prefer configurable input mappings. Existing defaults from source project:
- `B` open inventory
- `Escape` close inventory
- `E` interact/pickup
- `G` drop equipped item
- `1-6` select hotbar

## UI rules that should not be broken
### Canvases
- `Canvas_HUD`
- `Canvas_Inventory`
- `Canvas_Overlay`

### Canvas settings
- render mode: `Screen Space - Overlay`
- scaler mode: `Scale With Screen Size`
- reference resolution: `1920 x 1080`
- match width/height: start at `0.5`

### Anchors
- hotbar: bottom-center
- inventory container: center
- drag icon: overlay canvas, free-positioned, not inside layout group
- prompt: center-bottom or near crosshair

### Layout
- hotbar: `HorizontalLayoutGroup`
- inventory grid: `GridLayoutGroup` or script-sized responsive cell layout
- panels/backgrounds: 9-slice sprites
- text: TextMeshPro

## Recommended implementation order in target repo
1. audit target controller/camera/input/HUD
2. port `ItemSO`, `Item`, `Slot`
3. rebuild slot prefab + HUD roots
4. port `Inventory.cs` in compile-safe mode
5. replace FPS-sensitive seams
6. test parity
7. only then consider cleanup/refactor

## Cut list for phase 1
Do now:
- pickup
- stacking
- hotbar
- drag/drop
- world drop
- inventory open/close
- optional held item anchor

Do later:
- save/load
- mission integration
- dialogue integration
- viewmodel polish
- architecture cleanup

## Failure modes to watch
- keys conflict with existing FPS controls
- item viewmodel too large or wrong pivot
- drag icon under wrong canvas / wrong sort order
- slot prefab structure mismatches `Slot.cs`
- target project has no clean way to lock look/move separately

## Unresolved questions
- target input stack unknown
- target UI stack unknown
- target save requirement unknown