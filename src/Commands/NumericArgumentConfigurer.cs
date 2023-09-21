using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for numeric-typed argument.
    /// </summary>
    /// <typeparam name="TArg">Numeric argument type.</typeparam>
    public class NumericArgumentConfigurer<TArg> : ArgumentConfigurer<NumericArgumentConfigurer<TArg>, NumericArgument<TArg>>
        where TArg : INumber<TArg>        
    {
        public NumericArgumentConfigurer() : base(new NumericArgument<TArg>())                 
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
    }
}
