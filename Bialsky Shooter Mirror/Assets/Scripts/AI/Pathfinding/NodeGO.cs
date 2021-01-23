using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI.Pathfinding
{
    public class NodeGO : MonoBehaviour
    {
        public Node node;

        public void Init()
        {
            node = new Node(transform.position);
        }
    }
}