using BialskyShooter.Combat;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BialskyShooter.ClassSystem.UI
{
    public class CreaturePlateDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text levelText = default;
        [SerializeField] TMP_Text healthText = default;
        [SerializeField] TMP_Text secondaryText = default;
        [SerializeField] GameObject canvasGO = default;
        Health health;
        CreatureStats stats;
        protected bool initialized;

        

        public void Init(Health health, CreatureStats stats)
        {
            secondaryText.text = string.Empty;
            this.health = health;
            this.stats = stats;
            canvasGO.SetActive(true);
            initialized = true;
        }

        protected virtual void Update()
        {
            if (!initialized) return;
            var currentHealth = health.CurrentHealth;
            var maxHealth = health.MaxHealth;
            var level = stats.Level;
            levelText.text = level.ToString();
            healthText.text = $"{currentHealth}/{maxHealth}";
        }

        public void Close()
        {
            if (canvasGO != null)
            {
                canvasGO.SetActive(false);
            }
            initialized = false;
            health = null;
            stats = null;
        }

    }
}
