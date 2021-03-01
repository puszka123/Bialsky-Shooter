using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.EquipmentSystem
{
    [RequireComponent(typeof(Equipment))]
    public class Equipmentnstantiator : NetworkBehaviour
    {
        [Inject] Equipment equipment = null;
        [SerializeField] GameObject weaponHandler = null;

        #region server
        public event Action<GameObject, ItemSlotType> serverOnItemInstantiated;

        List<ItemInformation> serverInstantiatedItems = new List<ItemInformation>();

        Dictionary<ItemSlotType, GameObject> serverInstantiatedItemsDict = new Dictionary<ItemSlotType, GameObject>();

        public override void OnStartServer()
        {
            equipment.serverOnEquipmentChanged += ServerEquipmentChanged;
        }

        public override void OnStopServer()
        {
            equipment.serverOnEquipmentChanged -= ServerEquipmentChanged;
        }

        [Server]
        private void ServerEquipmentChanged()
        {
            foreach (var itemToInstantiate in GetItemsToInstantiate(serverInstantiatedItems))
            {
                serverInstantiatedItems.Add(itemToInstantiate);
                InstantiateItem(itemToInstantiate);
            }
            foreach (var itemToDestroy in GetItemsToDestroy(serverInstantiatedItems))
            {
                serverInstantiatedItems.Remove(itemToDestroy);
                DestroyItem(itemToDestroy);
            }
        }

        [Server]
        public GameObject GetInstantiatedItem(ItemSlotType itemSlotType)
        {
            if (!serverInstantiatedItemsDict.ContainsKey(itemSlotType)) return null;
            return serverInstantiatedItemsDict[itemSlotType];
        }

        #endregion


        #region client

        List<ItemInformation> clientInstantiatedItems = new List<ItemInformation>();

        [Client]
        public override void OnStartClient()
        {
            if (NetworkServer.active) return;
            equipment.clientOnEquipmentChanged += ClientEquipmentChanged;
            Invoke(nameof(ClientEquipmentChanged), 1f);
        }

        [Client]
        public override void OnStopClient()
        {
            if (NetworkServer.active) return;
            equipment.clientOnEquipmentChanged -= ClientEquipmentChanged;
        }

        [Client]
        private void ClientEquipmentChanged()
        {
            foreach (var itemToInstantiate in GetItemsToInstantiate(clientInstantiatedItems))
            {
                clientInstantiatedItems.Add(itemToInstantiate);
                InstantiateItem(itemToInstantiate);
            }
            foreach (var itemToDestroy in GetItemsToDestroy(clientInstantiatedItems))
            {
                clientInstantiatedItems.Remove(itemToDestroy);
                DestroyItem(itemToDestroy);
            }
        }

        #endregion

        private IList<ItemInformation> GetItemsToDestroy(List<ItemInformation> instantiatedItems)
        {
            var itemsToDestroy = new List<ItemInformation>();
            foreach (var itemInformation in instantiatedItems)
            {
                if (!equipment.ItemInformations.Contains(itemInformation)) itemsToDestroy.Add(itemInformation);
            }
            return itemsToDestroy;
        }

        private IList<ItemInformation> GetItemsToInstantiate(List<ItemInformation> instantiatedItems)
        {
            var itemsToInstantiate = new List<ItemInformation>();
            foreach (var itemInformation in equipment.ItemInformations)
            {
                if (!instantiatedItems.Contains(itemInformation)) itemsToInstantiate.Add(itemInformation);
            }
            return itemsToInstantiate;
        }

        private void InstantiateItem(ItemInformation itemInformation)
        {
            var item = Resources.Load<GameObject>(itemInformation.itemName);
            var parentTransform = GetItemParentTransform(itemInformation.slotType);
            var itemInstance = Instantiate
                (item,
                parentTransform.position,
                parentTransform.rotation,
                parentTransform);
            itemInstance.transform.localScale *= 10f;
            if (isServer)
            {
                serverInstantiatedItemsDict[itemInformation.slotType] = itemInstance;
                serverOnItemInstantiated?.Invoke(itemInstance, itemInformation.slotType);
            }
        }

        private void DestroyItem(ItemInformation itemInformation)
        {

        }

        private Transform GetItemParentTransform(ItemSlotType itemSlotType)
        {
            switch (itemSlotType)
            {
                case ItemSlotType.Weapon:
                    return weaponHandler.transform;
                case ItemSlotType.Shield:
                    break;
                case ItemSlotType.Helmet:
                    break;
                case ItemSlotType.Chest:
                    break;
                case ItemSlotType.Legs:
                    break;
                case ItemSlotType.Boots:
                    break;
                default:
                    return null;
            }
            return null;
        }
    }
}
