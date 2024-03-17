using System.Text.Json;
using ShortestPathFinder;
using ShortestPathFinder.Dto;

public class Program
{
    /// <summary>
    /// The start point of the program.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var inputData = ReadInputFile();

        string[] labels = GetLabelList(inputData.Nodes);

        Console.WriteLine($"The nodes in the graph is {string.Join(',', labels)}. The labels are case-sensitive.");
        while (true)
        {
            string sourceNode = GetInputValue(true, labels);
            string destinationNode = GetInputValue(false, labels);

            var response = inputData.Graph.ShortestPath(sourceNode, destinationNode, inputData.Nodes);

            if (response.Distance == -1)
            {
                Console.WriteLine($"Path not defined in between '{sourceNode}' and '{destinationNode}'");
            }
            else
            {
                Console.WriteLine($"The shortest distance between '{sourceNode}' and '{destinationNode}' is {response.Distance}.");
                Console.WriteLine($"The path is {string.Join(',', response.NodeNames)}.");
            }

            Console.WriteLine("Do you want to exit?(Y/N)");
            string loopBreak = Console.ReadLine();
            if (!string.IsNullOrEmpty(loopBreak) && loopBreak.ToUpper() == "Y")
            {
                break;
            }
        }

    }

    /// <summary>
    /// Get the directory of the project file to read the input file.
    /// </summary>
    /// <returns></returns>
    private static string GetDirectory()
    {
        // Get the current directory where the application is running
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        return $"{currentDirectory.Split("bin")[0]}\\InputFiles";
    }

    /// <summary>
    /// Read the user input to fetch the source and destination node labels
    /// </summary>
    /// <param name="isSource"></param>
    /// <param name="labels"></param>
    /// <returns></returns>
    private static string GetInputValue(bool isSource, string[] labels)
    {
        string nodeLabel;
        while (true)
        {
            Console.WriteLine($"Enter the {(isSource ? "source" : "destination")} node: ");
            nodeLabel = Console.ReadLine();

            if (string.IsNullOrEmpty(nodeLabel) || !labels.Contains(nodeLabel))
            {
                Console.WriteLine("Please enter a valid node label.");
            }
            else
            {
                break;
            }
        }
        return nodeLabel;
    }

    /// <summary>
    /// Read the input json file and return the data.
    /// </summary>
    /// <returns></returns>
    private static (Graph Graph, List<Node> Nodes) ReadInputFile()
    {
        // Specify the relative path to your JSON file (assuming it's in the same folder)
        string filePath = Path.Combine(GetDirectory(), "input.json");

        Graph graph = new Graph();
        List<Node> graphNodes = new List<Node>();
        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<InputNodeData> nodes = JsonSerializer.Deserialize<List<InputNodeData>>(jsonData);
            graphNodes = nodes.Select(x => new Node(x.Source, x.Destination, x.Distance)).ToList();
            graphNodes.AddRange(nodes.Where(x => x.IsBiDirectional).Select(x => new Node(x.Destination, x.Source, x.Distance)));
        }
        catch (IOException ex)
        {
            Console.WriteLine($"ERROR: Invalid file path. Please check the path and below error message.");
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"ERROR: Invalid json file. Please verify the json data.");
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Unknown error occured");
            Console.WriteLine(ex.Message);
            throw;
        }
        return (graph, graphNodes);
    }

    /// <summary>
    /// Get the list of available nodes
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    private static string[] GetLabelList(List<Node> nodes)
    {
        List<string> labelList = nodes.Select(x => x.Source).Distinct().ToList();
        labelList.AddRange(nodes.Where(x => !labelList.Contains(x.Destination)).Select(x => x.Destination));
        labelList = labelList.Distinct().ToList();
        return labelList.ToArray();
    }
}