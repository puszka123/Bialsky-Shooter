using BialskyShooter.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    public class CreatureStatsDisplay : MonoBehaviour
    {
        struct StatDisplay
        {
            public TMP_Text label;
            public TMP_Text value;
        };

        [SerializeField] GameObject statsPanel = default;
        List<StatDisplay> statDisplays;
        CreatureStats creatureStats;

        private void Start()
        {
            statDisplays = new List<StatDisplay>();
            foreach (Transform child in statsPanel.transform)
            {
                foreach (Transform stat in child)
                {
                    statDisplays.Add
                    (
                        new StatDisplay
                        {
                            label = stat.GetChild(0).GetComponent<TMP_Text>(),
                            value = stat.GetChild(1).GetComponent<TMP_Text>(),
                        }
                    );
                    statDisplays[statDisplays.Count - 1].label.gameObject.SetActive(false);
                    statDisplays[statDisplays.Count - 1].value.gameObject.SetActive(false);
                }
            }
            ToggleCharacterInfoDisplay.clientOnCharacterInfoDisplayed += OnCharacterInfoDisplayed;
        }

        private void OnDestroy()
        {
            ToggleCharacterInfoDisplay.clientOnCharacterInfoDisplayed -= OnCharacterInfoDisplayed;
        }

        public void SetCreatureStats(CreatureStats stats)
        {
            creatureStats = stats;
        }

        private void OnCharacterInfoDisplayed()
        {
            GetLocalPlayerStats();
        }

        private void GetLocalPlayerStats()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    SetCreatureStats(player.GetComponent<CreatureStats>());
                    break;
                }
            }
        }

        private void Update()
        {
            if (creatureStats == null) return;
            statDisplays[0].label.gameObject.SetActive(true);
            statDisplays[0].value.gameObject.SetActive(true);
            statDisplays[0].label.text = creatureStats.Health?.nameToDisplay;
            statDisplays[0].value.text = creatureStats.Health?.value.ToString();

            statDisplays[1].label.gameObject.SetActive(true);
            statDisplays[1].value.gameObject.SetActive(true);
            statDisplays[1].label.text = creatureStats.Stamina?.nameToDisplay;
            statDisplays[1].value.text = creatureStats.Stamina?.value.ToString();

            statDisplays[2].label.gameObject.SetActive(true);
            statDisplays[2].value.gameObject.SetActive(true);
            statDisplays[2].label.text = creatureStats.Strength?.nameToDisplay;
            statDisplays[2].value.text = creatureStats.Strength?.value.ToString();

            statDisplays[3].label.gameObject.SetActive(true);
            statDisplays[3].value.gameObject.SetActive(true);
            statDisplays[3].label.text = creatureStats.Power?.nameToDisplay;
            statDisplays[3].value.text = creatureStats.Power?.value.ToString();

            statDisplays[4].label.gameObject.SetActive(true);
            statDisplays[4].value.gameObject.SetActive(true);
            statDisplays[4].label.text = creatureStats.Agility?.nameToDisplay;
            statDisplays[4].value.text = creatureStats.Agility?.value.ToString();
        }
    }
}
