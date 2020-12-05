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
        [SerializeField] SkillsProgression skillsProgression = default;
        List<Skill> availableSkills;
        Dictionary<Guid, Skill> availableSkillsDict;
        Dictionary<string, Skill> skillBindings;
        Dictionary<Guid, bool> skillsAvailability;
        CreatureStats creatureStats;

        #region Server

        public override void OnStartServer()
        {
            skillBindings = new Dictionary<string, Skill>();
            skillsAvailability = new Dictionary<Guid, bool>();
            availableSkillsDict = new Dictionary<Guid, Skill>();
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
            return availableSkillsDict[skillId];
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
            if (!skillsAvailability.ContainsKey(skill.Id))
            {
                skillsAvailability[skill.Id] = true;
            }
        }

        [Server]
        public void BindSkill(string keyBinding, Guid skillId)
        {
            skillBindings[keyBinding] = GetSkill(skillId);
            if (!skillsAvailability.ContainsKey(skillId))
            {
                skillsAvailability[skillId] = true;
            }
        }

        [Server]
        public bool IsSkillAvailable(Guid skillId)
        {
            return skillsAvailability[skillId];
        }

        [Server]
        public void SetSkillAvailability(Guid skillId, bool availability)
        {
            skillsAvailability[skillId] = availability;
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
            availableSkillsDict = new Dictionary<Guid, Skill>();
            foreach (var skill in availableSkills)
            {
                availableSkillsDict[skill.Id] = skill;
            }
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
