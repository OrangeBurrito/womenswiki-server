using HotChocolate.Authorization;

namespace WomensWiki.Common.Attributes;

public class RoleAuthorizeAttribute : AuthorizeAttribute {
	public RoleAuthorizeAttribute(Constants.Role roleEnum) {
		Roles = roleEnum.ToString()
			.Split(',', StringSplitOptions.TrimEntries)
			.ToArray();
	}
}