using System.Security.Claims;
using HotChocolate.Authorization;
using MediatR;
using WomensWiki.Common;
using WomensWiki.Domain;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.Features.Users;

public static class GetCurrentUser {
	public record GetCurrentUserRequest(Guid Id) : IRequest<Result<GetCurrentUserResponse>>;
	public record GetCurrentUserResponse(Guid Id, string Username, string Email, List<Role> Roles);

	internal sealed class GetCurrentUserHandler(IUserRepository userRepository) : IRequestHandler<GetCurrentUserRequest, Result<GetCurrentUserResponse>> {
		public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken) {
			var user = await userRepository.GetUserById(request.Id);
			if (user is null) {
				return Result.Failure<GetCurrentUserResponse>(new List<Error> { new Error("UserNotFound", "The user was not found.") });
			}

			return Result.Success(new GetCurrentUserResponse(user.Id, user.Username, user.Email, user.Roles));
		}
	}

	[QueryType]
	public class GetCurrentUserQuery {
		// [RoleAuthorize(Constants.Role.Editor | Constants.Role.Admin)]
		[Authorize]
		public async Task<Result<GetCurrentUserResponse>> GetCurrentUserAsync([Service] ISender sender, ClaimsPrincipal claimsPrincipal) {
			return await sender.Send(new GetCurrentUserRequest(claimsPrincipal.UserId()));
		}
	}
}