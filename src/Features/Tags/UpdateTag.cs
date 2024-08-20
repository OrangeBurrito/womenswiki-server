using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Tags;

public static class UpdateTag {
    public record UpdateTagRequest(Guid Id, string parentTag) : IRequest<Result<TagResponse>>;

    internal sealed class UpdateTagHandler(AppDbContext dbContext) : IRequestHandler<UpdateTagRequest, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(UpdateTagRequest request, CancellationToken cancellationToken) {
            var tag = await dbContext.Tags
                .Include(t => t.ParentTags)
                .Include(t => t.Articles)
                .FirstOrDefaultAsync(t => t.Id == request.Id);

            var parentTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == request.parentTag);
            
            if (tag is null) {
                return Result.Failure<TagResponse>(new List<Error>{});
            }

            tag.Update(parentTag);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(TagResponse.FromTag(tag));
        }
    }

    [QueryType]
    public class UpdateTagQuery {
        public async Task<Result<TagResponse>> UpdateTag([Service] ISender sender, UpdateTagRequest input) {
            return await sender.Send(input);
        }
    }
}