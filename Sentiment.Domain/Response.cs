using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentiment.Domain
{
    public class Response
    {
        public bool Success { get; set; }
        public List<string>? Comments { get; set; }
        public string? Message { get; set; }
    }
}
