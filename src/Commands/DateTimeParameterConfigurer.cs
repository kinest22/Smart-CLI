using System;


namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for DateTime-typed parameter.
    /// </summary>
    public class DateTimeParameterConfigurer : CommandParameterConfigurer<DateTimeParameterConfigurer, DateTimeParameter>
    {
        public DateTimeParameterConfigurer(Action<DateTime> valueProvider) : base(new DateTimeParameter(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Sets parameter's start date.
        /// </summary>
        /// <returns><see cref="DateTimeParameterConfigurer"/></returns>
        public DateTimeParameterConfigurer WithStartDate(DateTime startDate)
        {
            _param.StartDate = startDate;
            return this;
        }

        /// <summary>
        ///     Sets parameter's end date.
        /// </summary>
        /// <returns><see cref="DateTimeParameterConfigurer"/></returns>
        public DateTimeParameterConfigurer WithEndDate(DateTime endDate)
        {
            _param.EndDate = endDate;
            return this;
        }
    }
}
