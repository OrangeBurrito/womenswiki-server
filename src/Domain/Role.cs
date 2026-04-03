using WomensWiki.Common;

namespace WomensWiki.Domain;

public class Role : Entity {
	public string Name { get; private set; } = null!;

	public static Role Create(Constants.Role role) {
		return new Role {
			Name = role.ToString().ToLowerInvariant()
		};
	}
}