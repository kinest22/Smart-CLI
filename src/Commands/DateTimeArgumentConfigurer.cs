using System;


namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for DateTime-typed parameter.
    /// </summary>
    public class DateTimeArgumentConfigurer : ArgumentConfigurer<DateTimeArgumentConfigurer, DateTimeParameter>
    {
        public DateTimeArgumentConfigurer(Action<DateTime> valueProvider) : base(new DateTimeParameter(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Sets parameter's start date.
        /// </summary>
        /// <returns><see cref="DateTimeArgumentConfigurer"/></returns>
        public DateTimeArgumentConfigurer WithStartDate(DateTime startDate)
        {
            _param.StartDate = startDate;
            return this;
        }

        /// <summary>
        ///     Sets parameter's end date.
        /// </summary>
        /// <returns><see cref="DateTimeArgumentConfigurer"/></returns>
        public DateTimeArgumentConfigurer WithEndDate(DateTime endDate)
        {
            _param.EndDate = endDate;
            return this;
        }
    }
}
