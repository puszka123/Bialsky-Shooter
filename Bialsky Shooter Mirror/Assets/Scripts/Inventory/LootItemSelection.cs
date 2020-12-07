using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule
{
    public class LootItemSelection : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<Guid> clientOnItemSelected;

        public Guid itemId;

        public void OnPointerClick(PointerEventData eventData)
        {
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1,1,1,0);
            image.sprite = null;
            clientOnItemSelected?.Invoke(itemId);
        }
    }
}
