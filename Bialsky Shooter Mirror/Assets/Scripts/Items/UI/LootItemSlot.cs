using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public class LootItemSlot : MonoBehaviour, IPointerClickHandler, IItemSlot
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemDraggedIn;
        public static event Action<Guid> clientOnItemDraggedOut;
        [SerializeField] Image itemImage;
        public ItemInformation itemInformation;
        bool readOnlyMode = default;


        public void OnPointerClick(PointerEventData eventData)
        {
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        void InjectItem(ItemInformation itemInformation)
        {
            if (readOnlyMode) return;
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = Resources.Load<Sprite>(itemInformation.iconPath);
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
            itemImage.color = new Color(1, 1, 1, 0);
            itemImage.sprite = null;
            itemInformation = null;
            return clearedItemId;
        }

        public void SetItemVisibility(bool visibility)
        {
            itemImage.color = new Color(1, 1, 1, visibility ? 1 : 0);
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
