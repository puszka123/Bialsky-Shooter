using BialskyShooter.ClassSystem;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.InventoryModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.CharacterModule
{
    public class DisplayCharacterInfo : NetworkBehaviour
    {
        [SerializeField] GameObject equipmentDisplayPrefab = default;
        [SerializeField] LayerMask layerMask = new LayerMask();
        GameObject equipmentDisplayInstance;

        [ClientCallback]
        void Start()
        {
            if (!hasAuthority) return;
            InitInputSystem();
        }

        private void InitInputSystem()
        {
            Controls controls = new Controls();
            controls.Player.CharacterInfo.performed += CharacterInfoPerformed;
            controls.Enable();
        }

        private void CharacterInfoPerformed(InputAction.CallbackContext ctx)
        {
            var selectedGameObject = GetSelectedGameObject();
            if (selectedGameObject == null) return;
            if (!selectedGameObject.TryGetComponent(out Equipment equipment)) return;
            if (!selectedGameObject.TryGetComponent(out CreatureStats stats)) return;
            if (selectedGameObject.TryGetComponent(out LootTarget lootTarget)) return;
            equipmentDisplayInstance = Instantiate(equipmentDisplayPrefab);
            DisplayEquipment(equipment);
            DisplayCreatureStats(stats);
        }

        private void DisplayCreatureStats(CreatureStats stats)
        {
            equipmentDisplayInstance.GetComponent<CreatureStatsDisplay>().SetCreatureStats(stats);
        }

        private void DisplayEquipment(Equipment equipment)
        {
            
            var equipmentDisplay = equipmentDisplayInstance.GetComponent<EquipmentDisplay>();
            if (!equipment.hasAuthority) equipmentDisplay.ReadOnly();
            foreach (var itemInformation in equipment.ItemInformations)
            {
                equipmentDisplay.DisplayItem(itemInformation);
            }
        }

        private GameObject GetSelectedGameObject()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return null;
            return hit.transform.gameObject;
        }
    }
}