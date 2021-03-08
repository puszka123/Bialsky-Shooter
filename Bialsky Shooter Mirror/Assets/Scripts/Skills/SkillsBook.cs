using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using BialskyShooter.ClassSystem;
using Zenject;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(CreatureStats))]
    public class SkillsBook : NetworkBehaviour
    {
        [Inject] CreatureStats creatureStats;

        [SerializeField] SkillsProgression skillsProgression = default;
        List<Skill> availableSkills;
        Dictionary<Guid, Skill> availableSkillsDict;
        Dictionary<string, Skill> skillBindings;
        Dictionary<Guid, int> skillsUses;
        Dictionary<Guid, bool> skillsActivity;
        Dictionary<Guid, bool> skillsCooldowns;
        SyncList<SkillDisplayData> skillDisplayData = new SyncList<SkillDisplayData>();
        public IList<SkillDisplayData> SkillDisplayData { get { return skillDisplayData; } }

        #region Server

        public override void OnStartServer()
        {
            skillBindings = new Dictionary<string, Skill>();
            skillsUses = new Dictionary<Guid, int>();
            availableSkillsDict = new Dictionary<Guid, Skill>();
            skillsActivity = new Dictionary<Guid, bool>();
            skillsCooldowns = new Dictionary<Guid, bool>();
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
            print($"{keyBinding} {skill.UniqueName}");
            skillBindings[keyBinding] = skill;
            if (!skillsUses.ContainsKey(skill.Id))
            {
                skillsUses[skill.Id] = skill.GetUsesCount();
            }
        }

        [Server]
        public void BindSkill(string keyBinding, Guid skillId)
        {
            skillBindings[keyBinding] = GetSkill(skillId);
            if (!skillsUses.ContainsKey(skillId))
            {
                skillsUses[skillId] = skillBindings[keyBinding].GetUsesCount();
            }
        }

        [Server]
        public void CooldownSkill(Guid skillId)
        {
            skillsCooldowns[skillId] = true;
        }

        [Server]
        public bool IsSkillOnCooldown(Guid skillId)
        {
            return skillsCooldowns.ContainsKey(skillId) && skillsCooldowns[skillId];
        }

        [Server]
        public void UnbindSkill(string keyBinding, Guid skillId)
        {
            skillBindings[keyBinding] = null;
        }

        [Server]
        public bool IsSkillAvailable(Guid skillId)
        {
            return skillsUses[skillId] > 0 && IsSkillActive(skillId);
        }

        [Server]
        public void ResetSkill(Skill skill)
        {
            skillsUses[skill.Id] = skill.GetUsesCount();
            skillsCooldowns[skill.Id] = false;
        }

        [Server]
        public void DisableSkill(Guid skillId)
        {
            skillsActivity[skillId] = false;
        }

        [Server]
        public void EnableSkill(Guid skillId)
        {
            skillsActivity[skillId] = true;
        }

        [Server]
        public bool IsSkillActive(Guid skillId)
        {
            return !skillsActivity.ContainsKey(skillId) || skillsActivity[skillId];
        }

        [Server]
        public void IncreaseSkillUses(Guid skillId)
        {
            skillsUses[skillId]++;
        }

        [Server]
        public void DecreaseSkillUses(Guid skillId)
        {
            skillsUses[skillId]--;
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
            skillDisplayData.Clear();
            foreach (var skill in availableSkills)
            {
                skillDisplayData.Add(new SkillDisplayData { id = skill.Id, iconPath = skill.Icon.name });
                availableSkillsDict[skill.Id] = skill;
                if (!skillsUses.ContainsKey(skill.Id))
                {
                    skillsUses[skill.Id] = skill.GetUsesCount();
                }
            }
        }

        [Command]
        void CmdBindSkill(string binding, Guid skillId)
        {
            BindSkill(binding, skillId);
        }

        [Command]
        void CmdUnbindSkill(string binding, Guid skillId)
        {
            UnbindSkill(binding, skillId);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            SkillSlot.clientOnSkillInjected += OnSkillInjected;
            SkillSlot.clientOnSkillRemoved += OnSkillRemoved;
        }

        public override void OnStopClient()
        {
            SkillSlot.clientOnSkillInjected -= OnSkillInjected;
            SkillSlot.clientOnSkillRemoved -= OnSkillRemoved;
        }

        [Client]
        private void OnSkillInjected(string binding, Guid skillId)
        {
            if (!hasAuthority) return;
            CmdBindSkill(binding, skillId);
        }

        [Client]
        private void OnSkillRemoved(string binding, Guid skillId)
        {
            if (!hasAuthority) return;
            CmdUnbindSkill(binding, skillId);
        }

        #endregion
    }
}
