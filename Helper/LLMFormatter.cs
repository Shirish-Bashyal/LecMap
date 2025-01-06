using System.Text;
using GroqSharp;
using GroqSharp.Models;
using LecMap.Interfaces;

namespace LecMap.Helper
{
    public class LLMFormatter : ILLMInterface
    {
        private readonly IConfiguration _configuration;

        public LLMFormatter(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<string> FormatPathAsync(string userQuestion, string pathDescription)
        {
            var apiKey = _configuration["LLM:ApiKey"];
            var apiModel = _configuration["LLM:ApiModel"];
            IGroqClient groqClient = new GroqClient(apiKey, apiModel)
                .SetTemperature(0.5)
                .SetMaxTokens(256)
                .SetTopP(1)
                .SetStop("NONE");

            var prompt = new StringBuilder();

            prompt.AppendLine($"User's Question: {userQuestion}");
            prompt.AppendLine("Path Description:");
            prompt.AppendLine(pathDescription);

            var response = await groqClient.CreateChatCompletionAsync(
                new Message
                {
                    Role = MessageRoleType.System,
                    Content =
                        "You are a helpful assistant designed to format and clarify navigation instructions for users based on the provided path description. Your task is to interpret the path details and provide a clear, concise, and polite navigation instruction in a human-readable format."
                },
                new Message
                {
                    Role = MessageRoleType.Assistant,
                    Content =
                        "Based on the provided Path Description and weights give instruction to user, If no path is detected or the description is unclear, respond with 'No path detected.' and give meaningfull message, Ensure the response is in a friendly, easy-to-understand format. If the path is valid, summarize the key steps of the journey and present them in a natural language format. weights determine the distance in meters"
                },
                new Message { Role = MessageRoleType.User, Content = prompt.ToString() }
            );

            return response;
        }

        public async Task<string> GetNodesAsync(string userQuestion)
        {
            var apiKey = _configuration["LLM:ApiKey"];
            var apiModel = _configuration["LLM:ApiModel"];

            IGroqClient groqClient = new GroqClient(apiKey, apiModel)
                .SetTemperature(0.5)
                .SetMaxTokens(256)
                .SetTopP(1)
                .SetStop("NONE");

            var response = await groqClient.CreateChatCompletionAsync(
                new Message
                {
                    Role = MessageRoleType.System,
                    Content =
                        "You are a question analyzer tasked with identifying the start and end locations mentioned in a user's question. The locations are limited to the following: MainGate, Stationery, WorkShop, Parking, VolleyballCourt, BasketballCourt. You should only return the start and end locations in the format 'StartLocation EndLocation'. If the question does not contain recognizable locations, respond with 'Invalid'."
                },
                new Message
                {
                    Role = MessageRoleType.Assistant,
                    Content =
                        "The valid locations are: MainGate, Stationery, WorkShop, Parking, VollyballCourt, BasketballCourt, BadmintonCourt, WaterTap2, SecondBuilding, Garden, GroundToilet, MainBuilding, Canteen, SoilLab, FootballGround. You must identify the start and end locations mentioned in the user's question. Your response should be formatted as two words separated by a space, for example: 'MainGate Parking'. If the locations cannot be identified, reply with 'Invalid'. Analyze the user description and determine where the user is and where user want to go and respond based on defined locations only"
                },
                new Message { Role = MessageRoleType.User, Content = userQuestion }
            );

            return response;
        }
    }
}
