using System;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents numeric command parameters argument (int, float, etc.)
    /// </summary>
    /// <typeparam name="TArg">Numeric type.</typeparam>
    public class NumericArgument<TArg> : Argument where TArg : INumber<TArg>
    {
        /// <summary>
        ///     Name of argument.
        /// </summary>
        public override string? Name { get; set; }

        /// <summary>
        ///     Description of argument.
        /// </summary>
        public override string? Description { get; set; }

        /// <summary>
        ///     Position of argument in command line.
        /// </summary>
        public override int Position { get; set; }
        
        /// <summary>
        ///     Argument value. Is subject to validation for min, max or allowed values constraints (if any).
        /// </summary>
        public TArg? Value { get; set; }

        /// <summary>
        ///     Minimal value allowed.
        /// </summary>
        public TArg? MinValue { get; set; }

        /// <summary>
        ///     Maximum value allowed.
        /// </summary>
        public TArg? MaxValue { get; set; }

        /// <summary>
        ///     Set of values allowed.
        /// </summary>
        public TArg[]? AllowedValues { get; set; }

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            var nstl = NumberStyle is null
                ? NumberStyles.None
                : NumberStyle.Value; 

            Value = TArg.TryParse(strval, nstl, fmt, out TArg? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TArg).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Validates parsed argument value for min, max and allowed values if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public override void Validate()
        {
            if (MinValue is not null && Value! < MinValue)
                throw new ArgumentException($"Value passed for <{Name}> argument should be greater or equal than {MinValue}. Value passed is {Value!}.");

            if (MaxValue is not null && Value! > MaxValue)
                throw new ArgumentException($"Value passed for <{Name}> argument should be less or equal than {MaxValue}. Value passed is {Value!}.");

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
