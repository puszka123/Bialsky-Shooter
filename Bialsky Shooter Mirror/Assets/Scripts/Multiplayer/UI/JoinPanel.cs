using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BialskyShooter.Multiplayer.UI
{
    public class JoinPanel : MonoBehaviour
    {
        [SerializeField] TMP_InputField ipAddressInput;

        public string IPAddress { get { return ipAddressInput.text; } }
    }
}
