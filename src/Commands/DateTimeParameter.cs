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

        public DateTimeParameter(Action<DateTime?> valueProvider, bool isOptional) : base(valueProvider, isOptional)
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
        public override string? Description { get; internal set; }

        /// <summary>
        ///     Position of parameter in command line.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Parameter value. Is subject to validation for period constraints (if any).
        /// </summary>
        public DateTime Value { get; internal set; }

        /// <summary>
        ///     Minimal start date allowed.
        /// </summary>
        public DateTime? StartDate { get; internal set; }

        /// <summary>
        ///     Maximum start date allowed.
        /// </summary>
        public DateTime? EndDate { get; internal set; }

        internal override void AcceptParser(Parser parser)
        {
            parser.SetDateTimeValue(this);
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
            => ((Action<DateTime>)_valueProvider).Invoke(default);
    }
}
