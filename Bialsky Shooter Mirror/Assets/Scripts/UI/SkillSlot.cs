using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BialskyShooter.UI
{
    public class SkillSlot : MonoBehaviour
    {
        public static event Action<string, Guid> clientOnSkillInjected;
        [SerializeField] TMP_Text bindingText;
        Guid skillId;

        public string Binding { get { return bindingText.text; } }
        public Guid SkillId { get { return skillId; } }

        public void InjectSkill(Guid id)
        {
            skillId = id;
            clientOnSkillInjected?.Invoke(Binding, id);
        }
    }
}
