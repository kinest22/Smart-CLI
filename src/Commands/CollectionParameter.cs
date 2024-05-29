using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents collection-typed parameter (array, list, etc.) of command. 
    /// </summary>
    /// <typeparam name="TParam">Collection element type.</typeparam>
    public class CollectionParameter<TParam> : CommandParameter, IEnumerable<TParam>
        where TParam : IParsable<TParam>
    {
        public CollectionParameter(Action<ICollection<TParam>> valueProvider, bool isOptional) : base(valueProvider, isOptional)
        {
            Name = string.Empty;
        }

        /// <summary>
        ///     Name of parameter.
        /// </summary>
        public override string Name { get; internal set; }

        /// <summary>
        ///     Parameter alias (for option case only)
        /// </summary>
        public override string? Alias { get; internal set; }

        /// <summary>
        ///     Description of parameter.
        /// </summary>
        public override string? Description { get; set; }

        /// <summary>
        ///     Position of parameter in command line.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Parameter value. Is subject to validation for max capacity or allowed values constraints (if any).
        /// </summary>
        public ICollection<TParam> Value { get; set; } = new List<TParam>(0);        

        /// <summary>
        ///     Max capacity of collection. 
        /// </summary>
        public int? MaxCapacity { get; set; }

        /// <summary>
        ///     Set of values allowed.
        /// </summary>
        public TParam[]? AllowedValues { get; set; }

        /// <summary>
        ///     Is used to validate constraint for each element in collection.
        /// </summary>
        public Predicate<TParam>? Validator { get; set; } 

        /// <summary>
        ///     Is used to transform values while parsing.
        /// </summary>
        public Func<TParam, TParam>? Transformer { get; set; }

        /// <summary>
        ///     Provides parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()        
            => ((Action<ICollection<TParam>>)_valueProvider).Invoke(Value);

        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<ICollection<TParam>>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TParam> GetEnumerator()
            => Value.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        internal override void AcceptParser(Parser parser)
        {
            parser.SetCollectionValue(this);
        }
    }
}
