using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    [CreateAssetMenu(fileName = "StateGraph", menuName = "ScriptableObjects/AI/StateGraph")]
    public class StateGraph : ScriptableObject
    {
        public StateNode[] actionNodes;

        [Serializable]
        public class StateNode
        {
            public ActionId action;
            public List<ActionId> actionConnections;
        }
    }
}
