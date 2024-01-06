using System;
using System.Globalization;
using System.Numerics;

namespace SmartCLI.Commands
{
    public class NumericOptionConfigurer<TOpt> : OptionConfigurer<NumericOptionConfigurer<TOpt>, NumericParameter<TOpt>>
        where TOpt : INumber<TOpt>
    {
        public NumericOptionConfigurer(string name, Action<TOpt> valueProvider) : base(name, new NumericParameter<TOpt>(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Sets parameter's minimum value.
        /// </summary>
        /// <returns><see cref="NumericOptionConfigurer{TOpt}"/></returns>
        public NumericOptionConfigurer<TOpt> WithMinValue(TOpt minValue)
        {
            _param.MinValue = new TOpt[1] { minValue };
            return this;
        }

        /// <summary>
        ///     Sets parameter's maximum value.
        /// </summary>
        /// <returns><see cref="NumericOptionConfigurer{TOpt}"/></returns>
        public NumericOptionConfigurer<TOpt> WithMaxValue(TOpt maxValue)
        {
            _param.MaxValue = new TOpt[1] { maxValue };
            return this;
        }

        /// <summary>
        ///     Sets parameter's allowed values.
        /// </summary>
        /// <returns><see cref="NumericArgumentConfigurer{TOpt}"/></returns>
        public NumericOptionConfigurer<TOpt> WithAllowedValues(params TOpt[] allowedValues)
        {
            _param.AllowedValues = allowedValues;
            return this;
        }

        /// <summary>
        ///     Sets format provider to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericOptionConfigurer{TOpt}"/></returns>
        public NumericOptionConfigurer<TOpt> WithFormatProvider(IFormatProvider formatProvider)
        {
            _param.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        ///     Sets number style to be used for parsing value.
        /// </summary>
        /// <returns><see cref="NumericOptionConfigurer{TOpt}"/></returns>
        public NumericOptionConfigurer<TOpt> WithNumberStyle(NumberStyles numberStyle)
        {
            _param.NumberStyle = numberStyle;
            return this;
        }
    }
}
