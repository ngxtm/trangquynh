# Researcher 01 — Existing inventory analysis

## Scope
Analyze the active inventory implementation currently used in this Unity project and identify what is portable vs coupled to third-person gameplay.

## Confirmed active system
Use the custom inventory under:
- `Assets/Scripts/Inventory/Inventory.cs`
- `Assets/Scripts/Inventory/ItemSO.cs`
- `Assets/Scripts/Inventory/Item.cs`
- `Assets/Scripts/Inventory/Slot.cs`

Do not base the migration on `Assets/Devion Games/Inventory System`.

## Core behavior confirmed
### Inventory inputs
`Assets/Scripts/Inventory/Inventory.cs:58-59`
- open inventory: `B`
- close inventory: `Escape`

`Assets/Scripts/Inventory/Inventory.cs:39-56`, `1251-1262`
- hotbar select: `1-6`, numpad `1-6`

`Assets/Scripts/Inventory/Inventory.cs:1264-1301`
- drop equipped item: `G`

`Assets/Scripts/Inventory/Inventory.cs:659-691`
- pick up looked-at item: `E`

### Data shape
`Assets/Scripts/Inventory/ItemSO.cs:5-12`
- `itemName`
- `icon`
- `maxStackSize`
- `itemPrefab`
- `handItemPrefab`

### World item shape
`Assets/Scripts/Inventory/Item.cs:5-108`
- `Item` is world pickup MonoBehaviour
- stores `ItemSO` + `amount`
- auto-adds or resizes root `BoxCollider` from renderer bounds

### Slot state
`Assets/Scripts/Inventory/Slot.cs:7-101`
- slot itself stores held item + amount
- UI is also storage
- hover state used for pointer fallback during drag/drop

### Inventory manager responsibilities
`Assets/Scripts/Inventory/Inventory.cs:11-1724`
Single class manages:
- slot caching
- add/stack logic
- UI drag/drop
- inventory open/close
- EventSystem fallback
- pickup detection and highlight
- hotbar selection
- world drop
- equipped hand visual
- gameplay lock / camera suppression

## Portable parts
### Safe to reuse first
- `ItemSO`
- `Item`
- most of `Slot`
- add/stack logic in `Inventory.AddItem`, `AddItemToMatchingSlots`, `AddItemToEmptySlots`
- drag/drop rules in `HandleDrop`
- drop-to-world prep in `PrepareDroppedItemForPickup`
- recent-drop grace logic

### Portable with minor adaptation
- hotbar selection logic
- inventory open/close state flow
- slot caching, as long as target HUD maps the same references
- object bounds helpers used for safe drop positioning

## Third-person couplings that must be replaced in FPS repo
### Input/controller coupling
`Assets/Scripts/Inventory/Inventory.cs:475-540`
- manipulates `StarterAssetsInputs`
- zeros move/look/jump/sprint
- drives cursor state through Starter Assets

### Camera coupling
`Assets/Scripts/Inventory/Inventory.cs:542-588`
- disables `CinemachineFreeLook`
- disables `CinemachineInputProvider`

### Player rig coupling
`Assets/Scripts/Inventory/Inventory.cs:66-69`, `1493-1674`
- bone name `Wrist_R`
- default held visual name `FREE GREAT SWORD 3 COLOR 2`
- pose derived from current third-person held object
- axe special-case rotation hack

### Pickup interaction tuning
`Assets/Scripts/Inventory/Inventory.cs:731-1182`
- pickup target resolution includes camera + player-root compensation and fallback search tuned for third-person spacing

## Scene/UI wiring facts
The inventory logic expects these 4 references to be wired:
- `hotbarObj`
- `inventorySlotParent`
- `container`
- `dragIcon`

These must exist in the target repo’s HUD or inventory canvas.

## Adjacent integrations worth treating as optional
### `Assets/Scripts/Missions/CollectibleItem.cs`
- optional bridge from collectible trigger to inventory reward
- project-specific because it also talks to `MissionManager`

### `Assets/Scripts/ConversationUIHotbarCoordinator.cs`
- modal UI suppression reference only
- not portable as-is because it depends on `DialogueEditor`, `StarterAssets`, `Cinemachine`, and `PlayerControl`

## Migration conclusion
Do not rewrite the whole inventory first.
Port the custom inventory in compatibility mode, then replace only the FPS-sensitive seams:
1. gameplay/input lock
2. camera/look lock
3. pickup targeting origin
4. equipped-item presentation anchor

## Unresolved questions
- target input backend unknown
- target FPS camera rig unknown
- target HUD structure unknown
- target save/load requirements unknown