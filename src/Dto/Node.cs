namespace ShortestPathFinder.Dto;

public class Node
{
    public string Source { get; set; }
    public string Destination { get; set; }
    public int Distance { get; set; }

    public Node(string source, string destination, int distance)
    {
        Source = source;
        Destination = destination;
        Distance = distance;
    }
}

public class InputNodeData : Node
{
    public bool IsBiDirectional { get; set; }

    public InputNodeData(string source, string destination, int distance, bool isBiDirectional) : base(source, destination, distance)
    {
        IsBiDirectional = isBiDirectional;
    }
}
