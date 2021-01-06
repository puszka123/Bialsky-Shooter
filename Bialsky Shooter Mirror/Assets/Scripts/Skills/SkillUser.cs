using BialskyShooter.Combat;
using BialskyShooter.EquipmentSystem;
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
    public class SkillUser : NetworkBehaviour, ISkillUser
    {
        [Inject] SkillsBook skillsBook = null;
        [Inject] Equipment equipment = null;
        [Inject] WeaponUser weaponUser = null;

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
                StartCoroutine(CooldownSkill(skill));
                skill.Use(this);
            }
        }

        [Server]
        IEnumerator CooldownSkill(Skill skill)
        {
            skillsBook.SetSkillAvailability(skill.Id, false);
            yield return new WaitForSeconds(skill.GetCooldown());
            skillsBook.SetSkillAvailability(skill.Id, true);
        }

        [Server]
        public WeaponController GetWeaponController()
        {
            return null;
        }

        [Server]
        public Transform GetTransform()
        {
            return transform;
        }

        public void UseWeapon()
        {
            weaponUser.UseWeapon(equipment.Weapon);
        }

        #endregion
    }
}
