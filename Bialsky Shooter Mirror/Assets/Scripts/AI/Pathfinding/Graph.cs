using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.AI.Pathfinding
{
    public class Graph : MonoBehaviour
    {
        public Dictionary<Guid, Node> AllNodes { get; private set; }

        public void Init()
        {
            AllNodes = new Dictionary<Guid, Node>();
            var allNodesGameObjects = FindObjectsOfType<NodeGO>();
            foreach (var nodeGO in allNodesGameObjects)
            {
                nodeGO.Init();
                AllNodes[nodeGO.node.Id] = nodeGO.node;
            }
            foreach (var node in AllNodes.Values)
            {
                node.InitNeighbours(AllNodes.Values);
            }
        }

        public Node ToNode(Vector3 position)
        {
            var node = new Node(position);
            node.InitNeighbours(AllNodes.Values);
            node.AddSelfToNeighbours();
            return node;
        }
    }
}
