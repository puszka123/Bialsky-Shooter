using Priority_Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI.Pathfinding
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] int maxNodes;
        [Inject] Graph graph;
        FastPriorityQueue<Node> frontier;

        private void Awake()
        {
            frontier = new FastPriorityQueue<Node>(maxNodes);
        }

        private void Start()
        {
            graph.Init();
        }

        public IEnumerable<Vector3> FindPath(Vector3 destination)
        {
            var startPosition = graph.ToNode(transform.position);
             var targetPosition = graph.ToNode(destination);
            frontier.Clear();
            frontier.Enqueue(startPosition, 0);
            var cameFrom = new Dictionary<Guid, Node>();
            var costSoFar = new Dictionary<Guid, float>();
            cameFrom[startPosition.Id] = null;
            costSoFar[startPosition.Id] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if(current.Id == targetPosition.Id) break;
                foreach (var next in current.Neighbours.Values)
                {
                    var newCost = costSoFar[current.Id] + Vector3.Distance(current.Position, next.Position);
                    if (!costSoFar.ContainsKey(next.Id) || newCost < costSoFar[next.Id])
                    {
                        costSoFar[next.Id] = newCost;
                        var priority = newCost + Heuristic(targetPosition, next);
                        if (frontier.Contains(next)) frontier.Remove(next);
                        frontier.Enqueue(next, priority);
                        cameFrom[next.Id] = current;
                    }
                }
            }
            List<Node> desiredPath = new List<Node>();
            desiredPath.Add(targetPosition);
            if (!cameFrom.ContainsKey(targetPosition.Id))
            {
                return new List<Vector3>();
            }
            Node start = cameFrom[targetPosition.Id];
            while (start != null)
            {
                desiredPath.Add(start);
                start = cameFrom[start.Id];
            }
            desiredPath.Reverse();
            graph.Remove(startPosition.Id);
            graph.Remove(targetPosition.Id);
            return desiredPath.Select(n => n.Position);
        }

        float Heuristic(Node a, Node b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }
    }
}
