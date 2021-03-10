using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public interface IWeapon : IItem, IEquipmentItem
    {
        float GetDamage();
        float GetCooldown();
    }
}
