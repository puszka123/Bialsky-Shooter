using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace BialskyShooter.AI.Pathfinding
{
    public class Node : FastPriorityQueueNode
    {
        LayerMask layerMask = new LayerMask();
        Guid id;
        Vector3 position;

        public Dictionary<Guid, Node> Neighbours { get; private set; }
        public Guid Id { get { return id; } }
        public Vector3 Position { get { return position; } }

        public Node(Vector3 position)
        {
            id = Guid.NewGuid();
            this.position = position;
            layerMask = LayerMask.GetMask("Terrain");
        }

        public void InitNeighbours(IEnumerable<Node> allNodes)
        {
            if (Neighbours != null) return;
            Neighbours = GetNeighbours(allNodes);
        }

        public Dictionary<Guid, Node> GetNeighbours(IEnumerable<Node> allNodes)
        {
            var neighbours = new Dictionary<Guid, Node>();
            foreach (var node in allNodes)
            {
                if (node.id == id) continue;
                if (Physics.Linecast(position, node.position, out RaycastHit hit, layerMask)) continue;
                neighbours[node.id] = node;
            }
            return neighbours;
        }

        public void AddSelfToNeighbours()
        {
            foreach (var node in Neighbours.Values)
            {
                node.AddNeighbour(this);
            }
        }

        public void AddNeighbour(Node node)
        {
            Neighbours[node.id] = node;
        }
    }
}
