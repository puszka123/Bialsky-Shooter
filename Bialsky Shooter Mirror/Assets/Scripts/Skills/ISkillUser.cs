using BialskyShooter.Combat;
using BialskyShooter.ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    public interface ISkillUser
    {
        void UseWeapon();
        Transform GetTransform();
    }
}
