namespace ShortestPathFinder.Dto;

public class AdjascentNode(string destination, int distance) : IComparable<AdjascentNode>
{
    public string NodeLabel { get; set; } = destination;
    public int Distance { get; set; } = distance;

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