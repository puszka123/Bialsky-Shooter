using BialskyShooter.ItemSystem;
using BialskyShooter.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.EquipmentSystem
{
    public class EquipmentDisplay : MonoBehaviour
    {
        [SerializeField] GameObject weaponSlot;
        [SerializeField] GameObject shieldSlot;
        [SerializeField] GameObject chestSlot;
        [SerializeField] GameObject helmetSlot;
        [SerializeField] GameObject legsSlot;
        [SerializeField] GameObject bootsSlot;
        Equipment localPlayerEquipment;

        private void Start()
        {
            ToggleCharacterInfoDisplay.clientOnCharacterInfoDisplayed += OnCharacterInfoDisplayed;
        }

        private void OnDestroy()
        {
            ToggleCharacterInfoDisplay.clientOnCharacterInfoDisplayed -= OnCharacterInfoDisplayed;
        }

        private void OnCharacterInfoDisplayed()
        {
            if (localPlayerEquipment == null) GetLocalPlayerEquipment();
            if (localPlayerEquipment.ItemInformations == null) return;
            foreach (var item in localPlayerEquipment.ItemInformations)
            {
                DisplayItem(item);
            }
        }

        private void GetLocalPlayerEquipment()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    localPlayerEquipment = player.GetComponent<Equipment>();
                    break;
                }
            }
        }

        public void DisplayItem(ItemInformation itemInformation)
        {
            switch (itemInformation.slotType)
            {
                case ItemSlotType.Weapon:
                    DisplayWeapon(itemInformation);
                    break;
                case ItemSlotType.Shield:
                    DisplayShield(itemInformation);
                    break;
                case ItemSlotType.Helmet:
                    DisplayHelmet(itemInformation);
                    break;
                case ItemSlotType.Chest:
                    DisplayChest(itemInformation);
                    break;
                case ItemSlotType.Legs:
                    DisplayLegs(itemInformation);
                    break;
                case ItemSlotType.Boots:
                    DisplayBoots(itemInformation);
                    break;
                default:
                    break;
            }
        }

        void DisplayWeapon(ItemInformation itemInformation)
        {
            SetSlot(weaponSlot, itemInformation);
        }
        void DisplayShield(ItemInformation itemInformation)
        {
            SetSlot(shieldSlot, itemInformation);
        }

        void DisplayHelmet(ItemInformation itemInformation)
        {
            SetSlot(helmetSlot, itemInformation);
        }

        void DisplayChest(ItemInformation itemInformation)
        {
            SetSlot(chestSlot, itemInformation);
        }

        void DisplayLegs(ItemInformation itemInformation)
        {
            SetSlot(legsSlot, itemInformation);
        }

        void DisplayBoots(ItemInformation itemInformation)
        {
            SetSlot(bootsSlot, itemInformation);
        }

        private void SetSlot(GameObject slot, ItemInformation itemInformation)
        {
            var image = slot.transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            var icon = Resources.Load<Sprite>(itemInformation.iconPath);
            image.sprite = icon;
            slot.GetComponent<EquipmentItemSelection>().itemId = Guid.Parse(itemInformation.itemId);
        }
    }
}
