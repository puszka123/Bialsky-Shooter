using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using BialskyShooter.ClassSystem;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(CreatureStats))]
    [RequireComponent(typeof(Experience))]
    public class SkillsBook : NetworkBehaviour
    {
        [SerializeField] SkillsProgression skillsProgression;
        List<Skill> availableSkills;
        Dictionary<string, Skill> skillBindings;
        CreatureStats creatureStats;

        #region Server

        public override void OnStartServer()
        {
            skillBindings = new Dictionary<string, Skill>();
            creatureStats = GetComponent<CreatureStats>();
            creatureStats.serverOnLevelUp += OnLevelUp;
            UpdateAvailableSkills();
        }

        public override void OnStopServer()
        {
            creatureStats.serverOnLevelUp -= OnLevelUp;
        }

        [Server]
        public Skill GetSkill(Guid skillId)
        {
            return availableSkills.First(s => s.Id == skillId);
        }

        [Server]
        public Skill GetSkill(string keyBinding)
        {
            return skillBindings.ContainsKey(keyBinding) ? skillBindings[keyBinding] : null;
        }

        [Server]
        public Skill GetSkill(int index)
        {
            return availableSkills[index];
        }

        [Server]
        public void BindSkill(string keyBinding, Skill skill)
        {
            skillBindings[keyBinding] = skill;
        }

        [Server]
        public void BindSkill(string keyBinding, Guid skillId)
        {
            skillBindings[keyBinding] = GetSkill(skillId);
        }

        [Server]
        void OnLevelUp(int level)
        {
            UpdateAvailableSkills();
        }

        [Server]
        void UpdateAvailableSkills()
        {
            availableSkills = skillsProgression
                .GetAvailableSkills(creatureStats.ClassType, 
                creatureStats.Level)
                .ToList();
        }

        [Command]
        void CmdBindSkill(string binding, Guid skillId)
        {
            BindSkill(binding, skillId);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            SkillSlot.clientOnSkillInjected += OnSkillInjected;
        }

        public override void OnStopClient()
        {
            SkillSlot.clientOnSkillInjected -= OnSkillInjected;
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
