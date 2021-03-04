using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BialskyShooter.ItemSystem.UI
{
    public class ItemInformationTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject itemInformationDisplayPrefab = default;
        ItemDisplay itemDisplay;
        GameObject itemInformationDisplayInstance;
        IItemSlot itemSlot;

        private void Update()
        {
            if (itemSlot == null) itemSlot = GetComponent<IItemSlot>();
            if(itemSlot.GetItemId() == Guid.Empty && itemDisplay != null)
            {
                SetItemDisplay(null);
                Destroy(itemInformationDisplayInstance);
            }
        }

        private void OnDestroy()
        {
            Destroy(itemInformationDisplayInstance);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemDisplay == null || itemDisplay.ItemId == Guid.Empty) return;
            itemInformationDisplayInstance = Instantiate(itemInformationDisplayPrefab);
            DisplayItemInformation(itemInformationDisplayInstance.GetComponent<ItemInformationDisplay>());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (itemInformationDisplayInstance == null) return;
            Destroy(itemInformationDisplayInstance);
        }

        public void SetItemDisplay(ItemDisplay itemDisplay)
        {
            this.itemDisplay = itemDisplay;
        }

        public void UnsetItemDisplay()
        {
            SetItemDisplay(null);
            Destroy(itemInformationDisplayInstance);
        }

        private void DisplayItemInformation(ItemInformationDisplay itemInformationDisplay)
        {
            if (itemInformationDisplay == null || itemDisplay == null || itemDisplay.ItemId == Guid.Empty) return;
            itemInformationDisplay.setItemDisplay(itemDisplay, GetComponent<RectTransform>());
            itemInformationDisplay.Display();
        }
    }
}
