using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(SkillsBook))]
    public class SkillUser : NetworkBehaviour, ISkillUser
    {
        SkillsBook skillsBook;
        Equipment equipment;

        #region Server

        public override void OnStartServer()
        {
            equipment = GetComponent<Equipment>();
            skillsBook = GetComponent<SkillsBook>();
        }

        [Command]
        public void CmdUseSkill(string keyBinding)
        {
            UseSkill(keyBinding);
        }

        [Server]
        void UseSkill(string keyBinding)
        {
            skillsBook.GetSkill(keyBinding).Use(this);
        }

        [Server]
        public Weapon GetWeapon()
        {
            return equipment.Weapon;
        }

        [Server]
        public Transform GetTransform()
        {
            return transform;
        }

        #endregion
    }
}
