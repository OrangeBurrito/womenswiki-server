using System.Reflection.Metadata.Ecma335;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Articles;

public static class GetArticlesByTag {
    public record GetArticlesByTagRequest(string Tag, int Limit = 10, int Offset = 0, bool Descending = false) : IRequest<Result<List<ArticleResponse>>>;

    internal sealed class GetArticlesByTagHandler(AppDbContext dbContext) : IRequestHandler<GetArticlesByTagRequest, Result<List<ArticleResponse>>> {
        public async Task<Result<List<ArticleResponse>>> Handle(GetArticlesByTagRequest request, CancellationToken cancellationToken) {
            var tag = await dbContext.Tags.Include(a => a.Articles).FirstOrDefaultAsync(t => t.Name == request.Tag);

            if (tag == null) {
                return Result.Failure<List<ArticleResponse>>(new List<Error> { new Error("Tag", "Tag not found")});
            }

            var articles = tag.Articles.Skip(request.Offset).Take(request.Limit).ToList();

            return Result.Success(articles.Select(ArticleResponse.FromArticle).ToList());
        }
    }

    [QueryType]
    public class GetArticlesByTagQuery {
        [UseSorting]
        public async Task<Result<List<ArticleResponse>>> GetArticlesByTag([Service] ISender sender, GetArticlesByTagRequest input) {
            return await sender.Send(input);
        }
    }
}