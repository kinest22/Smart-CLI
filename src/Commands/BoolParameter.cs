using System;

namespace SmartCLI.Commands
{
    public class BoolParameter : CommandParameter
    {
        public BoolParameter(Action<bool> valueProvider) : base(valueProvider, true)
        {
            Name = string.Empty;
        }

        public override string Name { get; internal set; }

        public override string? Description { get; set; }

        public override int Position { get; internal set; }

        public override string? Alias { get; internal set; }

        public bool Value { get; set; }

        internal override void Parse(string strval)
        {
            Value = bool.TryParse(strval, out bool parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(bool).Name}.")
                : parsed;
        }

        internal override void ProvideValue()
            => ((Action<bool>)_valueProvider).Invoke(Value!);

        internal override void ResetValue()
            => ((Action<bool>)_valueProvider).Invoke(default!);

        internal override void Validate()
        {            
        }
    }
}
