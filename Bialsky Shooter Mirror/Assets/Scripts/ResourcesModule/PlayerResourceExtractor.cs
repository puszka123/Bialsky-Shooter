using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class PlayerResourceExtractor : NetworkBehaviour
    {
        [SerializeField] float capacity;
        [SerializeField] LayerMask layerMask = new LayerMask();

        #region server

        [Command]
        public void CmdExtract(float amount, NetworkIdentity resourceSource)
        {
            resourceSource.GetComponent<ResourceSource>().Extract(amount, connectionToClient.identity);
        }

        #endregion

        #region client

        [ClientCallback]
        private void Awake()
        {
            Controls controls = new Controls();
            controls.Player.ResourceExtractor.performed += ExtractionPerformed;
            controls.Enable();
        }


        [Client]
        private void ExtractionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!hasAuthority) return;
            var selectedGameObject = GetSelectedGameObject();
            if (selectedGameObject == null) return;
            if (!selectedGameObject.TryGetComponent(out ResourceSource resourceSource)) return;
            CmdExtract(capacity, resourceSource.GetComponent<NetworkIdentity>());
        }

        [Client]
        private GameObject GetSelectedGameObject()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return null;
            return hit.transform.gameObject;
        }

        #endregion
    }
}