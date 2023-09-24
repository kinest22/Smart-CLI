using System;
using System.Globalization;
using System.Numerics;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for numeric-typed argument.
    /// </summary>
    /// <typeparam name="TArg">Numeric argument type.</typeparam>
    public class NumericArgumentConfigurer<TArg> : ArgumentConfigurer<NumericArgumentConfigurer<TArg>, NumericArgument<TArg>>
        where TArg : INumber<TArg>        
    {
        public NumericArgumentConfigurer(Action<TArg> valueProvider) : base(new NumericArgument<TArg>(valueProvider))                 
            => _configurer = this;

        /// <summary>
        ///     Sets argument's minimum value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithMinValue(TArg minValue)
        {
            _arg.MinValue = minValue;
            return this;
        }

        /// <summary>
        ///     Sets argument's maximum value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithMaxValue(TArg maxValue)
        {
            _arg.MaxValue = maxValue;
            return this;
        }

        /// <summary>
        ///     Sets argument's allowed values.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithAllowedValues(params TArg[] allowedValues)
        {
            _arg.AllowedValues = allowedValues;
            return this;
        }

        /// <summary>
        ///     Sets format provider to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithFormatProvider(IFormatProvider formatProvider)
        {
            _arg.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        ///     Sets number style to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithNumberStyle(NumberStyles numberStyle)
        {
            _arg.NumberStyle = numberStyle;
            return this;
        }
    }
}
