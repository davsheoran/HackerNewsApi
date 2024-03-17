using HackerNewsApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
namespace HackerNewsXUnitTest;

public class ApiRepo_Test
{
    HttpClient httpClient;
    IMemoryCache memoryCache;
    public ApiRepo_Test()
    {
        httpClient= new HttpClient() { BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/") };
        var services = new ServiceCollection();
        services.AddMemoryCache(); // Register the memory cache service
        var serviceProvider = services.BuildServiceProvider();
        memoryCache = serviceProvider.GetService<IMemoryCache>();
    }
    [Fact]
    public async void Test_GetNewsIdList()
    {
        IApiRepo _apiRepo = new ApiRepo(httpClient, memoryCache);
        var res = await _apiRepo.GetNewsIdList();
        Assert.IsType<List<int>>(res);
    }
    [Fact]
    public async void Test_GetNewsArticles()
    {
        IApiRepo _apiRepo = new ApiRepo(httpClient, memoryCache);
        var res = await _apiRepo.GetNewsArticles(1);
        Assert.IsType<List<NewsArticle>>(res);
    }
}