using System;

namespace Euclid.Common.Configuration
{
    public class OverridableTypeSetting<TImplements> : IOverridableSetting<Type>
    {
        public OverridableTypeSetting(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        private Type _value;

        public Type Value
        {
            get { return _value; }
            private set
            {
                if (!typeof (TImplements).IsAssignableFrom(value))
                {
                    throw new InvalidTypeSettingException(Name, typeof(TImplements), value);
                }
                _value = value;
            }
        }

        public Type DefaultValue { get; private set; }
        public bool WasOverridden { get; private set; }

        public void ApplyOverride(Type newValue)
        {
            Value = newValue;
            WasOverridden = true;
        }

        public void WithDefault(Type value)
        {
            Value = value;
            DefaultValue = Value;
        }
    }
}