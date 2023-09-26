using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents DateTime-typed argument for command parameters
    /// </summary>
    public class DateTimeArgument : Argument
    {
        public DateTimeArgument(Action<DateTime> valueProvider) : base(valueProvider)
        {
        }

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
        ///     Argument value. Is subject to validation for period constraints (if any).
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
        ///     Provides argument value to command parameters.
        /// </summary>
        internal override void ProvideValue()
            => ((Action<DateTime>)_valueProvider).Invoke(Value);

        /// <summary>
        ///     Validates parsed argument value for start and end dates if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {
            if (StartDate is not null && Value < StartDate)
                throw new ArgumentException($"Value passed for <{Name}> argument should be greater or equal than {StartDate}. Value passed is {Value}.");

            if (EndDate is not null && Value > EndDate)
                throw new ArgumentException($"Value passed for <{Name}> argument should be less or equal than {EndDate}. Value passed is {Value}.");
        }
    }
}
