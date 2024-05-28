using System;

namespace SmartCLI.Commands
{
    public class EnumParameter<TEnum> : CommandParameter where TEnum : struct, Enum
    {
        public EnumParameter(Action<TEnum> valueProvider) : base(valueProvider, true)
        {
            Name = string.Empty;
        }

        /// <summary>
        ///     Name of parameter.
        /// </summary>
        public override string Name { get; internal set; }

        /// <summary>
        ///     Parameter alias (for option case only)
        /// </summary>
        public override string? Alias { get; internal set; }

        /// <summary>
        ///     Description of parameter.
        /// </summary>
        public override string? Description { get; set; }

        /// <summary>
        ///     Position of parameter in command line.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Ignore case flag for enum parameters. True to ignore case, false to consider case.
        /// </summary>
        public bool IgnoreCase { get; internal set; } = true;

        /// <summary>
        ///     Parameter value. Is subject to validation for min, max or allowed values constraints (if any).
        /// </summary>
        public TEnum? Value { get; set; }

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        { 
            Value = Enum.TryParse<TEnum>(strval, IgnoreCase, out var parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TEnum).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Provides parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()
            => ((Action<TEnum>)_valueProvider).Invoke(Value!.Value);


        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<TEnum>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Validates parsed parameter value for constraints.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {            
        }
    }
}
