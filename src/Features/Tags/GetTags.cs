using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Tags;

public static class GetTags {
    public record GetTagsRequest(int Limit = 30, int Offset = 0, bool Descending = false) : IRequest<Result<List<TagResponse>>>;

    internal sealed class GetTagsHandler(AppDbContext dbContext) : IRequestHandler<GetTagsRequest, Result<List<TagResponse>>> {
        public async Task<Result<List<TagResponse>>> Handle(GetTagsRequest request, CancellationToken cancellationToken) {
            var query = request.Descending ?
                dbContext.Tags.OrderByDescending(t => t.CreatedAt) :
                dbContext.Tags.OrderBy(t => t.CreatedAt);
                // todo: sort alphabetically

            var tags = await query
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();

            return Result.Success(tags.Select(t => new TagResponse(t.Id, t.CreatedAt, t.Name)).ToList());
        }
    }

    [QueryType]
    public class GetTagsQuery {
        [UseSorting]
        public async Task<Result<List<TagResponse>>> GetTagsAsync([Service] ISender sender, GetTagsRequest input) {
            return await sender.Send(input);
        }
    }
}