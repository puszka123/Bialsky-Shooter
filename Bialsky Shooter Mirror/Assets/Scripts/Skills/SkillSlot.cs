using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.SkillSystem
{
    public class SkillSlot : MonoBehaviour, ISkillSlot
    {
        public static event Action<string, Guid> clientOnSkillInjected;
        public static event Action<string, Guid> clientOnSkillRemoved;
        [SerializeField] TMP_Text bindingText = default;
        [SerializeField] Image skillImage = default;
        Guid skillId;
        Sprite icon;

        public string Binding { get { return bindingText.text; } }
        public Guid SkillId { get { return skillId; } }
        public Sprite Icon { get { return icon; } }

        private void Start()
        {
            var binding = SkillBindingManager.PopAvailableBinding();
            if (binding != null)
            {
                bindingText.text = binding;
            }
            else
            {
                bindingText.text = "Missing";
            }
        }

        public void InjectSkill(Guid id, Sprite icon)
        {
            skillId = id;
            this.icon = icon;
            skillImage.sprite = icon;
            skillImage.color = new Color(1f, 1f, 1f, 1f);
            clientOnSkillInjected?.Invoke(Binding, id);
        }

        public void RemoveSkill()
        {
            Guid removedSkillId = skillId;
            skillId = Guid.Empty;
            icon = null;
            skillImage.sprite = null;
            skillImage.color = new Color(1f, 1f, 1f, 0f);
            clientOnSkillRemoved?.Invoke(Binding, removedSkillId);
        }

        public Guid GetSkillId()
        {
            return skillId;
        }

        public Sprite GetSkillIcon()
        {
            return icon;
        }

        public void InjectSkill(ISkillSlot skillSlot)
        {
            InjectSkill(skillSlot.GetSkillId(), skillSlot.GetSkillIcon());
        }
    }
}
