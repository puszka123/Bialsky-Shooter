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

        public Guid Id { get { return id; } }
        public string UniqueName { get { return uniqueName; } }

        public Sprite Icon { get { return icon; } }

        public abstract void Use(ISkillUser skillUser);
    }
}
