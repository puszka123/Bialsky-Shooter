using BialskyShooter.ItemSystem.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule.UI
{
    public class LootItemSlot : MonoBehaviour, IPointerClickHandler, IItemSlot
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemInjected;
        public Guid itemId;
        bool readOnlyMode = default;

        private void Start() { }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClearItem();
        }

        void InjectItem(Guid itemId, Sprite icon)
        {
            if (readOnlyMode) return;
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = icon;
            this.itemId = itemId;
            clientOnItemInjected?.Invoke(itemId);
        }

        public Guid GetItemId()
        {
            return itemId;
        }

        public Sprite GetItemIcon()
        {
            return transform.GetChild(0).GetComponent<Image>().sprite;
        }

        public void ItemDragged()
        {
            
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            InjectItem(itemSlot.GetItemId(), itemSlot.GetItemIcon());
        }

        public Guid ClearItem()
        {
            if (readOnlyMode) return Guid.Empty;
            var clearedItemId = itemId;
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            clientOnItemSelected?.Invoke(itemId);
            itemId = Guid.Empty;
            return clearedItemId;
        }

        public void SetItemVisibility(bool visibility)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, visibility ? 1 : 0);
        }
    }
}
