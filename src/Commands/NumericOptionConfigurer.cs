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
    }
}
