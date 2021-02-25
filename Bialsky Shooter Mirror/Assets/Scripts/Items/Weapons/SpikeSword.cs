using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "Spike Sword", menuName = "ScriptableObjects/Items/Spike Sword")]
    public class SpikeSword : Item, IWeapon
    {
        public float GetDamage()
        {
            return ItemStatsBook.GetStat(ItemStatType.Damage).value;
        }

        public ItemSlotType GetItemSlotType()
        {
            return ItemSlotType.Weapon;
        }
    }
}
