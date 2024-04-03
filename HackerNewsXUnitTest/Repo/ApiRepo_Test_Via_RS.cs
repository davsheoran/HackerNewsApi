using HackerNewsApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsXUnitTest.Repo
{
    public class ApiRepo_Test_Via_RS
    {
        IMemoryCache memoryCache;
        public ApiRepo_Test_Via_RS()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache(); // Register the memory cache service
            var serviceProvider = services.BuildServiceProvider();
            memoryCache = serviceProvider.GetService<IMemoryCache>();
        }
        [Fact]
        public async void Test_GetNewsIdList()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty")
                        .Respond("application/json", "[1,2,3,4]"); // Respond with JSON
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
            IApiRepo _apiRepo = new ApiRepo(httpClient, memoryCache);
            var res = await _apiRepo.GetNewsIdList();
            Assert.IsType<List<int>>(res);
        }
        [Fact]
        public async void Test_GetNewsArticles()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty")
                       .Respond("application/json", "[1]"); // Respond with JSON
            mockHttp.When("https://hacker-news.firebaseio.com/v0/item/*")
                        .Respond("application/json", "{'id':'1','title':'Title1','url':'abc.com'}"); // Respond with JSON
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
            IApiRepo _apiRepo = new ApiRepo(httpClient, memoryCache);
            var res = await _apiRepo.GetNewsArticles(1);
            Assert.IsType<List<NewsArticle>>(res);
        }
    }
}
