using System;
using System.Globalization;
using System.Numerics;

namespace SmartCLI.Commands
{
    public class NumericOptionConfigurer<TOpt> : OptionConfigurer<NumericOptionConfigurer<TOpt>, NumericParameter<TOpt>>
        where TOpt : INumber<TOpt>
    {
        public NumericOptionConfigurer(Action<TOpt> valueProvider) : base(new NumericParameter<TOpt>(valueProvider))
            => _configurer = this;       
    }
}
