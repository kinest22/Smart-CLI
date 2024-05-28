using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents DateTime-typed parameter of command.
    /// </summary>
    public class DateTimeParameter : CommandParameter
    {
        public DateTimeParameter(Action<DateTime> valueProvider, bool isOptional) : base(valueProvider, isOptional)
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
        ///     Parameter value. Is subject to validation for period constraints (if any).
        /// </summary>
        public DateTime Value { get; set; }

        /// <summary>
        ///     Minimal start date allowed.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Maximum start date allowed.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            Value = DateTime.TryParse(strval, fmt, out DateTime parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(DateTime).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Provides parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()
            => ((Action<DateTime>)_valueProvider).Invoke(Value);

        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<DateTime>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Validates parsed parameter value for start and end dates if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {
            if (StartDate is not null && Value < StartDate)
                throw new ArgumentException($"Value passed for <{Name}> parameter should be greater or equal than {StartDate}. Value passed is {Value}.");

            if (EndDate is not null && Value > EndDate)
                throw new ArgumentException($"Value passed for <{Name}> parameter should be less or equal than {EndDate}. Value passed is {Value}.");
        }
    }
}
