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

        public Image GetItemImage()
        {
            return transform.GetChild(0).GetComponent<Image>();
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
            InjectItem(itemSlot.GetItemId(), itemSlot.GetItemImage().sprite);
        }

        public Guid ClearItem()
        {
            var clearedItemId = itemId;
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            clientOnItemSelected?.Invoke(itemId);
            itemId = Guid.Empty;
            return clearedItemId;
        }
    }
}
