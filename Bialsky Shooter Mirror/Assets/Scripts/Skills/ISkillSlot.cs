using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    public interface ISkillSlot
    {
        Guid GetSkillId();
        Sprite GetSkillIcon();
        void RemoveSkill();
        void InjectSkill(ISkillSlot skillSlot);
    }
}
