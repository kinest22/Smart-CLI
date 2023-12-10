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
    public class NumericParameter<TParam> : CommandParameter where TParam : INumber<TParam>
    {
        public NumericParameter(Action<TParam> valueProvider) : base(valueProvider)
        {
        }

        /// <summary>
        ///     Name of parameter.
        /// </summary>
        public override string? Name { get; internal set; }

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

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            var nstl = NumberStyle is null
                ? NumberStyles.Any
                : NumberStyle.Value; 

            Value = TParam.TryParse(strval, nstl, fmt, out TParam? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TParam).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Provides parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()
            => ((Action<TParam>)_valueProvider).Invoke(Value!);

        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<TParam>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Validates parsed parameter value for min, max and allowed values if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {
            if (MinValue is not null && Value! < MinValue[0])
                throw new ArgumentException($"Value passed for <{Name}> parameter should be greater or equal than {MinValue[0]}. Value passed is {Value!}.");

            if (MaxValue is not null && Value! > MaxValue[0])
                throw new ArgumentException($"Value passed for <{Name}> parameter should be less or equal than {MaxValue[0]}. Value passed is {Value!}.");

            if (AllowedValues is not null && !AllowedValues.Contains(Value!))
            {
                string allowedVals = string.Empty;
                foreach (var val in AllowedValues)                
                    allowedVals += val.ToString() + ", ";               
                throw new ArgumentException($"Value passed for <{Name}> parameter should belong to the set: {{{allowedVals[..^2]}}}. Value passed is {Value!}.");
            }
        }
    }
}
