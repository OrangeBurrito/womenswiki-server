using FluentValidation;
using MediatR;
using WomensWiki.Common;
using WomensWiki.Domain;
using WomensWiki.Domain.Users;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.Features.Users;

public static class GetUserById {
	public record GetUserByIdRequest(Guid Id) : IRequest<Result<GetUserByIdResponse>>;
	public record GetUserByIdResponse(Guid Id, string Username, string Email, List<Role> Roles);

	public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdRequest> {
		public GetUserByIdValidator() {
			RuleFor(x => x.Id).NotEmpty().UserExists();
		}
	}

	internal sealed class GetUserByIdHandler(
		IUserRepository userRepository,
		GetUserByIdValidator validator
	) : IRequestHandler<GetUserByIdRequest, Result<GetUserByIdResponse>> {
		public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken cancellationToken) {
			var user = await userRepository.GetUserById(request.Id);

			var validationResult = await validator.ValidateAsync(Validation.Context(request, ("User", user)));
			if (!validationResult.IsValid) {
				return Result.Failure<GetUserByIdResponse>(ErrorMapper.Map(validationResult));
			}

			return Result.Success(new GetUserByIdResponse(user.Id, user.Username, user.Email, user.Roles));
		}
	}

	[QueryType]
	public class GetUserByIdQuery {
		public async Task<Result<GetUserByIdResponse>> GetUserByIdAsync([Service] ISender sender, GetUserByIdRequest input) {
			return await sender.Send(input);
		}
	}
}