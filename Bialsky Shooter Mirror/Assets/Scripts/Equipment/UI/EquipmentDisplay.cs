using BialskyShooter.ItemSystem;
using BialskyShooter.ItemSystem.UI;
using BialskyShooter.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.EquipmentSystem.UI
{
    public class EquipmentDisplay : MonoBehaviour, IEquipmentSlotsContainer
    {
        [SerializeField] List<EquipmentItemSlot> itemSlots = null;
        Equipment equipment;
        EquipmentController equippingController;
        bool readOnlyMode;

        private void Awake()
        {
            ToggleCharacterInfoDisplay.clientOnCharacterInfoDisplayed += OnLocalCharacterInfoDisplayed;
        }

        private void OnDestroy()
        {
            ToggleCharacterInfoDisplay.clientOnCharacterInfoDisplayed -= OnLocalCharacterInfoDisplayed;
            if (equippingController != null)
            {
                equippingController.clientOnItemEquipped -= DisplayItem;
                equippingController.clientOnItemUnequipped -= StopDisplayItem;
            }
        }

        public void ReadOnly()
        {
            itemSlots.ForEach(slot => slot.ReadOnly());
        }

        public void SetupEquipmentDisplay(Equipment equipment, EquipmentController equippingController)
        {
            if (this.equippingController != null)
            {
                this.equippingController.clientOnItemEquipped -= DisplayItem;
                this.equippingController.clientOnItemUnequipped -= StopDisplayItem;
            }
            this.equipment = equipment;
            this.equippingController = equippingController;
            this.equippingController.clientOnItemEquipped += DisplayItem;
            this.equippingController.clientOnItemUnequipped += StopDisplayItem;
            DisplayEquipment();
        }

        private void OnLocalCharacterInfoDisplayed()
        {
            var player = GetLocalPlayer();
            SetupEquipmentDisplay(player.GetComponent<Equipment>(), player.GetComponent<EquipmentController>());
        }

        private GameObject GetLocalPlayer()
        {
            GameObject localPlayer = null;
            foreach (var player in GameObject.FindGameObjectsWithTag("PlayerCharacter"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    localPlayer = player;
                    break;
                }
            }
            return localPlayer;
        }

        void DisplayEquipment()
        {
            if (equipment == null || equipment.ItemInformations == null) return;
            foreach (var item in equipment.ItemInformations)
            {
                DisplayItem(item);
            }
        }

        void DisplayItem(ItemInformation itemInformation)
        {
            var foundSlot = itemSlots.FirstOrDefault(slot =>
            slot.ItemSlotType == itemInformation.slotType
            && slot.GetItemId() != Guid.Parse(itemInformation.itemId));
            if (foundSlot != null) SetSlot(foundSlot.gameObject, itemInformation);
        }

        private void StopDisplayItem(ItemInformation itemInformation)
        {
            var found = itemSlots.FirstOrDefault(e => e.itemId == Guid.Parse(itemInformation.itemId));
            if (found != null) UnsetSlot(found);
        }

        private void SetSlot(GameObject slot, ItemInformation itemInformation)
        {
            var icon = Resources.Load<Sprite>(itemInformation.iconPath);
            slot.GetComponent<EquipmentItemSlot>().InjectItem(Guid.Parse(itemInformation.itemId), icon);
            SetItemInformationToggle(slot, new ItemDisplay(itemInformation));
        }

        private void UnsetSlot(EquipmentItemSlot slot)
        {
            slot.ClearItem();
        }

        private void SetItemInformationToggle(GameObject slot, ItemDisplay displayItem)
        {
            if (slot == null || slot.GetComponent<ItemInformationTooltip>() == null) return;
            slot.GetComponent<ItemInformationTooltip>().SetItemDisplay(displayItem);
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            if (equippingController != null)
            {
                equippingController.ClientEquipItem(itemSlot.GetItemId());
            }
        }
    }
}
