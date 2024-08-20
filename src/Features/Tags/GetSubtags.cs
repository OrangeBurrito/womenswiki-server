using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;

namespace WomensWiki.Features.Tags;

public static class GetSubtags {
    public record GetSubtagsRequest(string ParentTag, int Limit = 30, int Offset = 0, bool Descending = false) : IRequest<Result<List<TagResponse>>>;

    internal sealed class GetSubtagsHandler(AppDbContext dbContext) : IRequestHandler<GetSubtagsRequest, Result<List<TagResponse>>> {
        public async Task<Result<List<TagResponse>>> Handle(GetSubtagsRequest request, CancellationToken cancellationToken) {
            var query = request.Descending ?
                dbContext.Tags.OrderByDescending(t => t.CreatedAt) :
                dbContext.Tags.OrderBy(t => t.CreatedAt);
            
            var tags = await query
                .Where(t => t.ParentTags.Any(pt => pt.Name == request.ParentTag))
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();

            return Result.Success(tags.Select(tag => TagResponse.FromTag(tag)).ToList());
        }
    }

    [QueryType]
    public class GetSubtagsQuery {
        [UseSorting]
        public async Task<Result<List<TagResponse>>> GetSubtags([Service] ISender sender, GetSubtagsRequest input) {
            return await sender.Send(input);
        }
    }
}