public class Mutation {
    public async Task<MutationResult<Article>> CreateArticleAsync(
        [Service] ArticleService articleService,
        Article article
        ) => await articleService.CreateArticleAsync(article);
}