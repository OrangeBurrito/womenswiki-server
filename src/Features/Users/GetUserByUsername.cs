using FluentValidation;
using MediatR;
using WomensWiki.Common;
using WomensWiki.Domain;
using WomensWiki.Domain.Users;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.Features.Users;

public static class GetUserByUsername {
	public record GetUserByUsernameRequest(string Username) : IRequest<Result<GetUserByUsernameResponse>>;
	public record GetUserByUsernameResponse(Guid Id, string Username, List<Role> Roles);

	public sealed class GetUserByUsernameValidator : AbstractValidator<GetUserByUsernameRequest> {
		public GetUserByUsernameValidator() {
			RuleFor(x => x.Username).NotEmpty().UserExists();
		}
	}

	internal sealed class GetUserByUsernameHandler(
		IUserRepository userRepository,
		GetUserByUsernameValidator validator
	) : IRequestHandler<GetUserByUsernameRequest, Result<GetUserByUsernameResponse>> {
		public async Task<Result<GetUserByUsernameResponse>> Handle(GetUserByUsernameRequest request, CancellationToken cancellationToken) {
			var user = await userRepository.GetUserByUsername(request.Username);

			var validationResult = await validator.ValidateAsync(Validation.Context(request, ("User", user)));
			if (!validationResult.IsValid) {
				return Result.Failure<GetUserByUsernameResponse>(ErrorMapper.Map(validationResult));
			}

			return Result.Success(new GetUserByUsernameResponse(user.Id, user.Username, user.Roles));
		}
	}

	[QueryType]
	public class GetUserByUsernameQuery {
		public async Task<Result<GetUserByUsernameResponse>> GetUserByUsernameAsync([Service] ISender sender, GetUserByUsernameRequest input) {
			return await sender.Send(input);
		}
	}
}