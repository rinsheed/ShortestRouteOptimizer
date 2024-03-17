namespace ShortestPathFinder.Dto;

public class AdjascentNode : IComparable<AdjascentNode>
{
    public string NodeLabel { get; set; }
    public int Distance { get; set; }

    public AdjascentNode(string destination, int distance)
    {
        NodeLabel = destination;
        Distance = distance;
    }

    public int CompareTo(AdjascentNode other)
    {
        int destinationComparison = NodeLabel.CompareTo(other.NodeLabel);

        if (destinationComparison != 0)
        {
            return destinationComparison;
        }

        return Distance.CompareTo(other.Distance);
    }
}