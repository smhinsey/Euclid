
d:\Projects\Euclid\platform>@git.exe %*
using System;
using System.Text;

namespace Euclid.Common.Configuration
{
	public class OverridableTypeSetting<TImplements> : IOverridableSetting<Type>
	{
		public OverridableTypeSetting(string name)
		{
			Name = name;
		}

		public Type DefaultValue { get; private set; }

		public string Name { get; private set; }

        public Type Value { get; private set; }

		public bool WasOverridden { get; private set; }

		public void ApplyOverride(Type newValue)
		{
			Value = newValue;
			WasOverridden = true;

		    Validate();
		}

		public void WithDefault(Type value)
		{
			Value = value;
			DefaultValue = Value;

		    Validate();
		}

        public virtual void Validate()
        {
            if (Value == null)
            {
                throw new NullSettingException(Name);
            }

            if (!typeof(TImplements).IsAssignableFrom(Value))
            {
                throw new InvalidTypeSettingException(Name, typeof(TImplements), Value);
            }
        }

        public virtual bool IsValid()
        {
            return (Value != null && typeof (TImplements).IsAssignableFrom(Value));
        }

        public virtual string GetInvalidReason()
        {
            if (!IsValid())
            {
                var message = new StringBuilder("The setting ");
                message.AppendFormat("'{0}'", Name);

                if (Value == null)
                {
                    message.AppendFormat(" is null");
                }
                else if (!typeof(TImplements).IsAssignableFrom(Value))
                {
                    message.AppendFormat(" does not implement {0}", typeof (TImplements).Name);
                }

                return message.ToString();
            }

            return string.Empty;
        }
	}
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
