using BialskyShooter.ItemSystem;
using BialskyShooter.ItemSystem.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule.UI
{
    public class LootDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform slotsPanel = null;
        [SerializeField] RectTransform mainPanel = null;
        [SerializeField] GameObject slotImagePrefab = null;
        [SerializeField] Canvas canvas = null;
        int rowsCount;
        int columnsCount;
        List<ItemInformation> itemDisplays;
        List<GameObject> slots;

        public Inventory Loot { get; private set; }

        private void Start()
        {
            LootItemSlot.clientOnItemSelected += OnLootItemSelected;
        }

        private void OnDestroy()
        {
            LootItemSlot.clientOnItemSelected -= OnLootItemSelected;
        }

        private void OnLootItemSelected(Guid itemId)
        {
            var slot = slots.FirstOrDefault(e => e.GetComponent<LootItemSlot>().GetItemId() == itemId);
            if (slot != null)
            {
                SetItemInformationToggle(slot, null);
            }
        }

        public void Display(Inventory loot)
        {
            if (loot.SyncItemInformations == null) return;
            Loot = loot;
            itemDisplays = new List<ItemInformation>(loot.SyncItemInformations);
            if (itemDisplays.Count == 0) return;
            var count = itemDisplays.Count;
            rowsCount = count / 2;
            columnsCount = count / 2;
            rowsCount += count % 2;
            columnsCount += count % 2;
            if (columnsCount == 0) columnsCount = 1;
            if (rowsCount == 0) rowsCount = 1;
            if(count == 2)
            {
                rowsCount = 1;
                columnsCount = 2;
            }
            InitLootPanel();
            SetMainPanelPositionToMousePosition();
        }

        void InitLootPanel()
        {
            var slotRect = slotImagePrefab.GetComponent<RectTransform>();
            slots = new List<GameObject>();
            ComputePanelSize(slotRect);
            PlaceSlots(slotRect);
        }

        private void SetMainPanelPositionToMousePosition()
        {
            float maxCanvasWidth = canvas.GetComponent<RectTransform>().rect.width - mainPanel.rect.width;
            float maxCanvasHeight = canvas.GetComponent<RectTransform>().rect.height - mainPanel.rect.height;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 lerp = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            mainPanel.anchoredPosition = new Vector2(
                Mathf.Lerp(0f, maxCanvasWidth, lerp.x),
                -Mathf.Lerp(0f, maxCanvasHeight, 1 - lerp.y));
        }

        private void PlaceSlots(RectTransform slotRect)
        {
            var index = 0;
            for (int row = 1; row <= rowsCount; row++)
            {
                for (int column = 1; column <= columnsCount; column++)
                {
                    if (index >= itemDisplays.Count) return;
                    var slotInstance = Instantiate(slotImagePrefab, slotsPanel);
                    slots.Add(slotInstance);
                    slotInstance.AddComponent<LootItemSlot>();
                    DisplayItem(slotInstance, itemDisplays[index]);
                    SetLootItemSelection(slotInstance, itemDisplays[index]);
                    SetItemInformationToggle(slotInstance, itemDisplays[index]);
                    index++;
                }
            }
        }

        private void SetLootItemSelection(GameObject slotInstance, ItemInformation itemInformation)
        {
            var slotItemSelection = slotInstance.GetComponent<LootItemSlot>();
            slotItemSelection.itemInformation = itemInformation;
        }

        private void SetItemInformationToggle(GameObject slot, ItemInformation itemInformation)
        {
            if (slot == null || slot.GetComponent<ItemInformationTooltip>() == null) return;
            slot.GetComponent<ItemInformationTooltip>().SetItemInformation(itemInformation);
        }

        private void DisplayItem(GameObject slotInstance, ItemInformation itemInformation)
        {
            var image = slotInstance.transform.GetChild(1).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = Resources.Load<Sprite>(itemInformation.iconPath);
        }

        private void ComputePanelSize(RectTransform slotRect)
        {
            float marginX = Mathf.Abs(slotsPanel.sizeDelta.x);
            float marginY = Mathf.Abs(slotsPanel.sizeDelta.y);
            float w = (slotRect.rect.width + Mathf.Abs(slotRect.anchoredPosition.x)) * columnsCount + marginX;
            float h = (slotRect.rect.height + Mathf.Abs(slotRect.anchoredPosition.y)) * rowsCount + marginY;
            mainPanel.sizeDelta = new Vector2(w, h);
            mainPanel.anchoredPosition = Vector2.zero;
        }

        public void CloseLoot()
        {
            Destroy(gameObject);
        }
    }
}
