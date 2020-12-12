using BialskyShooter.ClassSystem;
using BialskyShooter.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.CharacterModule
{
    public class EnemyPlateDisplay : CreaturePlateDisplay
    {
        [SerializeField] LayerMask layerMask = new LayerMask();

        private void Start()
        {
            InitInputSystem();
        }

        private void InitInputSystem()
        {
            Controls controls = new Controls();
            controls.Player.EnemyPlate.performed += EnemyPlatePerformed;
            controls.Enable();
        }

        private void EnemyPlatePerformed(InputAction.CallbackContext ctx)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Close();
                return;
            }
            if (!hit.transform.TryGetComponent<Health>(out Health enemyHealth)
                || !hit.transform.TryGetComponent<CreatureStats>(out CreatureStats creatureStats))
            {
                Close();
                return;
            }
            Init(enemyHealth, creatureStats);
        }
    }
}
