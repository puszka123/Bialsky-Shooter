using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem.UI
{
    public static class ItemStacker
    {
        public static bool TryStackItems(IItemSlot source, IItemSlot destination)
        {
            if (!ValidateItemSlots(source, destination)) return false;
            StackItems(source, destination);
            return true;
        }

        private static bool ValidateItemSlots(IItemSlot source, IItemSlot destination)
        {
            return source.GetItemInformation() != null
                            && source.GetItemInformation().stackable
                            && destination.GetItemInformation() != null
                            && destination.GetItemInformation().stackable
                            && source.GetItemInformation().itemName == destination.GetItemInformation().itemName;
        }

        static void StackItems(IItemSlot source, IItemSlot destination)
        {
            destination.Stack(source.GetItemId(), source.GetItemInformation().count);
        }
    }
}
