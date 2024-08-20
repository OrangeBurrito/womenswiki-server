using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Tags;

public static class GetTag {
    public record GetTagRequest(string Name) : IRequest<Result<TagResponse>>;

    internal sealed class GetTagHandler(AppDbContext dbContext) : IRequestHandler<GetTagRequest, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(GetTagRequest request, CancellationToken cancellationToken) {
            var tag = await dbContext.Tags
                .Include(t => t.ParentTags).AsNoTracking()
                .Include(t => t.Articles).AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == request.Name);
                
            if (tag is null) {
                return Result.Failure<TagResponse>( new List<Error> { new("TagNotFound", "Tag not found") });
            }

            return Result.Success(TagResponse.FromTag(tag));
        }
    }

    [QueryType]
    public class GetTagQuery {
        public async Task<Result<TagResponse>> GetTag([Service] ISender sender, GetTagRequest input) {
            return await sender.Send(input);
        }
    }
}