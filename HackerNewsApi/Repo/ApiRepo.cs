using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace HackerNewsApi
{
    public class ApiRepo : IApiRepo
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        public ApiRepo(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }
        public async Task<List<int>> GetNewsIdList()
        {
            if (!_memoryCache.TryGetValue("NewsList", out List<int>? newsIdList))
            {
                HttpResponseMessage response = await _httpClient.GetAsync("topstories.json?print=pretty");
                if (response.IsSuccessStatusCode)
                {
                    newsIdList = await response.Content.ReadAsAsync<List<int>>();
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(15)
                    };
                    _memoryCache.Set("NewsList", newsIdList, cacheExpiryOptions);
                }
            }
            return newsIdList;
        }
        public async Task<List<NewsArticle>> GetNewsArticles(int fetchNewCount)
        {
            List<int> newsIdList = await GetNewsIdList();
            List<NewsArticle> ReturnnewsArticles = new List<NewsArticle>();
            _memoryCache.TryGetValue("NewsArticles", out List<NewsArticle>? newsArticles);
            if (newsArticles == null)
                newsArticles = new List<NewsArticle>();

            Parallel.ForEach(newsIdList.Take(fetchNewCount), new ParallelOptions() { MaxDegreeOfParallelism = 4 }, newsID =>
            {
                var news = newsArticles.Find(u => u.Id == newsID);
                if (news == null)
                {
                    var response = _httpClient.GetAsync("item/" + newsID + ".json?print=pretty").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        news = response.Content.ReadAsAsync<NewsArticle>().Result;
                        newsArticles.Add(news);
                    }
                }
                ReturnnewsArticles.Add(news);
            });
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };
            _memoryCache.Set("NewsArticles", newsArticles, cacheExpiryOptions);
            return ReturnnewsArticles;
        }
    }
}