using BialskyShooter.ClassSystem;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.EquipmentSystem.UI;
using BialskyShooter.InventoryModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.CharacterModule.UI
{
    public class DisplayCharacterInfo : MonoBehaviour
    {
        [SerializeField] Canvas canvas = null;


        public void CloseCharacterInfoDisplay()
        {
            Destroy(gameObject);
        }

        public  void Display(GameObject selectedCreature)
        {
            
            if (selectedCreature == null) return;
            if (!selectedCreature.TryGetComponent(out Equipment equipment)) return;
            if (!selectedCreature.TryGetComponent(out CreatureStats stats)) return;
            if (selectedCreature.TryGetComponent(out LootTarget lootTarget)) return;
            DisplayEquipment(equipment, selectedCreature.GetComponent<EquippingController>());
            DisplayCreatureStats(stats);
            canvas.gameObject.SetActive(true);
        }

        private void DisplayCreatureStats(CreatureStats stats)
        {
            GetComponent<DisplayCharacterInfo>().GetComponent<CreatureStatsDisplay>().SetCreatureStats(stats);
        }

        private void DisplayEquipment(Equipment equipment, EquippingController equippingController)
        {
            var equipmentDisplay = GetComponent<DisplayCharacterInfo>().GetComponent<EquipmentDisplay>();
            if (!equipment.hasAuthority) equipmentDisplay.ReadOnly();
            equipmentDisplay.SetupEquipmentDisplay(equipment, equippingController);
        }
    }
}