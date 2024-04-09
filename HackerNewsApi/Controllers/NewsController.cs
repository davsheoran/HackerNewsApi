using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;

namespace HackerNewsApi
{
    [Route("Api/[Controller]")]
    [ApiController]
    public class NewsController:ControllerBase
    {
        IApiRepo _apiRepo;
        public NewsController(IApiRepo apiRepo)
        {
            _apiRepo=apiRepo;
        }
        [HttpGet]
        [Route("GetNewsPageArticles")]
        public async Task<List<NewsArticle>> GetNewsPageArticlesAsync([FromQuery]int fetchNewCount=200)
        {
            return await _apiRepo.GetNewsArticles(fetchNewCount);
        }
    }
}