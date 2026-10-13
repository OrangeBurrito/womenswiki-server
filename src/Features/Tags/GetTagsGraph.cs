using MediatR;
using WomensWiki;
using WomensWiki.Features.Tags;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class GetTagsGraph {
	public record GetTagsGraphRequest() : IRequest<Result<List<TagResponse>>>;

	internal sealed class GetTagsGraphHandler(ITagRepository tagRepository) : IRequestHandler<GetTagsGraphRequest, Result<List<TagResponse>>> {
		public async Task<Result<List<TagResponse>>> Handle(GetTagsGraphRequest request, CancellationToken cancellationToken) {
			var tags = await tagRepository.GetTags(int.MaxValue, 0, false);

			return Result.Success(tags.Select(TagResponse.FromTag).ToList());
		}
	}

	[QueryType]
	public class GetTagsGraphQuery {
		[UseSorting]
		public async Task<Result<List<TagResponse>>> GetTagsGraph([Service] ISender sender, GetTagsGraphRequest input) {
			return await sender.Send(input);
		}
	}
}