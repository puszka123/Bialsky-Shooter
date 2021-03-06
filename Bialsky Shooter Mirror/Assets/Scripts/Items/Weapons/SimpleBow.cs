using BialskyShooter.StatsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "Simple Bow", menuName = "ScriptableObjects/Items/Simple Bow")]
    public class SimpleBow : Item, IWeapon
    {
        public float GetDamage()
        {
            return ItemStatsBook.GetStat(StatType.Damage).value;
        }

        public float GetCooldown()
        {
            return ItemStatsBook.GetStat(StatType.Cooldown).value;
        }

        public ItemSlotType GetItemSlotType()
        {
            return ItemSlotType.Weapon;
        }
    }
}
