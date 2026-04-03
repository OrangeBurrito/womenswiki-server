using FluentValidation;
using MediatR;
using WomensWiki.Domain;
using WomensWiki.Domain.Users;
using WomensWiki.Features.Users.Persistence;
using WomensWiki.Common;
using WomensWiki.Common.Attributes;

namespace WomensWiki.Features.Users;

public static class UpdateUserRole {
	public record UpdateUserRoleCommand(Guid UserId, Guid AdminUserId, Constants.Role Role) : IRequest<Result<UpdateUserRoleResponse>>;
	public record UpdateUserRoleResponse(Guid Id, string username, Constants.Role Role);

	public sealed class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleCommand> {
		public UpdateUserRoleValidator() {
			RuleFor(x => x.UserId).UserExists();
			RuleFor(x => x.AdminUserId).UserExists("AdminUser");
			RuleFor(x => x.Role).IsInEnum();
		}
	}

	internal sealed class UpdateUserRoleHandler(
		IUserRepository userRepository,
		UpdateUserRoleValidator validator
	) : IRequestHandler<UpdateUserRoleCommand, Result<UpdateUserRoleResponse>> {
		public async Task<Result<UpdateUserRoleResponse>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken) {
			var user = await userRepository.GetUserById(request.UserId);
			var adminUser = await userRepository.GetUserById(request.AdminUserId);

			var validationResult = await validator.ValidateAsync(Validation.Context(request, ("User", user), ("AdminUser", adminUser)));

			if (!validationResult.IsValid) {
				return Result.Failure<UpdateUserRoleResponse>(ErrorMapper.Map(validationResult));
			}

			await userRepository.UpdateUserRole(user, adminUser, Role.Create(request.Role));

			return Result.Success(new UpdateUserRoleResponse(user.Id, user.Username, request.Role));
		}
	}

	[MutationType]
	public class UpdateUserRoleMutation {
		[RoleAuthorize(Constants.Role.Admin)]
		public async Task<Result<UpdateUserRoleResponse>> UpdateUserRoleAsync([Service] ISender sender, UpdateUserRoleCommand input) {
			return await sender.Send(input);
		}
	}
}