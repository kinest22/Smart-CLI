using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    public class StringArgumentConfigurer : ArgumentConfigurer<StringArgumentConfigurer, StringArgument>
    {
        public StringArgumentConfigurer(Action valueProvider) : base(new StringArgument(valueProvider))
            => _configurer = this;

        public StringArgumentConfigurer WithMinLength(int length)
        {
            
            return this;
        }
    }
}
