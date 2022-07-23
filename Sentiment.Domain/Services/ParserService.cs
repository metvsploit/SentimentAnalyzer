using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sentiment.Domain.Entities;
using Sentiment.Domain.Interfaces;

namespace Sentiment.Domain.Services
{
    public class ParserService : IParserService
    {
        private readonly List<CommentData> _data = new List<CommentData>();
        private HttpClient _client = new HttpClient();
        private JObject _jObj = new JObject();
        private SentimentalService _sentimental = new SentimentalService();

        public async Task<List<CommentData>> GetCommentsData(string url)
        {
            await GetPageComments(url);
            string token = GetNextPageToken(_jObj);

            if(!string.IsNullOrEmpty(token))
                await GetPageComments(url, token);

            return _data;
        }

        private string GetNextPageToken(JObject obj)
        {
            try
            {
                string nextPage = obj.SelectToken("nextPageToken").Value<string>();
                return nextPage;
            }
            catch
            {
                return "";
            }
        }

        private async Task GetPageComments(string url, string token = "")
        {
            string key = "AIzaSyA-3WAIHB3-5OdrxhHjsO_rikos-0IZEfE";
            string videoId = url.Substring(url.IndexOf("=") + 1);

            string address = token == "" ?
                $"https://www.googleapis.com/youtube/v3/commentThreads?part=snippet&videoId={videoId}&maxResults=100&key={key}"
                : $"https://www.googleapis.com/youtube/v3/commentThreads?part=snippet&videoId=8CQuLoO_zyo&maxResults=100&pageToken={token}&key=AIzaSyA-3WAIHB3-5OdrxhHjsO_rikos-0IZEfE";

            try
            {
                var response = await _client.GetAsync(address);
                if(response.StatusCode.ToString() == "OK")
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    _jObj = (JObject)JsonConvert.DeserializeObject(jsonStr);
                    int countPages = _jObj.SelectToken("pageInfo.totalResults").Value<int>();

                    AddToList( countPages);
                }
            }
            catch
            {

            }
        }

        private void AddToList( int countPages)
        {
            for (int i = 0; i < countPages; i++)
            {
                string comment = _jObj.SelectToken($"items[{i}].snippet.topLevelComment.snippet.textDisplay")
                                  .Value<string>();

                _data.Add(new CommentData
                {
                    Comment = comment,
                    IsPositive = _sentimental.GetSentimantal(comment),
                    Person = _jObj.SelectToken($"items[{i}].snippet.topLevelComment.snippet.authorDisplayName")
                                  .Value<string>(),
                    PersonUrl = _jObj.SelectToken($"items[{i}].snippet.topLevelComment.snippet.authorChannelUrl")
                                  .Value<string>(),
                    Published = _jObj.SelectToken($"items[{i}].snippet.topLevelComment.snippet.publishedAt")
                                  .Value<string>(),
                    LikeCount = _jObj.SelectToken($"items[{i}].snippet.topLevelComment.snippet.likeCount")
                                  .Value<int>(),
                });
            }
        }
    }
}
