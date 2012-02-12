using System;
using System.Web;

namespace AdminComposite.Extensions
{
	public static class RequestExtensions
	{
		public static Guid GetLoggedInUserId(this HttpRequestBase request)
		{
			var id = Guid.Empty;
			var cookie = request.Cookies["OrganizationUserId"];

			if (cookie != null)
			{
				Guid.TryParse(cookie.Value, out id);
			}

			return id;
		}
	}
}