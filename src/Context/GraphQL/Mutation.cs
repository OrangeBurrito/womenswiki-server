public class Mutation {
    public async Task<Article> CreateArticleAsync(ArticleService articleService, ArticleDTO articleDTO) =>
        await articleService.CreateArticleAsync(articleDTO);
}