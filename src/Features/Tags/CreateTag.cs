using FluentValidation;
using MediatR;
using WomensWiki.Contracts;
using WomensWiki.Domain.Tags;
using WomensWiki.Common.Validation;
using WomensWiki.Features.Tags.Persistence;

namespace WomensWiki.Features.Tags;

public static class CreateTag {
    public record CreateTagCommand(string Name, string? ParentTag = null) : IRequest<Result<TagResponse>>;

    public class CreateTagValidator : AbstractValidator<CreateTagCommand> {
        public CreateTagValidator() {
            RuleFor(x => x.Name).NotEmpty().UniqueName().NoSpaces().MinimumLength(3);
            When(x => x.ParentTag != null, () => {
                RuleFor(x => x.ParentTag).TagExists("ParentTag");
            });
        }
    }

    internal sealed class CreateTagHandler(ITagRepository tagRepository, CreateTagValidator validator)
        : IRequestHandler<CreateTagCommand, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(CreateTagCommand request, CancellationToken cancellationToken) {
            var duplicateTag = await tagRepository.GetTag(request.Name);
            var parentTag = await tagRepository.GetTag(request.ParentTag);

            var validationResult = await validator.ValidateAsync(
                Validation.Context(request,
                ("Tag", duplicateTag),
                ("ParentTag", parentTag))
            );
            if (!validationResult.IsValid) {
                return Result.Failure<TagResponse>(ErrorMapper.Map(validationResult));
            }

            var tag = await tagRepository.CreateTag(request.Name, parentTag);
            return Result.Success(TagResponse.FromTag(tag));
        }
    }

    [MutationType]
    public class CreateTagMutation {
        public async Task<Result<TagResponse>> CreateTag([Service] ISender sender, CreateTagCommand input) {
            return await sender.Send(input);
        }
    }
}