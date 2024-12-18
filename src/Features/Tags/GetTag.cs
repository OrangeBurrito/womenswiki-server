using MediatR;
using WomensWiki.Contracts;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class GetTag {
    public record GetTagRequest(string Name) : IRequest<Result<TagResponse>>;

    internal sealed class GetTagHandler(ITagRepository tagRepository) : IRequestHandler<GetTagRequest, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(GetTagRequest request, CancellationToken cancellationToken) {
            var tag = await tagRepository.GetFullTag(request.Name);

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