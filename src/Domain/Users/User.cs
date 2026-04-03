using WomensWiki.Common;

namespace WomensWiki.Domain.Users;

public class User : Entity {
	public string Username { get; private set; } = null!;
	public string Email { get; private set; } = null!;
	public List<Role> Roles { get; private set; } = new();

	public static User Create(string username, string email) {
		return new User {
			Username = username,
			Email = email
		};
	}

	public void AddRole(Role role) {
		if (!Roles.Contains(role)) {
			Roles.Add(role);
		}
	}
}