using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Core
{
    public class RendererProxy : NetworkBehaviour
    {
        [SerializeField] Material opaque = null;
        [SerializeField] Material transparent = null;

        #region server

        [Server]
        public void ServerBecomeInvisibleForOthers(float time)
        {
            RpcBecomeInvisibleForOthers(time);
        }

        [Server]
        public void ServerBecomeTransparentForYourself(float time)
        {
            TargetBecomeTransparent(time);
        }

        #endregion

        #region client

        [ClientRpc]
        public void RpcBecomeInvisibleForOthers(float time)
        {
            if (!hasAuthority) StartCoroutine(BecomeInvisible(time));
        }

        [TargetRpc]
        public void TargetBecomeTransparent(float time)
        {
            StartCoroutine(BecomeTransparent(time));
        }

        #endregion

        IEnumerator BecomeInvisible(float time)
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
            yield return new WaitForSeconds(time);
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
        }

        IEnumerator BecomeTransparent(float time)
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material = transparent;
            }
            yield return new WaitForSeconds(time);
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material = opaque;
            }
        }
    }
}
