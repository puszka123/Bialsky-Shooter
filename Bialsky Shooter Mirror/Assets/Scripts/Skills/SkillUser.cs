using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using Mirror;
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
            skillsBook.BindSkill("1", skillsBook.GetSkill(0));
        }

        #region

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

        public Weapon GetWeapon()
        {
            return equipment.Weapon;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        #endregion
    }
}
