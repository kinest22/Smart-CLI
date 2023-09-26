using System;


namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for DateTime-typed argument.
    /// </summary>
    /// <typeparam name="TArg">Numeric argument type.</typeparam>
    public class DateTimeArgumentConfigurer : ArgumentConfigurer<DateTimeArgumentConfigurer, DateTimeArgument>
    {
        public DateTimeArgumentConfigurer(Action<DateTime> valueProvider) : base(new DateTimeArgument(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Sets argument's start date.
        /// </summary>
        /// <returns><see cref="DateTimeArgumentConfigurer"/></returns>
        public DateTimeArgumentConfigurer WithStartDate(DateTime startDate)
        {
            _arg.StartDate = startDate;
            return this;
        }

        /// <summary>
        ///     Sets argument's end date.
        /// </summary>
        /// <returns><see cref="DateTimeArgumentConfigurer"/></returns>
        public DateTimeArgumentConfigurer WithEndDate(DateTime endDate)
        {
            _arg.EndDate = endDate;
            return this;
        }
    }
}
