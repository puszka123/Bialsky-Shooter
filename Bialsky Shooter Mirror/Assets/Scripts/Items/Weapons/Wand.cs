using BialskyShooter.StatsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "Wand", menuName = "ScriptableObjects/Items/Wand")]
    public class Wand : Item, IWeapon
    {
        public float GetCooldown()
        {
            return ItemStatsBook.GetStat(StatType.Cooldown).value;
        }

        public float GetDamage()
        {
            return ItemStatsBook.GetStat(StatType.Damage).value;
        }

        public ItemSlotType GetItemSlotType()
        {
            return ItemSlotType.Weapon;
        }
    }
}
