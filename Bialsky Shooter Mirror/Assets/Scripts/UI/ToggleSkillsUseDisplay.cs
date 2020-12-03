using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class ToggleSkillsUseDisplay : MonoBehaviour
{
    [SerializeField] GameObject canvasGO;


    void Toggle()
    {
        canvasGO.SetActive(!canvasGO.activeSelf);
    }

    void Update()
    {
        if(Keyboard.current.leftCtrlKey.isPressed
            && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Toggle();
        }
    }
}
