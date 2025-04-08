using MediatR;
using WomensWiki.Contracts;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class GetTags {
    public record GetTagsRequest(int Limit = 30, int Offset = 0, bool Descending = false) : IRequest<Result<List<TagResponse>>>;

    internal sealed class GetTagsHandler(ITagRepository tagRepository) : IRequestHandler<GetTagsRequest, Result<List<TagResponse>>> {
        public async Task<Result<List<TagResponse>>> Handle(GetTagsRequest request, CancellationToken cancellationToken) {
            var tags = await tagRepository.GetTags(request.Limit, request.Offset, request.Descending);

            return Result.Success(tags.Select(TagResponse.FromTag).ToList());
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