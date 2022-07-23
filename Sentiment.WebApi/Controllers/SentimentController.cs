using Microsoft.AspNetCore.Mvc;
using Sentiment.Domain;
using Sentiment.Domain.Interfaces;

namespace Sentiment.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SentimentController : ControllerBase
    {
        private readonly IParserService _service;
        public SentimentController(IParserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetSentimentalAsync([FromBody] string url)
        {
            var response = new Response();
            var result = await _service.GetCommentsData(url);
            string videoId = url.Substring(url.IndexOf("=") + 1);

            if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("https://www.youtube.com/watch?v="))
            {
                response.Success = false;
                response.Message = "Введен неверный url";
                return BadRequest(response);
            }

            if (result.Count == 0)
            {
                response.Success = false;
                response.Message = "Возникла ошибка при выполнении запроса";
                return BadRequest(response);
            }

            response.Success = true;
            response.Message = "Запрос выполнен";
            response.Comments = result;
            return Ok(response);
        }
    }
}
