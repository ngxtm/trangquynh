# Scouter 01 — Codebase inventory map

## Primary files
- `Assets/Scripts/Inventory/Inventory.cs`
- `Assets/Scripts/Inventory/ItemSO.cs`
- `Assets/Scripts/Inventory/Item.cs`
- `Assets/Scripts/Inventory/Slot.cs`

## Secondary reference files
- `Assets/Scripts/Missions/CollectibleItem.cs`
- `Assets/Scripts/ConversationUIHotbarCoordinator.cs`

## Inventory.cs structure map
### Config and state
`Assets/Scripts/Inventory/Inventory.cs:13-87`
- public item references: `woodItem`, `axeItem`
- UI references: `hotbarObj`, `inventorySlotParent`, `container`, `dragIcon`
- pickup/drop config
- hotbar key arrays
- inventory toggle keys
- third-person attach config: `handAttachBoneName`, `defaultHeldVisualName`
- slot collections: `inventorySlots`, `hotbarSlots`, `allSlots`

### Lifecycle
`Assets/Scripts/Inventory/Inventory.cs:105-168`
- `Awake`: cache slots, resolve drag icon
- `Start`: sync inventory state, update hotbar opacity, refresh equipped hand item
- `Update`: toggles inventory, runs gameplay path or inventory-interaction path

### Storage / stack rules
`Assets/Scripts/Inventory/Inventory.cs:169-337`
- `AddItem`
- `AddItemToMatchingSlots`
- `AddItemToEmptySlots`
- slot sorting / cache order helpers
- `CacheSlots`

### UI drag/drop
`Assets/Scripts/Inventory/Inventory.cs:339-473`
- `StartDrag`
- `EndDrag`
- `GetHoveredSlot`
- `HandleDrop`
- `UpdateDragItemPosition`
- `ResolveDragIcon`

### Input/cursor/camera lock
`Assets/Scripts/Inventory/Inventory.cs:475-657`
- `SetInventoryInteraction`
- `ApplyStarterAssetsInputState`
- `ApplyCinemachineInventoryState`
- `CacheAndDisableBehaviours`
- `RestoreCameraBehaviours`
- `EnsureInventoryCursorState`
- `EnsureEventSystemForInventory`
- `RestoreEventSystemAfterInventory`
- `ReleaseGameplayLock`
- `StopDrag`

### Pickup and highlight
`Assets/Scripts/Inventory/Inventory.cs:659-1197`
- `Pickup`
- `DetectLookedAtItem`
- `TryGetLookedAtItem`
- `TryGetItemFromRay`
- `TryGetItemNearScreenCenter`
- `ResolvePickupCamera`
- `TryResolvePickupTarget`
- `TryGetClosestPickupableItem`
- `HasLineOfSight`
- `ApplyLookedAtItemHighlight`
- `ResolvePlayerRoot`
- `GetPickupRayDistance`
- range helpers

### Inventory open/close and hotbar visuals
`Assets/Scripts/Inventory/Inventory.cs:1199-1262`
- `ToggleInventory`
- `SetInventoryOpen`
- `IsInventoryOpen`
- `SyncInventoryState`
- `UpdateHotBarOpacity`
- `HandleHotBarSelection`

### Drop equipped item into world
`Assets/Scripts/Inventory/Inventory.cs:1264-1466`
- `HandleDropEquippedItem`
- `PrepareDroppedItemForPickup`
- `MoveDroppedItemClearOfPlayer`
- recent-drop tracking
- object bounds helpers

### Equipped hand item presentation
`Assets/Scripts/Inventory/Inventory.cs:1468-1724`
- `RefreshEquippedHandItem`
- `ApplyEquippedHandItem`
- `ResolveHandVisualPrefab`
- `PrepareHandVisualInstance`
- `ApplyEquippedHandPose`
- `ResolveHandAttachPoint`
- `ResolveDefaultHeldVisual`
- `CacheEquippedHandPose`
- `FindChildRecursive`
- drop spawn helpers

## Fast file classification for migration
### Copy nearly unchanged
- `Assets/Scripts/Inventory/ItemSO.cs`
- `Assets/Scripts/Inventory/Item.cs`

### Copy with light edits
- `Assets/Scripts/Inventory/Slot.cs`

### Copy then adapt heavily
- `Assets/Scripts/Inventory/Inventory.cs`

### Reference only
- `Assets/Scripts/Missions/CollectibleItem.cs`
- `Assets/Scripts/ConversationUIHotbarCoordinator.cs`

## FPS-sensitive seams to replace first
1. `ApplyStarterAssetsInputState`
2. `ApplyCinemachineInventoryState`
3. `TryGetLookedAtItem` / `TryGetItemNearScreenCenter` / `ResolvePickupCamera`
4. `ResolveHandAttachPoint` / `ResolveDefaultHeldVisual` / `ApplyEquippedHandItem`

## Notes for the target repo
Do not start by extracting a clean architecture. First copy enough files to preserve behavior, then replace seams one by one.

## Unresolved questions
- target HUD slot count unknown
- target FPS held-item anchor unknown
- target repo interaction conflicts unknown