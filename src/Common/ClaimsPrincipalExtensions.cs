using System.Security.Claims;

namespace WomensWiki.Common;

public static class ClaimsPrincipalExtensions {
	public static Guid UserId(this ClaimsPrincipal principal) {
		return Guid.Parse(principal.FindFirstValue("sub")!);
	}
}