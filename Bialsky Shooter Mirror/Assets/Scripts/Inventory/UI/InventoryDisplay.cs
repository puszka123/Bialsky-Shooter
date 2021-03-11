using BialskyShooter.Core;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using BialskyShooter.ItemSystem.UI;
using BialskyShooter.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule.UI
{
    public class InventoryDisplay : MonoBehaviour, IInventorySlotsContainer
    {
        [SerializeField] RectTransform slotsPanel = null;
        [SerializeField] RectTransform mainPanel = null;
        [SerializeField] GameObject slotImagePrefab = null;
        [SerializeField] int rowsCount = 10;
        [SerializeField] int columnsCount = 10;
        GameObject[] slots;
        Inventory inventory;
        ItemsSlotsJoint itemsSlotsJoint;

        private void Awake()
        {
            InventoryItemSlot.clientOnItemSelected += OnItemSelected;
            InventoryItemSlot.clientOnItemDraggedIn += OnItemDraggedIn;
            ToggleInventoryDisplay.clientOnInventoryToggled += onInventoryToggled;
        }

        void Start()
        {
            SetupInventoryDisplay();
        }

        void OnDestroy()
        {
            InventoryItemSlot.clientOnItemSelected -= OnItemSelected;
            InventoryItemSlot.clientOnItemDraggedIn -= OnItemDraggedIn;
            ToggleInventoryDisplay.clientOnInventoryToggled -= onInventoryToggled;
            itemsSlotsJoint.clientOnItemUnequipped -= OnItemUnequipped;
            itemsSlotsJoint.clientOnItemEquipped -= OnItemEquipped;
            inventory.clientOnInventoryChanged -= OnInventoryChanged;
        }

        private void Update()
        {
            if (itemsSlotsJoint != null && inventory != null) return;
            var player = GetLocalPlayer();
            if (player == null) return;
            itemsSlotsJoint = player.GetComponent<ItemsSlotsJoint>();
            itemsSlotsJoint.clientOnItemUnequipped += OnItemUnequipped;
            itemsSlotsJoint.clientOnItemEquipped += OnItemEquipped;
            inventory = player.GetComponent<Inventory>();
            inventory.clientOnInventoryChanged += OnInventoryChanged;
        }

        private void OnInventoryChanged(ItemInformation itemInformation)
        {
            DisplayItem(itemInformation);
        }

        private void OnItemEquipped(ItemInformation itemInformation)
        {
            if (Guid.Parse(itemInformation.itemId) == Guid.Empty) return;
            StopDisplayItem(Guid.Parse(itemInformation.itemId));
        }

        private void OnItemUnequipped(ItemInformation itemInformation)
        {
            if (Guid.Parse(itemInformation.itemId) == Guid.Empty) return;
            DisplayItem(itemInformation);
        }

        void onInventoryToggled(bool active)
        {
            if (active)
            {
                DisplayInventory();
            }
        }

        void DisplayInventory()
        {
            DisplayInventoryItems();
        }

        GameObject GetLocalPlayer()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("PlayerCharacter"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority) return player;
            }
            return null;
        }

        void OnItemSelected(Guid itemId)
        {

        }

        void OnItemDraggedIn(Guid itemId)
        {
            SetItemInformationTooltip(GetSlotGO(itemId),
                inventory.SyncItemInformations.FirstOrDefault(e => e.ItemId == itemId));
        }

        void SetupInventoryDisplay()
        {
            slots = new GameObject[rowsCount * columnsCount];
            InitInventoryPanel();
        }

        void InitInventoryPanel()
        {
            var slotRect = slotImagePrefab.GetComponent<RectTransform>();
            ComputePanelSize(slotRect);
            PlaceSlots(slotRect);
        }

        public IItemSlot GetFirstAvailableSlot()
        {
            foreach (var slot in slots)
            {
                var inventorySlot = slot.GetComponent<IItemSlot>();
                if (inventorySlot.GetItemInformation() != null) return inventorySlot;
            }
            return null;
        }

        public GameObject GetFirstAvailableSlotGO()
        {
            foreach (var slot in slots)
            {
                var inventorySlot = slot.GetComponent<IItemSlot>();
                if (inventorySlot.GetItemId() == Guid.Empty) return slot;
            }
            return null;
        }

        public GameObject GetSlotGO(Guid itemId)
        {
            return slots
                .FirstOrDefault(s => 
                    s.GetComponent<IItemSlot>()
                    .GetItemId() == itemId);
        }

        void DisplayInventoryItems()
        {
            foreach (var itemInformation in inventory.SyncItemInformations)
            {
                DisplayItem(itemInformation);
            }
        }

        void PlaceSlots(RectTransform slotRect)
        {
            int index = 0;
            for (int row = 1; row <= rowsCount; row++)
            {
                for (int column = 1; column <= columnsCount; column++)
                {
                    var slotInstance = Instantiate(slotImagePrefab, slotsPanel);
                    slotInstance.AddComponent<InventoryItemSlot>();
                    InitSlots(index, slotInstance);
                    ++index;
                }
            }
        }

        void InitSlots(int index, GameObject slotInstance)
        {
            if (slots == null) return;
            slots[index] = slotInstance;
        }

        void SetInventoryItemSlot(GameObject slotInstance, ItemInformation itemInformation)
        {
            var slotItemSlot = slotInstance.GetComponent<InventoryItemSlot>();
            slotItemSlot.itemInformation = itemInformation;
        }

        void UnsetInventoryItemSlot(GameObject slotInstance)
        {
            var slotItemSlot = slotInstance.GetComponent<InventoryItemSlot>();
            slotItemSlot.ClearItem();
        }

        void SetItemInformationTooltip(GameObject slot, ItemInformation itemInformation)
        {
            if (slot == null || slot.GetComponent<ItemInformationTooltip>() == null) return;
            slot.GetComponent<ItemInformationTooltip>().SetItemInformation(itemInformation);
        }

        void UnsetItemInformationTooltip(GameObject slot)
        {
            if (slot == null || slot.GetComponent<ItemInformationTooltip>() == null) return;
            slot.GetComponent<ItemInformationTooltip>().UnsetItemDisplay();
        }

        void DisplayItem(GameObject slotInstance, ItemInformation itemInformation)
        {
            slotInstance.GetComponent<InventoryItemSlot>().InjectItem(itemInformation);
        }

        void DisplayItem(ItemInformation itemInformation)
        {
            if (ItemExists(itemInformation)) return;
            var slot = GetFirstAvailableSlotGO();
            SetInventoryItemSlot(slot, itemInformation);
            DisplayItem(slot, itemInformation);
            SetItemInformationTooltip(slot, itemInformation);
        }

        void StopDisplayItem(Guid itemId)
        {
            var slot = GetSlotGO(itemId);
            if (slot == null) return;
            UnsetInventoryItemSlot(slot);
            UnsetItemInformationTooltip(slot);
        }

        bool ItemExists(ItemInformation itemInformation)
        {
            return slots
                .Where(s => s.GetComponent<InventoryItemSlot>().itemInformation != null)
                .Select(s => s.GetComponent<InventoryItemSlot>().itemInformation.ItemId)
                .Contains(itemInformation.ItemId);
        }

        void ComputePanelSize(RectTransform slotRect)
        {
            float marginX = Mathf.Abs(slotsPanel.sizeDelta.x);
            float marginY = Mathf.Abs(slotsPanel.sizeDelta.y);
            float w = (slotRect.rect.width + Mathf.Abs(slotRect.anchoredPosition.x)) * columnsCount + marginX;
            float h = (slotRect.rect.height + Mathf.Abs(slotRect.anchoredPosition.y)) * rowsCount + marginY;
            mainPanel.sizeDelta = new Vector2(w, h);
            mainPanel.anchoredPosition = Vector2.zero;
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            var slot = GetFirstAvailableSlot();
            slot.DragInItem(itemSlot);
        }

        public IItemSlot GetSlot(Guid itemId)
        {
            return slots.Select(slot => slot.GetComponent<IItemSlot>())
                .FirstOrDefault(slot => slot.GetItemId() == itemId);
        }
    }
}