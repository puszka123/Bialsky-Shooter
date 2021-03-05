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
        public static event Action<Guid> clientOnItemDraggedIn;
        public static event Action<Guid> clientOnItemDraggedOut;
        public Guid itemId;
        bool readOnlyMode = default;

        private void Start() { }

        public void OnPointerClick(PointerEventData eventData)
        {
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        void InjectItem(Guid itemId, Sprite icon)
        {
            if (readOnlyMode) return;
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = icon;
            this.itemId = itemId;
        }

        public Guid GetItemId()
        {
            return itemId;
        }

        public Sprite GetItemIcon()
        {
            return transform.GetChild(0).GetComponent<Image>().sprite;
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void DragInItem(IItemSlot itemSlot)
        {
            InjectItem(itemSlot.GetItemId(), itemSlot.GetItemIcon());
            clientOnItemDraggedIn?.Invoke(itemId);
        }

        public Guid DragOutItem()
        {
            var itemId = ClearItem();
            clientOnItemDraggedOut?.Invoke(itemId);
            return itemId;
        }

        public Guid ClearItem()
        {
            if (readOnlyMode) return Guid.Empty;
            var clearedItemId = itemId;
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            itemId = Guid.Empty;
            return clearedItemId;
        }

        public void SetItemVisibility(bool visibility)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, visibility ? 1 : 0);
        }
    }
}
