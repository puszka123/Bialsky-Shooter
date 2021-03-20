using BialskyShooter.InventoryModule.UI;
using BialskyShooter.ItemSystem.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace BialskyShooter.InventoryModule
{
    [RequireComponent(typeof(Inventory))]
    public class LootController : NetworkBehaviour
    {
        [Inject] Inventory inventory;

        [SerializeField] GameObject lootDisplayPrefab = default;
        [SerializeField] LayerMask layerMask = new LayerMask();
        Inventory loot;
        GameObject lootDisplayInstance;


        #region Client

        [ClientCallback]
        private void Awake()
        {
            LootItemSlot.clientOnItemDraggedOut += OnLootItemSelected;
        }

        [ClientCallback]
        private void Start()
        {
            if (!hasAuthority) return;
            inventory = GetComponent<Inventory>();
            InitInputSystem();
        }

        [ClientCallback]
        private void OnDestroy()
        {
            LootItemSlot.clientOnItemDraggedOut -= OnLootItemSelected;
        }

        private void InitInputSystem()
        {
            Controls controls = new Controls();
            controls.Player.Loot.performed += LootPerformed;
            controls.Enable();
        }

        [Client]
        private void LootPerformed(InputAction.CallbackContext ctx)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;
            if (hit.transform.GetComponent<LootTarget>() == null) return;
            if (!hit.transform.TryGetComponent(out loot)) return;
            if (LootDisplayed(loot)) return;
            lootDisplayInstance = Instantiate(lootDisplayPrefab);
            var slotsDisplay = lootDisplayInstance.GetComponentInChildren<LootDisplay>();
            slotsDisplay.Display(loot);
            LootItemSlot.clientOnItemSelected += OnLootItemSelected;
        }

        bool LootDisplayed(Inventory loot)
        {
            foreach (var item in FindObjectsOfType<LootDisplay>())
            {
                if (item.Loot != null && item.Loot == loot) return true;
            }
            return false;
        }


        [Client]
        void OnLootItemSelected(Guid itemId)
        {
            if (!hasAuthority) return;
            inventory.ClientLootItem(loot.GetComponent<NetworkIdentity>(), itemId);
        }


        #endregion
    }
}