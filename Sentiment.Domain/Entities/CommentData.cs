using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentiment.Domain.Entities
{
    public class CommentData
    {
        public string? Person { get; set; }
        public string? Comment { get; set; }
        public bool IsPositive { get; set; }
        public string? PersonUrl { get; set; }
        public string? Published { get; set ; }
        public int LikeCount { get; set; }
    }
}
