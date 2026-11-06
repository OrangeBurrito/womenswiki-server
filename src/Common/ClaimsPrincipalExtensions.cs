using System.Security.Claims;

namespace WomensWiki.Common;

public static class ClaimsPrincipalExtensions {
	public static Guid? FindUserId(this ClaimsPrincipal principal) {
		return Guid.TryParse(principal.FindFirstValue("sub"), out var id) 
		? id : null;
	}

	public static Guid UserId(this ClaimsPrincipal principal) {
		return principal.FindUserId() ?? throw new InvalidOperationException("Missing sub claim");
	}
}