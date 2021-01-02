using BialskyShooter.ClassSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(CreatureStats))]
    public class SkillsDisplay : MonoBehaviour
    {
        [SerializeField] bool skillsBook = default;
        [SerializeField] RectTransform slotsPanel = null;
        [SerializeField] RectTransform mainPanel = null;
        [SerializeField] GameObject slotImagePrefab = null;
        [SerializeField] Canvas canvas;
        [SerializeField] int rowsCount = 10;
        [SerializeField] int columnsCount = 10;
        [SerializeField] SkillsProgression skillsProgression = default;      
        CreatureStats creatureStats;
        List<Skill> availableSkills;
        bool panelInitialized;
        List<GameObject> skillsSlots; 

        public IEnumerable<GameObject> SkillsSlots { get { return skillsSlots; } }

        private void Start()
        {
            if(!skillsBook) InitSkillsPanel();
        }

        private void Update()
        {
            if (!skillsBook) return;

            if (skillsProgression != null && availableSkills == null)
            {
                GetAvailableSkills();
            }
            if(!panelInitialized && availableSkills != null) InitSkillsPanel();

        }

        private void GetAvailableSkills()
        {
            if (skillsProgression != null)
            {
                GetLocalCreatureStats();
                if (creatureStats == null) return;
                availableSkills = new List<Skill>(skillsProgression
                    .GetAvailableSkills(creatureStats.ClassType, creatureStats.Level));
            }
        }

        private void GetLocalCreatureStats()
        {
            foreach (var creatureStats in FindObjectsOfType<CreatureStats>())
            {
                if (creatureStats.hasAuthority)
                {
                    this.creatureStats = creatureStats;
                    break;
                }
            }
        }

        void InitSkillsPanel()
        {
            skillsSlots = new List<GameObject>();
            var slotRect = slotImagePrefab.GetComponent<RectTransform>();
            ComputePanelSize(slotRect);
            PlaceSlots(slotRect);
            panelInitialized = true;
        }

        private void PlaceSlots(RectTransform slotRect)
        {
            int index = 0;
            for (int row = 1; row <= rowsCount; row++)
            {
                for (int column = 1; column <= columnsCount; column++)
                {
                    var slotInstance = Instantiate(slotImagePrefab, slotsPanel);
                    SetBookSkillSlot(slotInstance, index);
                    skillsSlots.Add(slotInstance);
                    ++index;
                }
            }
        }

        private void SetBookSkillSlot(GameObject slotInstance, int index)
        {
            if (!skillsBook || availableSkills == null || availableSkills.Count <= index) return;
            slotInstance.GetComponent<BookSkillSlot>().SetSkill(availableSkills[index]);
        }

        private void ComputePanelSize(RectTransform slotRect)
        {
            float marginX = Mathf.Abs(slotsPanel.sizeDelta.x);
            float marginY = Mathf.Abs(slotsPanel.sizeDelta.y);
            float w = (slotRect.rect.width + Mathf.Abs(slotRect.anchoredPosition.x)) * columnsCount + marginX;
            float h = (slotRect.rect.height + Mathf.Abs(slotRect.anchoredPosition.y)) * rowsCount + marginY;
            mainPanel.sizeDelta = new Vector2(w, h);
            mainPanel.anchoredPosition = Vector2.zero;
        }
    }
}
