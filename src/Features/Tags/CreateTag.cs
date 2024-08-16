using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WomensWiki.Common;
using WomensWiki.Contracts;
using WomensWiki.Domain.Articles;
using WomensWiki.Domain.Tags;
using Tag = WomensWiki.Domain.Tags.Tag;

namespace WomensWiki.Features.Tags;

public static class CreateTag {
    public record CreateTagCommand(string Name) : IRequest<Result<TagResponse>>;

    public class CreateTagValidator : AbstractValidator<CreateTagCommand> {
        public CreateTagValidator() {
            RuleFor(x => x.Name).NotEmpty().UniqueName().NoSpaces().MinimumLength(3);
        }
    }

    internal sealed class CreateTagHandler(AppDbContext dbContext, CreateTagValidator validator)
        : IRequestHandler<CreateTagCommand, Result<TagResponse>> {
        public async Task<Result<TagResponse>> Handle(CreateTagCommand request, CancellationToken cancellationToken) {
            var duplicateTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == request.Name);

            var context = new ValidationContext<CreateTagCommand>(request);
            context.RootContextData["Tag"] = duplicateTag;
            var validationResult = await validator.ValidateAsync(context);

            if (!validationResult.IsValid) {
                return Result.Failure<TagResponse>(ErrorMapper.Map(validationResult));
            }

            var tag = Tag.Create(request.Name);
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            return Result.Success(new TagResponse(tag.Id, tag.CreatedAt, tag.Name));
        }
    }

    [MutationType]
    public class CreateTagMutation {
        public async Task<Result<TagResponse>> CreateTag([Service] ISender sender, CreateTagCommand input) {
            return await sender.Send(input);
        }
    }
}