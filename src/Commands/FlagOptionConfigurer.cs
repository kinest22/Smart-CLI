using System;

namespace SmartCLI.Commands
{
    public class FlagOptionConfigurer : OptionConfigurer<FlagOptionConfigurer, BoolParameter>
    {
        public FlagOptionConfigurer(Action<bool> valueProvider) 
            : base(new BoolParameter(valueProvider))
            => _configurer = this;
    }
}
