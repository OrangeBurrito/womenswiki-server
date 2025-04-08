using MediatR;
using WomensWiki.Features.Colors.Persistence;

namespace WomensWiki.Features.Colors;

public static class CreateColor {
    public record CreateColorCommand(string Name, string Value) : IRequest<Result<ColorResponse>>;

    internal sealed class CreateColorHandler(
        IColorRepository colorRepository
    ) : IRequestHandler<CreateColorCommand, Result<ColorResponse>> {
        public async Task<Result<ColorResponse>> Handle(CreateColorCommand request, CancellationToken cancellationToken) {
            // var duplicateColor = await colorRepository.GetColor(request.Name);
            
            // var validationResult = await validator.ValidateAsync(
            //     Validation.Context(request, ("Color", duplicateColor))
            // );
            // if (!validationResult.IsValid) {
            //     return Result.Failure<ColorResponse>(ErrorMapper.Map(validationResult));
            // }
            var color = await colorRepository.CreateColor(request.Name, request.Value);
            return Result.Success(ColorResponse.FromColor(color));
        }
    }

    [MutationType]
    public class CreateColorMutation {
        public async Task<Result<ColorResponse>> CreateColor([Service] ISender sender, CreateColorCommand input) {
            return await sender.Send(input);
        }
    }
}