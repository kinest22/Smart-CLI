using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents numeric-typed argumeny for command parameters (int, float, etc.)
    /// </summary>
    /// <typeparam name="TArg">Numeric type.</typeparam>
    public class NumericArgument<TArg> : Argument where TArg : INumber<TArg>
    {
        public NumericArgument(Action<TArg> valueProvider) : base(valueProvider)
        {
        }

        /// <summary>
        ///     Name of argument.
        /// </summary>
        public override string? Name { get; internal set; }

        /// <summary>
        ///     Description of argument.
        /// </summary>
        public override string? Description { get; set; }

        /// <summary>
        ///     Position of argument in command line.
        /// </summary>
        public override int Position { get; internal set; }
        
        /// <summary>
        ///     Argument value. Is subject to validation for min, max or allowed values constraints (if any).
        /// </summary>
        public TArg? Value { get; set; }

        /// <summary>
        ///     Minimal value allowed.
        /// </summary>
        public TArg[]? MinValue { get; set; }

        /// <summary>
        ///     Maximum value allowed.
        /// </summary>
        public TArg[]? MaxValue { get; set; }

        /// <summary>
        ///     Set of values allowed.
        /// </summary>
        public TArg[]? AllowedValues { get; set; }

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

            Value = TArg.TryParse(strval, nstl, fmt, out TArg? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TArg).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Provides argument value to command parameters.
        /// </summary>
        internal override void ProvideValue()
            => ((Action<TArg>)_valueProvider).Invoke(Value!);

        /// <summary>
        ///     Resets argument value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<TArg>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Validates parsed argument value for min, max and allowed values if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {
            if (MinValue is not null && Value! < MinValue[0])
                throw new ArgumentException($"Value passed for <{Name}> argument should be greater or equal than {MinValue[0]}. Value passed is {Value!}.");

            if (MaxValue is not null && Value! > MaxValue[0])
                throw new ArgumentException($"Value passed for <{Name}> argument should be less or equal than {MaxValue[0]}. Value passed is {Value!}.");

            if (AllowedValues is not null && !AllowedValues.Contains(Value!))
            {
                string allowedVals = string.Empty;
                foreach (var val in AllowedValues)                
                    allowedVals += val.ToString() + ", ";               
                throw new ArgumentException($"Value passed for <{Name}> argument should belong to the set: {{{allowedVals[..^2]}}}. Value passed is {Value!}.");
            }
        }
    }
}
