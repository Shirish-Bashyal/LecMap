using System.Text;
using Neo4j.Driver;

namespace LecMap.Helper
{
    public class FindPath
    {
        private readonly IConfiguration _Config;

        public FindPath(IConfiguration config)
        {
            _Config = config;
        }

        public async Task<string> GetPathWithInstructionsAsync(
            string startLocation,
            string endLocation
        )
        {
            var uri = _Config["Neo4jSettings:Uri"];
            var username = _Config["Neo4jSettings:UserName"];
            var password = _Config["Neo4jSettings:Password"];

            using var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
            await using var session = driver.AsyncSession();

            try
            {
                var query =
                    @"
            MATCH (start:Location {name: $start}), (end:Location {name: $end})
            CALL apoc.algo.dijkstra(start, end, 'CONNECTED_TO', 'weight') YIELD path, weight
            RETURN path, weight";

                var result = await session.RunAsync(
                    query,
                    new { start = startLocation, end = endLocation }
                );

                var output = new StringBuilder();
                await result.ForEachAsync(record =>
                {
                    var path = record["path"].As<IPath>();
                    var totalWeight = record["weight"].As<double>();

                    var nodes = path.Nodes;
                    var relationships = path.Relationships;

                    output.AppendLine($"Total path weight: {totalWeight}");

                    for (int i = 0; i < nodes.Count - 1; i++)
                    {
                        var currentNode = nodes[i];
                        var nextNode = nodes[i + 1];
                        var relationship = relationships[i];

                        var instruction = relationship.Properties["instruction"].As<string>();
                        output.AppendLine(
                            $"{currentNode["name"]} -> {instruction} -> {nextNode["name"]}"
                        );
                    }

                    output.AppendLine($"Final Destination: {nodes.Last()["name"]}");
                });

                return output.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
