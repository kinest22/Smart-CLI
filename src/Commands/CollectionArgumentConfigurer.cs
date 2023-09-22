using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    internal class CollectionArgumentConfigurer<TArg> : ArgumentConfigurer<CollectionArgumentConfigurer<TArg>, CollectionArgument<TArg>>
        where TArg : IParsable<TArg>
    {
        public CollectionArgumentConfigurer(Action valueProvider) : base(new CollectionArgument<TArg>(valueProvider))
            => _configurer = this;
        
        
    }
}
