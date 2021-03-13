using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public class Weapon : Item, IWeapon
    {

        public Weapon(ItemSettings itemSettings) : base(itemSettings)
        {

        }

        public float GetDamage()
        {
            return ItemSettings.ItemStatsBook.GetStat(StatType.Damage).value;
        }

        public float GetCooldown()
        {
            return ItemSettings.ItemStatsBook.GetStat(StatType.Cooldown).value;
        }

        public ItemSlotType GetItemSlotType()
        {
            return ItemSlotType.Weapon;
        }
    }
}
