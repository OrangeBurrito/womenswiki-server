namespace WomensWiki.Common;

public static class Constants {
	[Flags]
	[GraphQLName("RoleType")]
	public enum Role {
		User = 1,
		Editor = 2,
		Admin = 4,
	}
}
