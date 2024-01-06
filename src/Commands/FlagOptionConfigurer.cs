using System;

namespace SmartCLI.Commands
{
    public class FlagOptionConfigurer : OptionConfigurer<FlagOptionConfigurer, BoolParameter>
    {
        public FlagOptionConfigurer(string name, Action<bool> valueProvider) 
            : base(name, new BoolParameter(valueProvider))
            => _configurer = this;


    }
}
