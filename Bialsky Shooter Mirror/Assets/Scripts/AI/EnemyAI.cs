using System.Collections;
using System.Collections.Generic;
using BialskyShooter.AI.Pathfinding;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using Mirror;
using UnityEngine;
using System.Linq;

namespace BialskyShooter.AI
{
    public class EnemyAI : NetworkBehaviour
    {
        

        #region Server

        [ServerCallback]
        void Update()
        {
            
        }

        #endregion
    }
}