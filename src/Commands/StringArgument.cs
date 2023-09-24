
using System;
using System.Collections.Generic;

namespace SmartCLI.Commands
{
    public class StringArgument : CollectionArgument<char>
    {
        public StringArgument(Action<ICollection<char>> valueProvider) : base(valueProvider)
        {            
        }

    }
}
