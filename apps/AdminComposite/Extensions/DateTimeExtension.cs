using System;
using System.Data.SqlTypes;

namespace AdminComposite.Extensions
{
	public static class DateTimeExtension
	{
		public static string GetFriendlyRelativeTime(this DateTime date)
		{
			var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
			var delta = Math.Abs(ts.TotalSeconds);

			const int SECOND = 1;
			const int MINUTE = 60 * SECOND;
			const int HOUR = 60 * MINUTE;
			const int DAY = 24 * HOUR;
			const int MONTH = 30 * DAY;

			if (date == (DateTime)SqlDateTime.MinValue)
			{
				return "never";
			}
			if (delta < 1 * MINUTE)
			{
				return "moments ago";
			}
			if (delta < 2 * MINUTE)
			{
				return "a minute ago";
			}
			if (delta < 45 * MINUTE)
			{
				return ts.Minutes + " minutes ago";
			}
			if (delta < 90 * MINUTE)
			{
				return "an hour ago";
			}
			if (delta < 24 * HOUR)
			{
				return ts.Hours + " hours ago";
			}
			if (delta < 48 * HOUR)
			{
				return "yesterday";
			}
			if (delta < 30 * DAY)
			{
				return ts.Days + " days ago";
			}
			if (delta < 12 * MONTH)
			{
				int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
				return months <= 1 ? "one month ago" : months + " months ago";
			}
			else
			{
				int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
				return years <= 1 ? "one year ago" : years + " years ago";
			}
		}
	}
}