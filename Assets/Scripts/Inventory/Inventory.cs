using System.Collections.Generic;
using DialogueEditor;
using GinjaGaming.FinalCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemSO chopstickItem;
    public ItemSO bagItem;
    public GameObject hotbarObj;
    public GameObject inventorySlotParent;
    public GameObject container;
    public Image dragIcon;
    public float pickupRange = 3f;
    public Material highlightMaterial;
    public Material originalMaterial;
    public float equippedOpacity = 0.9f;
    public float normalOpacity = 0.58f;
    public Transform hand;
    public GameObject itemDescriptionParent;
    public Image itemDescriptionImage;
    public TextMeshProUGUI descriptionItemNameTxt;
    public TextMeshProUGUI itemDescriptionTxt;
    private GameObject currentHandItem;
    private Renderer lookedAtRenderer = null;
    private int equippedHotbarIndex = 0;
    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private Slot draggedSlot;
    private bool isDragging = false;
    private PlayerController playerController;

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
        playerController = FindAnyObjectByType<PlayerController>();

        if (dragIcon != null)
        {
            dragIcon.raycastTarget = false;
            dragIcon.enabled = false;
        }

        if (container != null)
        {
            container.SetActive(false);
        }

        if (itemDescriptionParent != null)
        {
            itemDescriptionParent.SetActive(false);
        }

        ApplyInventoryState();
        UpdateHotbarOpacity();
        EquipHandItem();
    }

    void Update()
    {
        bool isConversationOpen = ConversationManager.Instance != null && ConversationManager.Instance.IsConversationActive;

        ApplyInventoryState();

        if (Input.GetKeyDown(KeyCode.Tab) && !isConversationOpen)
        {
            container.SetActive(!container.activeInHierarchy);
            ApplyInventoryState();
        }

        if (container != null && container.activeInHierarchy)
        {
            StartDrag();
            UpdateDragItemPosition();
            EndDrag();
            UpdateItemDescription();
            return;
        }

        if (isConversationOpen)
        {
            return;
        }

        UpdateItemDescription();
        DetectLookedAtItem();
        Pickup();
        HandleHotBarSelection();
        HandleDropEquippedItem();
        UpdateHotbarOpacity();
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;
        foreach (Slot slot in allSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int amountCanAdd = Mathf.Min(maxStack - currentAmount, remaining);
                    slot.SetItem(itemToAdd, currentAmount + amountCanAdd);
                    remaining -= amountCanAdd;

                    if (remaining <= 0)
                    {
                        return;
                    }
                }
            }
        }

        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem())
            {
                int amountCanAdd = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountCanAdd);
                remaining -= amountCanAdd;

                if (remaining <= 0)
                {
                    return;
                }
            }
        }

        if (remaining > 0)
        {
            Debug.Log("Not enough space in inventory for " + remaining + " " + itemToAdd.itemName);
        }
    }

    public int GetTotalAmount(ItemSO item)
    {
        if (item == null)
        {
            return 0;
        }

        int total = 0;
        foreach (Slot slot in allSlots)
        {
            if (IsMatchingItem(slot.GetItem(), item))
            {
                total += slot.GetAmount();
            }
        }

        return total;
    }

    public bool HasItemAmount(ItemSO item, int requiredAmount = 1)
    {
        return GetTotalAmount(item) >= requiredAmount;
    }

    public bool RemoveItem(ItemSO item, int amountToRemove = 1)
    {
        if (item == null || amountToRemove <= 0)
        {
            return false;
        }

        if (!HasItemAmount(item, amountToRemove))
        {
            return false;
        }

        int remaining = amountToRemove;
        foreach (Slot slot in allSlots)
        {
            if (!IsMatchingItem(slot.GetItem(), item))
            {
                continue;
            }

            int slotAmount = slot.GetAmount();
            if (slotAmount <= remaining)
            {
                remaining -= slotAmount;
                slot.ClearSlot();
            }
            else
            {
                slot.RemoveAmount(remaining);
                remaining = 0;
            }

            if (remaining <= 0)
            {
                EquipHandItem();
                return true;
            }
        }

        EquipHandItem();
        return remaining <= 0;
    }

    private bool IsMatchingItem(ItemSO slotItem, ItemSO targetItem)
    {
        if (slotItem == null || targetItem == null)
        {
            return false;
        }

        if (slotItem == targetItem)
        {
            return true;
        }

        return slotItem.itemName == targetItem.itemName;
    }

    private void StartDrag()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Slot hovered = GetHoveredSlot();

        if (hovered != null && hovered.HasItem())
        {
            draggedSlot = hovered;
            isDragging = true;

            dragIcon.enabled = true;
            dragIcon.sprite = hovered.GetItem().icon;
            dragIcon.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    private void EndDrag()
    {
        if (!Input.GetMouseButtonUp(0) || !isDragging)
        {
            return;
        }

        Slot hovered = GetHoveredSlot();

        if (hovered != null)
        {
            HandleDrop(draggedSlot, hovered);
            EquipHandItem();
        }

        isDragging = false;
        draggedSlot = null;
        dragIcon.enabled = false;
    }

    private Slot GetHoveredSlot()
    {
        if (EventSystem.current == null)
        {
            return null;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Slot slot = result.gameObject.GetComponentInParent<Slot>();
            if (slot != null)
            {
                return slot;
            }
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to)
    {
        if (to == from)
            return;

        if (to.HasItem() && to.GetItem() == from.GetItem())
        {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetAmount();

            if (space > 0)
            {
                int move = Mathf.Min(space, from.GetAmount());
                to.SetItem(to.GetItem(), to.GetAmount() + move);
                from.SetItem(from.GetItem(), from.GetAmount() - move);
                
                if (from.GetAmount() <= 0)
                {
                    from.ClearSlot();
                }
                return;
            }
        }

        if (to.HasItem())
        {
            ItemSO tempItem = to.GetItem();
            int tempAmount = to.GetAmount();

            to.SetItem(from.GetItem(), from.GetAmount());
            from.SetItem(tempItem, tempAmount);
            return;
        }
        
        to.SetItem(from.GetItem(), from.GetAmount());
        from.ClearSlot();
    }

    private void UpdateDragItemPosition()
    {
        if (isDragging)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void UpdateHotbarOpacity()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();
            if (icon != null)
            {
                icon.color = i == equippedHotbarIndex
                    ? new Color(1f, 1f, 1f, equippedOpacity)
                    : new Color(1f, 1f, 1f, normalOpacity);
            }
        }
    }

    private void EquipHandItem()
    {
        if (currentHandItem != null)
        {
            Destroy(currentHandItem);
            currentHandItem = null;
        }

        if (hand == null || equippedHotbarIndex < 0 || equippedHotbarIndex >= hotbarSlots.Count)
        {
            return;
        }

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];
        if (!equippedSlot.HasItem())
        {
            return;
        }

        ItemSO item = equippedSlot.GetItem();
        if (item == null || item.handItemPrefab == null)
        {
            return;
        }

        currentHandItem = Instantiate(item.handItemPrefab, hand);
        currentHandItem.transform.localPosition = Vector3.zero;
        currentHandItem.transform.localRotation = Quaternion.identity;
    }

    private void HandleHotBarSelection()
    {
        int selectableSlotCount = Mathf.Min(hotbarSlots.Count, 9);

        for (int i = 0; i < selectableSlotCount; i++)
        {
            KeyCode key = KeyCode.Alpha1 + i;
            if (Input.GetKeyDown(key))
            {
                equippedHotbarIndex = i;
                UpdateHotbarOpacity();
                EquipHandItem();
                return;
            }
        }
    }

    private void HandleDropEquippedItem()
    {
        if (!Input.GetKeyDown(KeyCode.G))
        {
            return;
        }

        if (equippedHotbarIndex < 0 || equippedHotbarIndex >= hotbarSlots.Count)
        {
            return;
        }

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];
        if (!equippedSlot.HasItem())
        {
            return;
        }

        ItemSO itemSO = equippedSlot.GetItem();
        GameObject prefab = itemSO.itemPrefab;
        if (prefab == null || Camera.main == null)
        {
            return;
        }

        Vector3 dropPosition = Camera.main.transform.position + (Camera.main.transform.forward * 1.5f) + (Vector3.up * 0.25f);
        GameObject dropped = Instantiate(prefab, dropPosition, Quaternion.identity);

        Item item = dropped.GetComponent<Item>();
        if (item == null)
        {
            item = dropped.AddComponent<Item>();
        }

        Collider droppedCollider = dropped.GetComponentInChildren<Collider>();
        if (droppedCollider == null)
        {
            MeshCollider meshCollider = dropped.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }

        Rigidbody droppedRigidbody = dropped.GetComponent<Rigidbody>();
        if (droppedRigidbody == null)
        {
            droppedRigidbody = dropped.AddComponent<Rigidbody>();
        }

        droppedRigidbody.useGravity = true;
        droppedRigidbody.isKinematic = false;

        item.item = itemSO;
        item.amount = equippedSlot.GetAmount();

        equippedSlot.ClearSlot();
        EquipHandItem();
    }

    private void Pickup()
    {
        if (lookedAtRenderer != null && Input.GetKeyDown(KeyCode.E))
        {
            Item item = lookedAtRenderer.GetComponentInParent<Item>();
            if (item != null)
            {
                AddItem(item.item, item.amount);
                Destroy(item.gameObject);
                EquipHandItem();
            }
        }
    }

    private void DetectLookedAtItem()
    {
        if (lookedAtRenderer != null)
        {
            lookedAtRenderer.material = originalMaterial;
            lookedAtRenderer = null;
            originalMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Item item = hit.collider.GetComponentInParent<Item>();
            if (item != null)
            {
                Renderer rend = item.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    lookedAtRenderer = rend;
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                }
            }
        }
    }

    private void UpdateItemDescription()
    {
        if (itemDescriptionParent == null)
        {
            return;
        }

        bool isInventoryOpen = container != null && container.activeInHierarchy;
        if (!isInventoryOpen)
        {
            itemDescriptionParent.SetActive(false);
            return;
        }

        Slot hoveredSlot = GetHoveredSlot();
        if (hoveredSlot != null)
        {
            ItemSO hoveredItem = hoveredSlot.GetItem();
            if (hoveredItem != null)
            {
                itemDescriptionParent.SetActive(true);

                if (itemDescriptionImage != null)
                {
                    itemDescriptionImage.sprite = hoveredItem.icon;
                }

                if (itemDescriptionTxt != null)
                {
                    itemDescriptionTxt.text = hoveredItem.description;
                }

                if (descriptionItemNameTxt != null)
                {
                    descriptionItemNameTxt.text = hoveredItem.itemName;
                }

                return;
            }
        }

        itemDescriptionParent.SetActive(false);
    }

    private void ApplyInventoryState()
    {
        bool isInventoryOpen = container != null && container.activeInHierarchy;
        bool isConversationOpen = ConversationManager.Instance != null && ConversationManager.Instance.IsConversationActive;
        bool shouldUnlockCursor = isInventoryOpen || isConversationOpen;

        if (playerController != null)
        {
            playerController.SetCameraControlEnabled(!shouldUnlockCursor);
        }

        if (shouldUnlockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
