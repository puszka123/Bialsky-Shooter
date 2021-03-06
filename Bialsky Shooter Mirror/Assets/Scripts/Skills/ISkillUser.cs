using BialskyShooter.BuffsModule;
using BialskyShooter.Combat;
using BialskyShooter.ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BialskyShooter.MovementModule;
using BialskyShooter.EnhancementsModule;

namespace BialskyShooter.SkillSystem
{
    public interface ISkillUser
    {
        void UseWeapon(bool attack = true);
        Transform GetTransform();
        void ReceiveBuff(Buff buff);
        void ReceiveAttackEnhancement(AttackEnhancement attackEnhancement);
        void ReceiveBarrier(BarriersModule.Barrier barrier);
        Movement GetMovement();
        void ExecuteCoroutine(IEnumerator method);
        void ResetWeapon();
    }
}
