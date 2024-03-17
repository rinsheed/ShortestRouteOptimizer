using ShortestPathFinder.Dto;

namespace ShortestPathFinder;

public class Graph
{
    /// <summary>
    /// The graph nodes with key as the current node, and the value as the list of adjascent nodes
    /// </summary>
    private readonly Dictionary<string, List<AdjascentNode>> _graphNodes = [];

    /// <summary>
    /// Add the list of nodes to the graph nodes to create the graph.
    /// </summary>
    /// <param name="nodes"></param>
    private void AddNodesToGraph(List<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            if (_graphNodes.ContainsKey(node.Source))
            {
                _graphNodes[node.Source].Add(new AdjascentNode(node.Destination, node.Distance));
            }
            else
            {
                _graphNodes[node.Source] = [new AdjascentNode(node.Destination, node.Distance)];
            }
        }
    }

    /// <summary>
    /// Get the list of available nodes
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static string[] GetLabelList(List<Node> nodes)
    {
        List<string> labelList = nodes.Select(x => x.Source).Distinct().ToList();
        labelList.AddRange(nodes.Where(x => !labelList.Contains(x.Destination)).Select(x => x.Destination));
        labelList = labelList.Distinct().ToList();
        return [.. labelList];
    }

    /// <summary>
    /// Find the shortest path between two points using Dijikstra's algorithm
    /// </summary>
    /// <param name="fromNodeName"></param>
    /// <param name="toNodeName"></param>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public ShortestPathData ShortestPath(string fromNodeName, string toNodeName, List<Node> nodes)
    {
        Dictionary<string, int> dist = [];
        Dictionary<string, List<string>> paths = [];
        Dictionary<string, string> predecessor = [];

        AddNodesToGraph(nodes);

        var vertices = GetLabelList(nodes);

        foreach (string vertex in vertices)
        {
            dist[vertex] = int.MaxValue;
            predecessor[vertex] = null;
            paths[vertex] = [];
        }

        dist[fromNodeName] = 0;
        paths[fromNodeName].Add(fromNodeName);
        PriorityQueue<AdjascentNode> pq = new();
        pq.Enqueue(new AdjascentNode(fromNodeName, 0));

        while (pq.Count > 0)
        {
            AdjascentNode node = pq.Dequeue();

            // If no path exists, continue.
            if (!_graphNodes.ContainsKey(node.NodeLabel))
            {
                continue;
            }
            foreach (var edge in _graphNodes[node.NodeLabel])
            {
                if (dist[node.NodeLabel] + edge.Distance < dist[edge.NodeLabel])
                {
                    dist[edge.NodeLabel] = dist[node.NodeLabel] + edge.Distance;
                    paths[edge.NodeLabel] = new List<string>(paths[node.NodeLabel])
                    {
                        edge.NodeLabel
                    };
                    predecessor[edge.NodeLabel] = node.NodeLabel;
                    pq.Enqueue(new AdjascentNode(edge.NodeLabel, dist[edge.NodeLabel]));
                }
            }
        }

        if (dist[toNodeName] == int.MaxValue)
        {
            return new ShortestPathData(-1, null);
        }

        return new ShortestPathData(dist[toNodeName], paths[toNodeName]);
    }
}
