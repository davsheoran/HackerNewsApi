using System.Linq.Expressions;
using HackerNewsApi;
using Moq;
using Moq.Protected;

namespace HackerNewsXUnitTest;
public class NewsController_Test
{
    [Fact]
    public async void GetNewsPageArticles_Test()
    {
        List<NewsArticle> newsArticle=new List<NewsArticle>(){new NewsArticle(){Id=1,Title="test News",Url="http://www.demo.com"}};
        Mock<IApiRepo> mockApiRepo=new Mock<IApiRepo>();
        mockApiRepo.Setup(x=>x.GetNewsArticles(1)).ReturnsAsync(newsArticle);

        NewsController news=new NewsController(mockApiRepo.Object);
        var res=await news.GetNewsPageArticlesAsync(1);

        Assert.IsType<List<NewsArticle>>(res);
        Assert.Equal(newsArticle,res);
    }
}