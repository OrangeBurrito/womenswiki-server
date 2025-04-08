using MediatR;
using WomensWiki.Contracts;
using WomensWiki.Features.Colors.Persistence;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class UpdateTagColor {
    public record UpdateTagColorCommand(string tag, string color) : IRequest<Result<TagResponse>>;

    internal sealed class UpdateTagColorHandler(
        ITagRepository tagRepository,
        IColorRepository colorRepository
      ) : IRequestHandler<UpdateTagColorCommand, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(UpdateTagColorCommand request, CancellationToken cancellationToken) {
            var tag = await tagRepository.GetTag(request.tag);
            var color = await colorRepository.GetColor(request.color);

            await tagRepository.UpdateTagColor(tag, color);
            return Result.Success(TagResponse.FromTag(tag));
        }
    }

    [MutationType]
    public class UpdateTagColorMutation {
        public async Task<Result<TagResponse>> UpdateTagColor([Service] ISender sender, UpdateTagColorCommand input) {
            return await sender.Send(input);
        }
    }
}