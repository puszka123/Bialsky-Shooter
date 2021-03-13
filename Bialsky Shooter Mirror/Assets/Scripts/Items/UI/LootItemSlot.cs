using BialskyShooter.ItemSystem;
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
        public ItemInformation itemInformation;
        bool readOnlyMode = default;

        private void Start() { }

        public void OnPointerClick(PointerEventData eventData)
        {
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        void InjectItem(ItemInformation itemInformation)
        {
            if (readOnlyMode) return;
            var image = transform.GetChild(1).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = Resources.Load<Sprite>(itemInformation.iconPath);
            this.itemInformation = itemInformation;
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void DragInItem(IItemSlot itemSlot)
        {
            InjectItem(itemSlot.GetItemInformation());
            clientOnItemDraggedIn?.Invoke(itemSlot.GetItemId());
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
            var clearedItemId = itemInformation.ItemId;
            var image = transform.GetChild(1).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            itemInformation = null;
            return clearedItemId;
        }

        public void SetItemVisibility(bool visibility)
        {
            transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, visibility ? 1 : 0);
        }

        public ItemInformation GetItemInformation()
        {
            return itemInformation;
        }

        public Guid GetItemId()
        {
            return itemInformation?.ItemId ?? Guid.Empty;
        }

        public void Stack(Guid sourceItemId, int count)
        {
            itemInformation.count += count;
            GetComponent<ItemCountDisplay>().SetCount(itemInformation.count);
        }
    }
}
