using ShortestPathFinder.Dto;

namespace ShortestPathFinder;

public class Graph
{
    private Dictionary<string, List<AdjascentNode>> _graphNodes = new();

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
                _graphNodes[node.Source] = new List<AdjascentNode>() { new AdjascentNode(node.Destination, node.Distance) };
            }
        }
    }

    public ShortestPathData ShortestPath(string fromNodeName, string toNodeName, List<Node> nodes)
    {
        Dictionary<string, int> dist = new Dictionary<string, int>();
        Dictionary<string, List<string>> paths = new Dictionary<string, List<string>>();
        Dictionary<string, string> predecessor = new Dictionary<string, string>();

        AddNodesToGraph(nodes);

        foreach (string vertex in _graphNodes.Keys)
        {
            dist[vertex] = int.MaxValue;
            predecessor[vertex] = null;
            paths[vertex] = new List<string>();
        }

        dist[fromNodeName] = 0;
        paths[fromNodeName].Add(fromNodeName);
        PriorityQueue<AdjascentNode> pq = new PriorityQueue<AdjascentNode>();
        pq.Enqueue(new AdjascentNode(fromNodeName, 0));

        while (pq.Count > 0)
        {
            AdjascentNode node = pq.Dequeue();

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

    public string[] GetAllNodeLabels()
    {
        return _graphNodes.Keys.ToArray();
    }

}
