using System;
using System.Numerics;

namespace SmartCLI.Commands
{
    public class NumericOptionConfigurer<TOpt> : OptionConfigurer<NumericOptionConfigurer<TOpt>, NumericParameter<TOpt>>
        where TOpt : INumber<TOpt>
    {
        public NumericOptionConfigurer(Action<TOpt> valueProvider, bool isOptional) 
            : base(new NumericParameter<TOpt>(valueProvider, isOptional))
            => _configurer = this;       
    }
}
