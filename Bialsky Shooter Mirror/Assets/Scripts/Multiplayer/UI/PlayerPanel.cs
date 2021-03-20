using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BialskyShooter.Multiplayer.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text playerNameText;

        public void SetName(string name)
        {
            playerNameText.SetText(name);
        }
    }
}
