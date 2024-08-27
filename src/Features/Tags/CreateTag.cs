using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain.Tags;
using Tag = WomensWiki.Domain.Tags.Tag;
using WomensWiki.Common.Validation;

namespace WomensWiki.Features.Tags;

public static class CreateTag {
    public record CreateTagCommand(string Name, string? ParentTag = null) : IRequest<Result<TagResponse>>;

    public class CreateTagValidator : AbstractValidator<CreateTagCommand> {
        public CreateTagValidator() {
            RuleFor(x => x.Name).NotEmpty().UniqueName().NoSpaces().MinimumLength(3);
            When(x => x.ParentTag != null, () => {
                RuleFor(x => x.ParentTag).TagExists();
            });
        }
    }

    internal sealed class CreateTagHandler(AppDbContext dbContext, CreateTagValidator validator)
        : IRequestHandler<CreateTagCommand, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(CreateTagCommand request, CancellationToken cancellationToken) {
            var duplicateTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == request.Name);
            var parentTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == request.ParentTag);

            var validationResult = await validator.ValidateAsync(
                Validation.Context(request,
                ("Tag", duplicateTag),
                ("ParentTag", parentTag))
            );

            if (!validationResult.IsValid) {
                return Result.Failure<TagResponse>(ErrorMapper.Map(validationResult));
            }

            var tag = Tag.Create(request.Name, parentTag);
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

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