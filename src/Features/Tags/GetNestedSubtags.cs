using FluentValidation;
using MediatR;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Tags;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class GetNestedSubtags {
    public record GetNestedSubtagsRequest(string Tag) : IRequest<Result<List<TagResponse>>>;

    public class GetNestedSubtagsValidator : AbstractValidator<GetNestedSubtagsRequest> {
        public GetNestedSubtagsValidator() {
            RuleFor(x => x.Tag).TagExists();
        }
    }

    internal sealed class GetNestedSubtagsHandler(TagRepository tagRepository, GetNestedSubtagsValidator validator) : IRequestHandler<GetNestedSubtagsRequest, Result<List<TagResponse>>> {
        public async Task<Result<List<TagResponse>>> Handle(GetNestedSubtagsRequest request, CancellationToken cancellationToken) {
            var tag = await tagRepository.GetFullTag(request.Tag);

            var validationResult = validator.Validate(Validation.Context(request, ("Tag", tag)));
            if (!validationResult.IsValid) {
                return Result.Failure<List<TagResponse>>(ErrorMapper.Map(validationResult));
            }

            var tags = await tagRepository.GetNestedSubtags(tag);
            return Result.Success(tags.Select(TagResponse.FromTag).ToList());
        }
    }

    [QueryType]
    public class GetNestedSubtagsQuery {
        [UseSorting]
        public async Task<Result<List<TagResponse>>> GetNestedSubtags([Service] ISender sender, GetNestedSubtagsRequest input) {
            return await sender.Send(input);
        }
    }
}