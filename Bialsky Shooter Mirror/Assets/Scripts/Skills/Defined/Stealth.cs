using BialskyShooter.Core;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Stealth", menuName = "ScriptableObjects/Skills/Stealth")]
    public class Stealth : Skill
    {
        [SerializeField] float stealthTime = 5f;
        
        public override void Use(ISkillUser skillUser)
        {
            var rendererProxy = skillUser.GetTransform().GetComponent<RendererProxy>();
            rendererProxy.ServerBecomeInvisibleForOthers(stealthTime);
            rendererProxy.ServerBecomeTransparentForYourself(stealthTime);
        }
    }
}
