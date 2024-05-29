using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents numeric-typed parameter of command (int, float, etc.)
    /// </summary>
    /// <typeparam name="TParam">Numeric type.</typeparam>
    public class NumericParameter<TParam> : CommandParameter 
        where TParam : struct, INumber<TParam>
    {
        public NumericParameter(Action<TParam?> valueProvider, bool isOptional) : base(valueProvider, isOptional)
        {
            Name = string.Empty;
        }

        public NumericParameter(Action<TParam> valueProvider, bool isOptional) : base(valueProvider, isOptional)
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
        ///     Parameter value. Is subject to validation for min, max or allowed values constraints (if any).
        /// </summary>
        public TParam? Value { get; set; }

        /// <summary>
        ///     Minimal value allowed.
        /// </summary>
        public TParam[]? MinValue { get; set; } // always contains only one item if not null

        /// <summary>
        ///     Maximum value allowed.
        /// </summary>
        public TParam[]? MaxValue { get; set; } // always contains only one item if not null

        /// <summary>
        ///     Set of values allowed.
        /// </summary>
        public TParam[]? AllowedValues { get; set; }

        internal override void AcceptParser(Parser parser)
        {
            parser.SetNumericValue(this);
        }

        /// <summary>
        ///     Provides parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()
            => ((Action<TParam>)_valueProvider).Invoke(Value ?? default);

        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<TParam>)_valueProvider).Invoke(default!);
    }
}
