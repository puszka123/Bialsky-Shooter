using BialskyShooter.SkillSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace BialskyShooter.Control
{
    [RequireComponent(typeof(SkillUser))]
    public class PlayerSkillController : NetworkBehaviour
    {
        [Inject] SkillUser skillUser = null;

        #region Client

        public override void OnStartClient()
        {
            if (!hasAuthority) return;
            Controls controls = new Controls();
            controls.Player.UseSkill.performed += ClientOnUseSkillPerformed;
            controls.Enable();
        }

        private void ClientOnUseSkillPerformed(InputAction.CallbackContext ctx)
        {
            if (!hasAuthority) return;
            var ctrl = ctx.control;
            var bindingName = !string.IsNullOrEmpty(ctrl.shortDisplayName) ? ctrl.shortDisplayName : ctrl.displayName;
            skillUser.CmdUseSkill(bindingName);
        }

        #endregion
    }
}
