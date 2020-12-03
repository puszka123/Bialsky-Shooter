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

        private void Start()
        {
            skillImage.sprite = skill.Icon;
            skillImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
