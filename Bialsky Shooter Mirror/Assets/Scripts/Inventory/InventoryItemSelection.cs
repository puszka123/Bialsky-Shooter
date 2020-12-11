using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSelection : MonoBehaviour, IPointerClickHandler
{
    public static event Action<Guid> clientOnItemSelected;
    public Guid itemId;

    private void Start() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        var image = transform.GetChild(0).GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        image.sprite = null;
        clientOnItemSelected?.Invoke(itemId);
        itemId = Guid.Empty;
    }
}
