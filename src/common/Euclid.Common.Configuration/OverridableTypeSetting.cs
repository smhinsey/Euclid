using System;

namespace Euclid.Common.Configuration
{
	public class OverridableTypeSetting<TImplements> : IOverridableSetting<Type>
	{
		private Type _value;

		public OverridableTypeSetting(string name)
		{
			this.Name = name;
		}

		public Type DefaultValue { get; private set; }

		public string Name { get; private set; }

		public Type Value
		{
			get
			{
				return this._value;
			}

			private set
			{
				if (!typeof(TImplements).IsAssignableFrom(value))
				{
					throw new InvalidTypeSettingException(this.Name, typeof(TImplements), value);
				}

				this._value = value;
			}
		}

		public bool WasOverridden { get; private set; }

		public void ApplyOverride(Type newValue)
		{
			this.Value = newValue;
			this.WasOverridden = true;
		}

		public void WithDefault(Type value)
		{
			this.Value = value;
			this.DefaultValue = this.Value;
		}
	}
}