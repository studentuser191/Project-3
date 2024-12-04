using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetworkAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Social Network Influence Calculator!");
            Console.WriteLine("Choose the graph type:");
            Console.WriteLine("1. Unweighted Graph");
            Console.WriteLine("2. Weighted Graph");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
                UnweightedGraphDemo();
            else if (choice == 2)
                WeightedGraphDemo();
            else
                Console.WriteLine("Invalid choice.");
        }

        static void UnweightedGraphDemo()
        {
            var graph = new Dictionary<string, List<string>>
            {
                { "Alicia", new List<string> { "Britney" } },
                { "Britney", new List<string> { "Claire" } },
                { "Claire", new List<string> { "Diana" } },
                { "Diana", new List<string> { "Edward", "Harry" } },
                { "Edward", new List<string> { "Harry", "Gloria", "Fred" } },
                { "Harry", new List<string> { "Gloria" } },
                { "Gloria", new List<string> { "Fred" } },
                { "Fred", new List<string>() }
            };

            Console.WriteLine("\nList 1: edge_list of unweighted social network");
            Console.WriteLine("Node1 Node2");
            foreach (var node in graph)
            {
                foreach (var neighbor in node.Value)
                {
                    Console.WriteLine($"{node.Key} {neighbor}");
                }
            }

            Console.WriteLine("\nUnweighted Graph Influence Scores:");
            foreach (var node in graph.Keys)
            {
                double score = CalculateInfluenceScoreUnweighted(graph, node);
                Console.WriteLine($"{node}: {score:F2}");
            }
        }

        static double CalculateInfluenceScoreUnweighted(Dictionary<string, List<string>> graph, string startNode)
        {
            var distances = new Dictionary<string, int>();
            foreach (var node in graph.Keys)
                distances[node] = int.MaxValue;

            distances[startNode] = 0;
            var queue = new Queue<string>();
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                foreach (var neighbor in graph[current])
                {
                    if (distances[neighbor] == int.MaxValue)
                    {
                        distances[neighbor] = distances[current] + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            double sumDistances = distances.Values.Where(d => d != int.MaxValue).Sum();
            return (graph.Count - 1) / sumDistances;
        }

        static void WeightedGraphDemo()
        {
            var graph = new Dictionary<string, List<(string, int)>>
            {
                { "A", new List<(string, int)> { ("B", 1), ("C", 1), ("E", 5) } },
                { "B", new List<(string, int)> { ("C", 4), ("E", 1), ("G", 1), ("H", 1) } },
                { "C", new List<(string, int)> { ("D", 3), ("E", 1) } },
                { "D", new List<(string, int)> { ("E", 2), ("F", 1), ("G", 5) } },
                { "E", new List<(string, int)> { ("G", 2) } },
                { "F", new List<(string, int)> { ("G", 1) } },
                { "G", new List<(string, int)> { ("H", 2) } },
                { "H", new List<(string, int)> { ("I", 3) } },
                { "I", new List<(string, int)> { ("J", 3) } },
                { "J", new List<(string, int)>() }
            };

            Console.WriteLine("\nList 2: edge_list of weighted social network");
            Console.WriteLine("Node1 Node2 Weight");
            foreach (var node in graph)
            {
                foreach (var (neighbor, weight) in node.Value)
                {
                    Console.WriteLine($"{node.Key} {neighbor} {weight}");
                }
            }

            Console.WriteLine("\nWeighted Graph Influence Scores:");
            foreach (var node in graph.Keys)
            {
                double score = CalculateInfluenceScoreWeighted(graph, node);
                Console.WriteLine($"{node}: {score:F2}");
            }
        }

        static double CalculateInfluenceScoreWeighted(Dictionary<string, List<(string, int)>> graph, string startNode)
        {
            var distances = new Dictionary<string, int>();
            foreach (var node in graph.Keys)
                distances[node] = int.MaxValue;

            distances[startNode] = 0;
            var priorityQueue = new SortedSet<(int Distance, string Node)>();
            priorityQueue.Add((0, startNode));

            while (priorityQueue.Count > 0)
            {
                var (currentDistance, currentNode) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                foreach (var (neighbor, weight) in graph[currentNode])
                {
                    int newDistance = currentDistance + weight;
                    if (newDistance < distances[neighbor])
                    {
                        priorityQueue.Remove((distances[neighbor], neighbor));
                        distances[neighbor] = newDistance;
                        priorityQueue.Add((newDistance, neighbor));
                    }
                }
            }

            double sumDistances = distances.Values.Where(d => d != int.MaxValue).Sum();
            return (graph.Count - 1) / sumDistances;
        }
    }
}
