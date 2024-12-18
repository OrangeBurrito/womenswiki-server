using FluentValidation;
using MediatR;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Tags;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class GetSubtags {
    public record GetSubtagsRequest(string Tag, int Limit = 30, int Offset = 0, bool Descending = false) : IRequest<Result<List<TagResponse>>>;

    public class GetSubtagsValidator : AbstractValidator<GetSubtagsRequest> {
        public GetSubtagsValidator() {
            RuleFor(x => x.Tag).TagExists();
        }
    }

    internal sealed class GetSubtagsHandler(ITagRepository tagRepository, GetSubtagsValidator validator) : IRequestHandler<GetSubtagsRequest, Result<List<TagResponse>>> {
        public async Task<Result<List<TagResponse>>> Handle(GetSubtagsRequest request, CancellationToken cancellationToken) {
            var tag = await tagRepository.GetFullTag(request.Tag);

            var validationResult = validator.Validate(Validation.Context(request, ("Tag", tag)));
            if (!validationResult.IsValid) {
                return Result.Failure<List<TagResponse>>(ErrorMapper.Map(validationResult));
            }
            var sortedTags = tagRepository.SortTags(request.Limit, request.Offset, request.Descending);
            var tags = await tagRepository.GetSubtags(sortedTags, tag!);
            
            return Result.Success(tags.Select(TagResponse.FromTag).ToList());
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