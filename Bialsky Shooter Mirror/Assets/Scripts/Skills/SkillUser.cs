using BialskyShooter.BarriersModule;
using BialskyShooter.BuffsModule;
using BialskyShooter.Combat;
using BialskyShooter.EnhancementsModule;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using BialskyShooter.MovementModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(SkillsBook))]
    [RequireComponent(typeof(WeaponUser))]
    [RequireComponent(typeof(EnhancementReceiver))]
    public class SkillUser : NetworkBehaviour, ISkillUser
    {
        [Inject] SkillsBook skillsBook = null;
        [Inject] Equipment equipment = null;
        [Inject] WeaponUser weaponUser = null;
        [Inject] BuffsReceiver buffsReceiver = null;
        [Inject] Movement movement = null;
        [Inject] EnhancementReceiver enhancementReceiver = null;
        [Inject] BarrierReceiver barrierReceiver;

        #region Server

        [Server]
        public void UseRandomSkill()
        {
            if (skillsBook == null) return;
            Skill skill = skillsBook.GetSkill(0);
            if (skill == null) return;
            UseSkill(skill);
        }

        [Command]
        public void CmdUseSkill(string keyBinding)
        {
            UseSkill(keyBinding);
        }

        [Server]
        void UseSkill(string keyBinding)
        {
            Skill skill = skillsBook.GetSkill(keyBinding);
            if (skill == null) return;
            UseSkill(skill);
        }

        private void UseSkill(Skill skill)
        {
            if (skillsBook.IsSkillAvailable(skill.Id))
            {
                skillsBook.UpdateSkillAvailability(skill.Id, -1);
                skillsBook.DisableSkill(skill.Id);
                StartCoroutine(CooldownSkill(skill));
                StartCoroutine(IntervalBetweenUses(skill));
                skill.Use(this);
            }
        }

        [Server]
        IEnumerator IntervalBetweenUses(Skill skill)
        {
            yield return new WaitForSeconds(skill.UseInterval());
            skillsBook.EnableSkill(skill.Id);
        }

        [Server]
        IEnumerator CooldownSkill(Skill skill)
        {
            if (skillsBook.IsSkillOnCooldown(skill.Id)) yield return null;
            else
            {
                skillsBook.CooldownSkill(skill.Id);
                yield return new WaitForSeconds(skill.GetCooldown());
                skillsBook.ResetSkill(skill);
            }
        }

        [Server]
        public IWeaponController GetWeaponController()
        {
            return null;
        }

        [Server]
        public Transform GetTransform()
        {
            return transform;
        }

        [Server]
        public void UseWeapon(bool attack = true)
        {
            if (UsingWeaponAsAttack(attack)) return;
            if (UsingWeaponAsDefence())
            {
                weaponUser.ResetDefence();
                return;
            }
            weaponUser.Terminate();
            weaponUser.UseWeapon(equipment.GetItem<IWeapon>(ItemSlotType.Weapon), attack);
        }

        private bool UsingWeaponAsDefence()
        {
            return weaponUser.WeaponInUse && weaponUser.WeaponAsDefence;
        }

        private bool UsingWeaponAsAttack(bool attack)
        {
            return weaponUser.WeaponInUse && attack;
        }

        [Server]
        public void ReceiveBuff(Buff buff)
        {
            buffsReceiver.AddBuff(buff);
        }

        [Server]
        public Movement GetMovement()
        {
            return movement;
        }

        [Server]
        public void ReceiveAttackEnhancement(AttackEnhancement attackEnhancement)
        {
            enhancementReceiver.Receive(attackEnhancement);
        }

        [Server]
        public void ReceiveBarrier(BarriersModule.Barrier barrier)
        {
            barrierReceiver.Receive(barrier);
        }

        [Server]
        public void ExecuteCoroutine(IEnumerator method)
        {
            StartCoroutine(method);
        }

        [Server]
        public void ResetWeapon()
        {
            weaponUser.Terminate();
        }

        #endregion
    }
}
