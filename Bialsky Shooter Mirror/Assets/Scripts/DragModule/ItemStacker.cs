using BialskyShooter.ItemSystem.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.DragModule
{
    public class ItemStacker : MonoBehaviour
    {
        public static ItemStacker Instance { get; private set; }
        [SerializeField] GameObject stackModalPrefab = null;
        GameObject stackModalInstance = null;

        private void Awake()
        {
            Instance = this;
        }

        public bool TryStackItems(IItemSlot source, IItemSlot destination)
        {
            if (!ValidateItemSlots(source, destination)) return false;
            DisplayStackModal(source, destination);
            return true;
        }

        private bool ValidateItemSlots(IItemSlot source, IItemSlot destination)
        {
            return source.GetItemInformation() != null
                            && source.GetItemInformation().stackable
                            && destination.GetItemInformation() != null
                            && destination.GetItemInformation().stackable
                            && source.GetItemInformation().itemName == destination.GetItemInformation().itemName;
        }

        void StackItems(IItemSlot source, IItemSlot destination, int count)
        {
            destination.Stack(source.GetItemId(), count);
        }

        void DisplayStackModal(IItemSlot source, IItemSlot destination)
        {
            stackModalInstance = Instantiate(stackModalPrefab);
            stackModalInstance.GetComponent<StackModal>().Init(source, destination);
            stackModalInstance.GetComponent<StackModal>().OnStackFinished += StackItems;
        }
    }
}
