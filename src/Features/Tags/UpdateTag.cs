using FluentValidation;
using MediatR;
using WomensWiki.Common.Validation;
using WomensWiki.Contracts;
using WomensWiki.Domain.Tags;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class UpdateTag {
    public record UpdateTagRequest(string tag, string parentTag) : IRequest<Result<TagResponse>>;

    public class UpdateTagValidator : AbstractValidator<UpdateTagRequest> {
        public UpdateTagValidator() {
            RuleFor(x => x.tag).TagExists();
            RuleFor(x => x.parentTag).TagExists("ParentTag").NotChildOfTag();
        }
    }

    internal sealed class UpdateTagHandler(TagRepository repository, UpdateTagValidator validator) : IRequestHandler<UpdateTagRequest, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(UpdateTagRequest request, CancellationToken cancellationToken) {
            var tag = await repository.GetFullTag(request.tag);
            var parentTag = await repository.GetTag(request.parentTag);
            var parentIsChild = await repository.GetNestedSubtags(tag, request.parentTag);
            
            var validationResult = validator.Validate(Validation.Context(request, ("Tag", tag), ("ParentTag", parentTag), ("ParentIsChild", parentIsChild)));
            if (!validationResult.IsValid) {
                return Result.Failure<TagResponse>(ErrorMapper.Map(validationResult));
            }

            await repository.UpdateTag(tag, parentTag);
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