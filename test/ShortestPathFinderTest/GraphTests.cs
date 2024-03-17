using ShortestPathFinder.Dto;
using ShortestPathFinder;
using System.Text.Json;

namespace ShortestPathFinderTest
{
    public class GraphTests
    {
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
        /// Read the input test file
        /// </summary>
        /// <returns></returns>
        private static (Graph Graph, List<Node> Nodes) ReadInputFile()
        {
            // Specify the relative path to your JSON file (assuming it's in the same folder)
            string filePath = Path.Combine(GetDirectory(), "input.json");

            Graph graph = new();
            List<Node> graphNodes = [];

            string jsonData = File.ReadAllText(filePath);
            List<InputNodeData> nodes = JsonSerializer.Deserialize<List<InputNodeData>>(jsonData);
            graphNodes = nodes.Select(x => new Node(x.Source, x.Destination, x.Distance)).ToList();
            graphNodes.AddRange(nodes.Where(x => x.IsBiDirectional).Select(x => new Node(x.Destination, x.Source, x.Distance)));

            return (graph, graphNodes);
        }

        [Fact]
        public void ShortestPath_WithDirectEdge_ReturnDistanceWithPath()
        {
            var inputData = ReadInputFile();

            var response = inputData.Graph.ShortestPath("A", "B", inputData.Nodes);

            Assert.Equal(4, response.Distance);

            Assert.Equal(2, response.NodeNames.Count);
        }

        [Fact]
        public void ShortestPath_WithMultiplePath_ReturnShortestDistanceWithPath()
        {
            var inputData = ReadInputFile();

            var response = inputData.Graph.ShortestPath("B", "D", inputData.Nodes);

            Assert.Equal(7, response.Distance);

            Assert.Equal(4, response.NodeNames.Count);
        }

        [Fact]
        public void ShortestPath_WithNoPathExist_ReturnInvalidDistanceValue()
        {
            var inputData = ReadInputFile();

            var response = inputData.Graph.ShortestPath("J", "I", inputData.Nodes);

            Assert.Equal(-1, response.Distance);
        }
    }
}