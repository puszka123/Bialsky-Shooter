using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.SkillSystem
{
    public class BookSkillSlot : MonoBehaviour
    {
        [SerializeField] Skill skill;
        [SerializeField] Image skillImage;

        public Skill Skill { get { return skill; } }

        private void Start()
        {
            skillImage.sprite = skill.Icon;
            skillImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
