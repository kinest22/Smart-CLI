
using System;

namespace SmartCLI.Commands
{
    public class StringArgument : CollectionArgument<char>
    {
        public StringArgument(Action valueProvider) : base(valueProvider)
        {
        }


    }
}
