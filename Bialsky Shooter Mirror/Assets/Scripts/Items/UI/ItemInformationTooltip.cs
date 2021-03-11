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
        ItemInformation itemInformation;
        GameObject itemInformationDisplayInstance;
        IItemSlot itemSlot;

        private void Update()
        {
            if (itemSlot == null) itemSlot = GetComponent<IItemSlot>();
            if(itemSlot.GetItemInformation() == null && itemInformation != null)
            {
                SetItemInformation(null);
                Destroy(itemInformationDisplayInstance);
            }
        }

        private void OnDestroy()
        {
            Destroy(itemInformationDisplayInstance);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemInformation == null || itemInformation.ItemId == Guid.Empty) return;
            itemInformationDisplayInstance = Instantiate(itemInformationDisplayPrefab);
            DisplayItemInformation(itemInformationDisplayInstance.GetComponent<ItemInformationDisplay>());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (itemInformationDisplayInstance == null) return;
            Destroy(itemInformationDisplayInstance);
        }

        public void SetItemInformation(ItemInformation itemInformation)
        {
            this.itemInformation = itemInformation;
        }

        public void UnsetItemDisplay()
        {
            SetItemInformation(null);
            Destroy(itemInformationDisplayInstance);
        }

        private void DisplayItemInformation(ItemInformationDisplay itemInformationDisplay)
        {
            if (itemInformationDisplay == null || itemInformation == null || itemInformation.ItemId == Guid.Empty) return;
            itemInformationDisplay.SetItemInformation(itemInformation, GetComponent<RectTransform>());
            itemInformationDisplay.Display();
        }
    }
}
