using System;

namespace Euclid.Framework.Cqrs.Settings
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
			this._parent = parent;
		}

		public T Days(int d)
		{
			this._d = d;

			return this._parent;
		}

		public T Hours(int h)
		{
			this._h = h;

			return this._parent;
		}

		public T Milliseconds(int ms)
		{
			this._ms = ms;

			return this._parent;
		}

		public T Minutes(int m)
		{
			this._m = m;

			return this._parent;
		}

		public T Seconds(int s)
		{
			this._s = s;

			return this._parent;
		}

		public T TimeSpan(TimeSpan t)
		{
			this._ms = t.Milliseconds;
			this._d = t.Days;
			this._m = t.Minutes;
			this._s = t.Seconds;
			this._h = t.Hours;

			return this._parent;
		}

		public static explicit operator TimeSpan(TimeSpanConfiguration<T> config)
		{
			return new TimeSpan(config._d, config._h, config._m, config._s, config._ms);
		}
	}
}