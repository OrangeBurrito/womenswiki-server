using FluentValidation;
using MediatR;
using WomensWiki.Infrastructure.Services;
using WomensWiki.Common;
using WomensWiki.Domain.Users;
using WomensWiki.Features.Users.Persistence;

namespace WomensWiki.src.Features.Users;

public class CreateUser {
	public record CreateUserCommand(string Username, string Email, string Password) : IRequest<Result<CreateUserResponse>>;
	public record CreateUserResponse(Guid Id, string Username, string Email);

	public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand> {
		public CreateUserValidator() {
			RuleFor(x => x.Username)
				.UniqueUsername()
				.NotEmpty().WithMessage("Username is required.")
				.MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
				.MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

			RuleFor(x => x.Email)
				.UniqueEmail()
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
		}
	}

	internal sealed class CreateUserHandler(
		IUserRepository userRepository,
		CreateUserValidator validator,
		IdentityProviderService identityProviderService
	) : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>> {
		public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			var duplicateUser = await userRepository.GetUserByUsername(request.Username);
			var duplicateEmailUser = await userRepository.GetUserByEmail(request.Email);

			var validationResult = await validator.ValidateAsync(Validation.Context(request, ("User", duplicateUser), ("Email", duplicateEmailUser)));
			if (!validationResult.IsValid) {
				return Result.Failure<CreateUserResponse>(ErrorMapper.Map(validationResult));
			}

			var remoteUserExists = await identityProviderService.RemoteUserExistsAsync(request.Username, request.Email);
			if (remoteUserExists.IsFailure) {
				return Result.Failure<CreateUserResponse>(remoteUserExists.Errors!);
			}
			if (remoteUserExists.Data) {
				return Result.Failure<CreateUserResponse>([new Error("User.Duplicate", "A user with this username or email already exists in Keycloak.")]);
			}

			var user = await userRepository.CreateUser(request.Username, request.Email);

			var remoteUser = await identityProviderService.CreateRemoteUserAsync(user.Id, user.CreatedAt, request.Username, request.Email, request.Password);
			if (remoteUser.IsFailure) {
				await userRepository.DeleteNewUser(user);
				return Result.Failure<CreateUserResponse>(remoteUser.Errors!);
			}

			return Result.Success(new CreateUserResponse(user.Id, user.Username, user.Email));
		}
	}

	[MutationType]
	public class CreateUserMutation {
		public async Task<Result<CreateUserResponse>> CreateUserAsync([Service] ISender sender, CreateUserCommand input) {
			return await sender.Send(input);
		}
	}
}