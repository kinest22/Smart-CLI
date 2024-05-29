using System;
using System.Globalization;
using System.Numerics;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for numeric-typed parameter.
    /// </summary>
    /// <typeparam name="TArg">Numeric type.</typeparam>
    public class NumericArgumentConfigurer<TArg> : ArgumentConfigurer<NumericArgumentConfigurer<TArg>, NumericParameter<TArg>>
        where TArg : struct, INumber<TArg>        
    {
        public NumericArgumentConfigurer(Action<TArg> valueProvider, bool isOptional) 
            : base(new NumericParameter<TArg>(valueProvider, isOptional))                 
            => _configurer = this;

        /// <summary>
        ///     Sets parameter's minimum value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithMinValue(TArg minValue)
        {
            _param.MinValue = new TArg[1] { minValue };
            return this;
        }

        /// <summary>
        ///     Sets parameter's maximum value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithMaxValue(TArg maxValue)
        {
            _param.MaxValue = new TArg[1] { maxValue };
            return this;
        }

        /// <summary>
        ///     Sets parameter's allowed values.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithAllowedValues(params TArg[] allowedValues)
        {
            _param.AllowedValues = allowedValues;
            return this;
        }

        /// <summary>
        ///     Sets format provider to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithFormatProvider(IFormatProvider formatProvider)
        {
            _param.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        ///     Sets number style to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TArg}"/></returns>
        public NumericArgumentConfigurer<TArg> WithNumberStyle(NumberStyles numberStyle)
        {
            _param.NumberStyle = numberStyle;
            return this;
        }
    }
}
