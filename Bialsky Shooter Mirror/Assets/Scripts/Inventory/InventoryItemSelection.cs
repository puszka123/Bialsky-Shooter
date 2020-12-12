using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItemSelection : MonoBehaviour
{
    public static event Action<Guid> clientOnItemSelected;
    public static event Action<Guid> clientOnItemCleared;
    public static event Action<Guid> clientOnItemInjected;
    public Guid itemId;
    RectTransform rect;

    private void Start() 
    {
        rect = GetComponent<RectTransform>();
        Draggable.clientOnEndDrag += OnEndDrag;
        InitInputSystem();
    }

    private void OnDestroy()
    {
        Draggable.clientOnEndDrag -= OnEndDrag;
    }

    private void InitInputSystem()
    {
        Controls controls = new Controls();
        controls.Player.Inventory.performed += InventoryPerformed;
        controls.Enable();
    }

    private void InventoryPerformed(InputAction.CallbackContext ctx)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Mouse.current.position.ReadValue())) return;
        var itemId = ClearItem(this);
        clientOnItemSelected?.Invoke(itemId);
    }

    private void OnEndDrag(Draggable draggable)
    {
        if (!draggable.TryGetComponent<InventoryItemSelection>(out InventoryItemSelection inventoryItemSelection)) return;
        if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Mouse.current.position.ReadValue())) return; 
        var itemId = inventoryItemSelection.itemId;
        var sprite = inventoryItemSelection.transform.GetChild(0).GetComponent<Image>().sprite;
        ClearItem(inventoryItemSelection);  
        InjectItem(itemId, sprite);
    }

    Guid ClearItem(InventoryItemSelection inventoryItemSelection)
    {
        var itemId = inventoryItemSelection.itemId;
        clientOnItemCleared?.Invoke(itemId);
        var image = inventoryItemSelection.transform.GetChild(0).GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        image.sprite = null;
        inventoryItemSelection.itemId = Guid.Empty;
        return itemId;
    }

    void InjectItem(Guid itemId, Sprite icon)
    {
        var image = transform.GetChild(0).GetComponent<Image>();
        image.color = new Color(1, 1, 1, 1);
        image.sprite = icon;
        this.itemId = itemId;
        clientOnItemInjected?.Invoke(itemId);
    }
}
