using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.UI
{
    public class ToggleInventoryDisplay : MonoBehaviour
    {
        public static event Action<bool> clientOnInventoryToggled;

        [SerializeField] GameObject canvasGO = default;

        void Toggle()
        {
            canvasGO.SetActive(!canvasGO.activeSelf);
            clientOnInventoryToggled?.Invoke(canvasGO.activeSelf);
        }

        void Update()
        {
            if (Keyboard.current.leftCtrlKey.isPressed
                && Keyboard.current.iKey.wasPressedThisFrame)
            {
                Toggle();
            }
        }
    }
}
