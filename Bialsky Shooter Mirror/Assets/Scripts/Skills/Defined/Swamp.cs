using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Swamp", menuName = "ScriptableObjects/Skills/Swamp")]
    public class Swamp : Skill
    {
        [SerializeField] GameObject swampPrefab;
        public override void Use(ISkillUser skillUser)
        {
            var swampInstance = Instantiate(swampPrefab, skillUser.GetMouseWorldPosition(), Quaternion.identity);
            NetworkServer.Spawn(swampInstance);
            swampInstance.GetComponent<SwampController>().Init(buffs.First().Create());
        }
    }
}