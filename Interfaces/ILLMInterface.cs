namespace LecMap.Interfaces
{
    public interface ILLMInterface
    {
        Task<string> FormatPathAsync(string userQuestion, string pathDescription);

        Task<string> GetNodesAsync(string userQuestion);
    }
}
