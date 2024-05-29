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

        public override string? Description { get; internal set; }

        public override int Position { get; internal set; }

        public override string? Alias { get; internal set; }

        public bool Value { get; internal set; }

        internal override void AcceptParser(Parser parser)
        {
            parser.SetBoolValue(this);
        }

        internal override void ProvideValue()
            => ((Action<bool>)_valueProvider).Invoke(Value!);

        internal override void ResetValue()
            => ((Action<bool>)_valueProvider).Invoke(default!);
    }
}
