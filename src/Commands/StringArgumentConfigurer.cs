using System;
using System.Collections.Generic;

namespace SmartCLI.Commands
{
    public class StringArgumentConfigurer : ArgumentConfigurer<StringArgumentConfigurer, StringArgument>
    {
        public StringArgumentConfigurer(Action<string> valueProvider) : base(new StringArgument((Action<ICollection<char>>)valueProvider))
            => _configurer = this; // explicit cast is ok??

        public StringArgumentConfigurer WithMinLength(int length)
        {
            
            return this;
        }
    }
}
