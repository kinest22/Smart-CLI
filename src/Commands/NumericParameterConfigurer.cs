﻿using System;
using System.Globalization;
using System.Numerics;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for numeric-typed parameter.
    /// </summary>
    /// <typeparam name="TParam">Numeric type.</typeparam>
    public class NumericParameterConfigurer<TParam> : CommandParameterConfigurer<NumericParameterConfigurer<TParam>, NumericParameter<TParam>>
        where TParam : INumber<TParam>        
    {
        public NumericParameterConfigurer(Action<TParam> valueProvider) : base(new NumericParameter<TParam>(valueProvider))                 
            => _configurer = this;

        /// <summary>
        ///     Sets parameter's minimum value.
        /// </summary>
        /// <returns><see cref="NumericParameterConfigurer{TArg}"/></returns>
        public NumericParameterConfigurer<TParam> WithMinValue(TParam minValue)
        {
            _param.MinValue = new TParam[1] { minValue };
            return this;
        }

        /// <summary>
        ///     Sets parameter's maximum value.
        /// </summary>
        /// <returns><see cref="NumericParameterConfigurer{TArg}"/></returns>
        public NumericParameterConfigurer<TParam> WithMaxValue(TParam maxValue)
        {
            _param.MaxValue = new TParam[1] { maxValue };
            return this;
        }

        /// <summary>
        ///     Sets parameter's allowed values.
        /// </summary>
        /// <returns><see cref="NumericParameterConfigurer{TArg}"/></returns>
        public NumericParameterConfigurer<TParam> WithAllowedValues(params TParam[] allowedValues)
        {
            _param.AllowedValues = allowedValues;
            return this;
        }

        /// <summary>
        ///     Sets format provider to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericParameterConfigurer{TArg}"/></returns>
        public NumericParameterConfigurer<TParam> WithFormatProvider(IFormatProvider formatProvider)
        {
            _param.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        ///     Sets number style to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericParameterConfigurer{TArg}"/></returns>
        public NumericParameterConfigurer<TParam> WithNumberStyle(NumberStyles numberStyle)
        {
            _param.NumberStyle = numberStyle;
            return this;
        }
    }
}
