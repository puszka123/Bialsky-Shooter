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
        Controls controls;

        private void Awake()
        {
            skillUser = GetComponent<SkillUser>();
        }

        #region Client

        public override void OnStartClient()
        {
            if (!hasAuthority) return;
            controls = new Controls();
            controls.Enable();
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) return;
            foreach (var control in controls.Player.UseSkill.controls)
            {
                if(control.IsPressed())
                {
                    ClientOnUseSkillPerformed(control);
                }
            }
        }

        private void ClientOnUseSkillPerformed(InputControl ctrl)
        {
            if (!hasAuthority) return;
            var bindingName = !string.IsNullOrEmpty(ctrl.shortDisplayName) ? ctrl.shortDisplayName : ctrl.displayName;
            skillUser.CmdUseSkill(bindingName);
        }

        #endregion
    }
}
