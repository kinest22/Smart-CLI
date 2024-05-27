
using System;

namespace SmartCLI.Commands
{
    public class EnumOptionConfigurer<TEnum> : OptionConfigurer<EnumOptionConfigurer<TEnum>, EnumParameter<TEnum>>
        where TEnum : struct, Enum
    {
        public EnumOptionConfigurer(Action<TEnum> valueProvider) : base(new EnumParameter<TEnum>(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Forces to consider case when parsing string value.
        /// </summary>
        /// <returns><see cref="EnumOptionConfigurer{TEnum}"/></returns>
        public EnumOptionConfigurer<TEnum> ConsiderCaseForParsing()
        {
            _param.IgnoreCase = false;
            return this;
        }
    }
}
