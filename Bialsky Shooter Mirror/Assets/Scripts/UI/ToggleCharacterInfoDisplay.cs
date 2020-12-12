using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.UI
{
    public class ToggleCharacterInfoDisplay : MonoBehaviour
    {
        public static event Action clientOnCharacterInfoDisplayed;

        [SerializeField] GameObject canvasGO = default;

        void Toggle()
        {
            canvasGO.SetActive(!canvasGO.activeSelf);
            if(canvasGO.activeSelf) clientOnCharacterInfoDisplayed?.Invoke();
        }

        void Update()
        {
            if (Keyboard.current.leftCtrlKey.isPressed
                && Keyboard.current.kKey.wasPressedThisFrame)
            {
                Toggle();
            }
        }
    }
}
