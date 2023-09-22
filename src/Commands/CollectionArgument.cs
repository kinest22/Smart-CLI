using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SmartCLI.Commands
{
    public class CollectionArgument<TArg> : Argument, IEnumerable<TArg>
        where TArg : IParsable<TArg>
    {
        public CollectionArgument(Action valueProvider) : base(valueProvider)
        {
        }

        public override string? Name { get; set; }
        public override string? Description { get; set; }
        public override int Position { get; set; }
        public List<TArg> Value { get; set; } = new List<TArg>(0);        
        public int? MaxCapacity { get; set; }


        internal override void Parse(string strval)
        {
            throw new NotImplementedException();
        }

        internal override void Validate()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TArg> GetEnumerator()
            => Value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
