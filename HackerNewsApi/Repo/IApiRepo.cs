using HackerNewsApi;

public interface IApiRepo
{
    public Task<List<int>> GetNewsIdList();
    public Task<List<NewsArticle>> GetNewsArticles(int fetchNewCount);
}