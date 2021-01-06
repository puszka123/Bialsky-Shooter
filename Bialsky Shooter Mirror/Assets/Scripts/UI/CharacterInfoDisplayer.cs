using BialskyShooter.CharacterModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.UI
{
    public class CharacterInfoDisplayer : MonoBehaviour
    {
        [SerializeField] GameObject characterInfoPrefab = null;
        [SerializeField] LayerMask layerMask = new LayerMask();
        GameObject characterInfoInstance;

        void Start()
        {
            InitInputSystem();
        }

        private void InitInputSystem()
        {
            Controls controls = new Controls();
            controls.Player.CharacterInfo.performed += CharacterInfoPerformed;
            controls.Enable();
        }

        private void CharacterInfoPerformed(InputAction.CallbackContext ctx)
        {
            var selectedGameObject = GetSelectedGameObject();
            if (selectedGameObject == null) return;
            characterInfoInstance = Instantiate(characterInfoPrefab);
            characterInfoInstance.GetComponent<DisplayCharacterInfo>().Display(selectedGameObject);
        }

        private GameObject GetSelectedGameObject()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return null;
            return hit.transform.gameObject;
        }
    }
}
