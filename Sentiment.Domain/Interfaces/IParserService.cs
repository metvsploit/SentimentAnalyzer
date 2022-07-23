using Sentiment.Domain.Entities;

namespace Sentiment.Domain.Interfaces
{
    public interface IParserService
    {
        Task<List<CommentData>> GetCommentsData(string url);
    }
}
