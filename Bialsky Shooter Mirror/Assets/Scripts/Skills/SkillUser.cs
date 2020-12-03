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
    public class SkillUser : NetworkBehaviour, ISkillUser
    {
        [SerializeField] SkillsBook skillsBook = null;
        Equipment equipment;

        private void Start()
        {
            equipment = GetComponent<Equipment>();
            //debug only
            //skillsBook.BindSkill("1", skillsBook.GetSkill(0));
        }

        #region

        [Command]
        public void CmdUseSkill(string keyBinding)
        {
            UseSkill(keyBinding);
        }

        [Command]
        void CmdBindSkill(string binding, Guid skillId)
        {
            skillsBook.BindSkill(binding, skillId);
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

        #region Client

        public override void OnStartClient()
        {
            SkillSlot.clientOnSkillInjected += OnSkillInjected;
        }

        [Client]
        private void OnSkillInjected(string binding, Guid skillId)
        {
            if (!hasAuthority) return;
            CmdBindSkill(binding, skillId);
        }

        #endregion
    }
}
