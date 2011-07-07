using System;

namespace Euclid.Framework.Cqrs.Configuration
{
	public class TimeSpanConfiguration<T>
	{
		private readonly T _parent;
		private int _d;
		private int _h;
		private int _m;
		private int _ms;
		private int _s;

		public TimeSpanConfiguration(T parent)
		{
			_parent = parent;
		}

		public T Days(int d)
		{
			_d = d;

			return _parent;
		}

		public T Hours(int h)
		{
			_h = h;

			return _parent;
		}

		public T Milliseconds(int ms)
		{
			_ms = ms;

			return _parent;
		}

		public T Minutes(int m)
		{
			_m = m;

			return _parent;
		}

		public T Seconds(int s)
		{
			_s = s;

			return _parent;
		}

		public T TimeSpan(TimeSpan t)
		{
			_ms = t.Milliseconds;
			_d = t.Days;
			_m = t.Minutes;
			_s = t.Seconds;
			_h = t.Hours;

			return _parent;
		}

		public static explicit operator TimeSpan(TimeSpanConfiguration<T> config)
		{
			return new TimeSpan(config._d, config._h, config._m, config._s, config._ms);
		}
	}
}