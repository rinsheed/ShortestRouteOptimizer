namespace ShortestPathFinder.Dto;

public class ShortestPathData
{
    public int Distance { get; set; }
    public List<string> NodeNames { get; set; }

    public ShortestPathData(int distance, List<string> nodeNames)
    {
        Distance = distance;
        NodeNames = nodeNames;
    }
}
