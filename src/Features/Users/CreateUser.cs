using FluentValidation;
using MediatR;
using WomensWiki.Features.Users.Persistence;
using WomensWiki.src.Common;

namespace WomensWiki.src.Features.Users;

public class CreateUser {
	public record CreateUserCommand(string Username, string Email, string Password) : IRequest<Result<CreateUserResponse>>;
	public record CreateUserResponse(Guid Id, string Username, string Email);

	public class CreateUserValidator : AbstractValidator<CreateUserCommand> {
		public CreateUserValidator() {
			RuleFor(x => x.Username)
				.NotEmpty().WithMessage("Username is required.")
				.MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
				.MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
		}
	}

	internal sealed class CreateUserHandler(
		IUserRepository userRepository,
		AuthService authService,
		CreateUserValidator validator
	) {
		public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			var validationResult = await validator.ValidateAsync(request);
			if (!validationResult.IsValid) {
				return Result.Failure<CreateUserResponse>(ErrorMapper.Map(validationResult));
			}

			var user = await userRepository.CreateUser(request.Username, request.Email, authService.HashPassword(request.Password));
			await userRepository.SetRefreshToken(user, authService.GenerateRefreshToken(), DateTime.UtcNow.AddMinutes(5));

			return Result.Success(new CreateUserResponse(user.Id, user.Username, user.Email));
		}

	}
}