using BialskyShooter.BarriersModule;
using BialskyShooter.BuffsModule;
using BialskyShooter.EnhancementsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    public abstract class Skill : ScriptableObject
    {
        [SerializeField] protected Guid id = default;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string uniqueName = default;
        [SerializeField] protected LayerMask layerMask = new LayerMask();
        [SerializeField] protected float cooldownBase;
        [SerializeField] protected List<Buff> buffs;
        [SerializeField] protected List<AttackEnhancement> attackEnhancements;
        [SerializeField] protected List<BarriersModule.Barrier> barriers;
        [SerializeField] protected List<Skill> blockedSkills;

        public Guid Id { get { return id; } }
        public string UniqueName { get { return uniqueName; } }

        public Sprite Icon { get { return icon; } }

        private void OnEnable()
        {
            id = Guid.NewGuid();
        }

        public abstract void Use(ISkillUser skillUser);

        public virtual float GetCooldown()
        {
            return cooldownBase;
        }
    }
}
