using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    public class CollectionOptionConfigurer<TParam> : OptionConfigurer<CollectionOptionConfigurer<TParam>, CollectionParameter<TParam>>
        where TParam : IParsable<TParam>
    {
        public CollectionOptionConfigurer(Action<ICollection<TParam>> valueProvider, bool isOptional)
            : base(new CollectionParameter<TParam>(valueProvider, isOptional))
            => _configurer = this;
    }
}
