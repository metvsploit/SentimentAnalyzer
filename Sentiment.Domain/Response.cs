
using Sentiment.Domain.Entities;

namespace Sentiment.Domain
{
    public class Response
    {
        public bool Success { get; set; }
        public List<CommentData>? Comments { get; set; }
        public string? Message { get; set; }
    }
}
