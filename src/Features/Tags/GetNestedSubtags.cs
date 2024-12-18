using System.Text.Json;
using FluentValidation;
using MediatR;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Tags;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class GetNestedSubtags {
    public record GetNestedSubtagsRequest(string Tag) : IRequest<Result<TagTree>>;

    public class GetNestedSubtagsValidator : AbstractValidator<GetNestedSubtagsRequest> {
        public GetNestedSubtagsValidator() {
            RuleFor(x => x.Tag).TagExists();
        }
    }

    internal sealed class GetNestedSubtagsHandler(ITagRepository tagRepository, GetNestedSubtagsValidator validator) : IRequestHandler<GetNestedSubtagsRequest, Result<TagTree>> {
        public async Task<Result<TagTree>> Handle(GetNestedSubtagsRequest request, CancellationToken cancellationToken) {
            var tag = await tagRepository.GetFullTag(request.Tag);

            var validationResult = validator.Validate(Validation.Context(request, ("Tag", tag)));
            if (!validationResult.IsValid) {
                return Result.Failure<TagTree>(ErrorMapper.Map(validationResult));
            }

            var tags = await tagRepository.GetNestedSubtags(tag);
            return Result.Success(tags);
        }
    }

    [QueryType]
    public class GetNestedSubtagsQuery {
        [UseSorting]
        public async Task<Result<TagTree>> GetNestedSubtags([Service] ISender sender, GetNestedSubtagsRequest input) {
            return await sender.Send(input);
        }
    }
}