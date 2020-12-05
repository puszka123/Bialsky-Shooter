using BialskyShooter.Combat;
using BialskyShooter.EquipmentSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(SkillsBook))]
    [RequireComponent(typeof(WeaponUser))]
    public class SkillUser : NetworkBehaviour, ISkillUser
    {
        SkillsBook skillsBook;
        Equipment equipment;
        WeaponUser weaponUser;
        #region Server

        public override void OnStartServer()
        {
            equipment = GetComponent<Equipment>();
            skillsBook = GetComponent<SkillsBook>();
            weaponUser = GetComponent<WeaponUser>();
        }

        [Command]
        public void CmdUseSkill(string keyBinding)
        {
            UseSkill(keyBinding);
        }

        [Server]
        void UseSkill(string keyBinding)
        {
            skillsBook.GetSkill(keyBinding)?.Use(this);
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
