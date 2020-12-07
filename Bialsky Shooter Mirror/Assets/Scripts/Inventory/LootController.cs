using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.InventoryModule
{
    [RequireComponent(typeof(Inventory))]
    public class LootController : NetworkBehaviour
    {
        [SerializeField] GameObject lootDisplayPrefab = default;
        [SerializeField] LayerMask layerMask = new LayerMask();
        Inventory inventory;
        Inventory loot;
        GameObject lootDisplayInstance;

        #region Client

        [ClientCallback]
        private void Start()
        {
            inventory = GetComponent<Inventory>();
            Controls controls = new Controls();
            controls.Player.Loot.performed += LootPerformed;
            controls.Enable();
        }

        [Client]
        private void LootPerformed(InputAction.CallbackContext ctx)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;
            if (!hit.transform.TryGetComponent<Inventory>(out loot)) return;
            //inventory.ClientLootItem(loot.GetComponent<NetworkIdentity>(), loot.GetFirstItemId());
            lootDisplayInstance = Instantiate(lootDisplayPrefab);
            var slotsDisplay = lootDisplayInstance.GetComponentInChildren<SlotsDisplay>();
            slotsDisplay.Display(loot.ItemDisplays);
        }

        #endregion
    }
}