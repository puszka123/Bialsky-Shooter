using BialskyShooter.ItemSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule
{
    public class LootItemSelection : MonoBehaviour, IPointerClickHandler, IItemSelection
    {
        public static event Action<Guid> clientOnItemSelected;
        public Guid itemId;
        bool readOnlyMode;

        private void Start() { }

        public void OnPointerClick(PointerEventData eventData)
        {
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1,1,1,0);
            image.sprite = null;
            clientOnItemSelected?.Invoke(itemId);
            itemId = Guid.Empty;
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
    }
}
