using BialskyShooter.SkillSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.Control
{
    [RequireComponent(typeof(SkillUser))]
    public class PlayerSkillController : NetworkBehaviour
    {
        SkillUser skillUser;

        #region Client

        public override void OnStartClient()
        {
            skillUser = GetComponent<SkillUser>();
            if (!hasAuthority) return;
            Controls controls = new Controls();
            controls.Player.UseSkill.performed += ClientOnUseSkillPerformed;
            controls.Enable();
        }

        private void ClientOnUseSkillPerformed(InputAction.CallbackContext ctx)
        {
            if (!hasAuthority) return;
            skillUser.CmdUseSkill(ctx.control.name);
        }

        #endregion
    }
}
