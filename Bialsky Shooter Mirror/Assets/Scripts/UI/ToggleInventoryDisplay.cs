using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.UI
{
    public class ToggleInventoryDisplay : MonoBehaviour
    {
        [SerializeField] GameObject canvasGO = default;

        void Toggle()
        {
            canvasGO.SetActive(!canvasGO.activeSelf);
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
