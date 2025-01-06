using LecMap.Helper;
using LecMap.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static LecMap.Helper.LocationEnums;

namespace LecMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly ILLMInterface _LLM;
        private readonly FindPath _path;

        public PathController(ILLMInterface LLM, FindPath path)
        {
            _LLM = LLM;
            _path = path;
        }

        [HttpGet("getpath")]
        public async Task<IActionResult> GetPath(string question)
        {
            var nodes = await _LLM.GetNodesAsync(question);
            // Extract enum values from the question string
            var enumNames = Enum.GetNames(typeof(Location));
            var words = nodes.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var matchedEnums = words
                .Where(word => enumNames.Contains(word, StringComparer.OrdinalIgnoreCase))
                .Select(word => Enum.Parse<Location>(word, true))
                .ToList();

            if (matchedEnums.Count < 2)
            {
                return BadRequest("The question must contain at least two valid locations.");
            }

            // Assuming the first two matches are start and end locations
            var start = matchedEnums[0].ToString();
            var end = matchedEnums[1].ToString();

            var result = await _path.GetPathWithInstructionsAsync(start, end);

            //call the llm api to get the prefered answer in human form

            var res = await _LLM.FormatPathAsync(question, result);

            return Ok(res);
        }
    }
}
