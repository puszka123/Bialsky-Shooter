using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.SkillSystem
{
    public class SkillSlot : MonoBehaviour
    {
        public static event Action<string, Guid> clientOnSkillInjected;
        [SerializeField] TMP_Text bindingText = default;
        [SerializeField] Image skillImage = default;
        Guid skillId;

        public string Binding { get { return bindingText.text; } }
        public Guid SkillId { get { return skillId; } }

        private void Start()
        {
            Draggable.ClientOnEndDrag += OnEndDrag;
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

        private void OnDestroy()
        {
            Draggable.ClientOnEndDrag -= OnEndDrag;
        }

        public void InjectSkill(Guid id, Sprite icon)
        {
            skillId = id;
            skillImage.sprite = icon;
            skillImage.color = new Color(1f, 1f, 1f, 1f);
            clientOnSkillInjected?.Invoke(Binding, id);
        }

        void OnEndDrag(Draggable draggable)
        {
            var bookSkill = draggable.GetComponent<BookSkillSlot>();
            if (bookSkill == null) return;
            if (RectTransformUtility.RectangleContainsScreenPoint(
                    GetComponent<RectTransform>(),
                    Mouse.current.position.ReadValue()))
            {
                InjectSkill(bookSkill.Skill.Id, bookSkill.Skill.Icon);
            }
        }
    }
}
