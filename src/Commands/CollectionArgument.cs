using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents collection of command parameters arguments (array, list, etc.)
    /// </summary>
    /// <typeparam name="TArg">Collection element type.</typeparam>
    public class CollectionArgument<TArg> : Argument, IEnumerable<TArg>
        where TArg : IParsable<TArg>
    {
        public CollectionArgument(Action<ICollection<TArg>> valueProvider) : base(valueProvider)
        {
        }

        /// <summary>
        ///     Name of argument.
        /// </summary>
        public override string? Name { get; internal set; }

        /// <summary>
        ///     Description of argument.
        /// </summary>
        public override string? Description { get; set; }

        /// <summary>
        ///     Position of argument in command line.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Argument value. Is subject to validation for max capacity or allowed values constraints (if any).
        /// </summary>
        public ICollection<TArg> Value { get; set; } = new List<TArg>(0);        

        /// <summary>
        ///     Max capacity of collection. 
        /// </summary>
        public int? MaxCapacity { get; set; }

        /// <summary>
        ///     Set of values allowed.
        /// </summary>
        public TArg[]? AllowedValues { get; set; }

        /// <summary>
        ///     Is used to validate constraint for each element in collection.
        /// </summary>
        public Predicate<TArg>? Validator { get; set; } 

        /// <summary>
        ///     Is used to transform values while parsing.
        /// </summary>
        public Func<TArg, TArg>? Transformer { get; set; }

        /// <summary>
        ///     Parses collection elements from specified string.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            string[] tokens = strval.Split(' ');
            List<TArg> values = new();
            foreach(string token in tokens)
            {
                var val = TArg.TryParse(token, fmt, out TArg? parsed) is false
                ? throw new FormatException($"Cannot parse collection element '{token}' as {typeof(TArg).Name}.")
                : parsed;

                if (Transformer is not null)
                    val = Transformer.Invoke(val);

                values.Add(val);
            }
            Value = values;
        }

        /// <summary>
        ///     Validates parsed argument value for min, max and allowed values if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {
            if (MaxCapacity is not null && Value.Count > MaxCapacity)
                throw new ArgumentException($"Argument <{Name}> should contain no more than {MaxCapacity} elements, but it contains {Value.Count} elements.");


            if (Value.Count is not 0 && AllowedValues is not null)
            {
                foreach (TArg val in Value)
                {
                    if (!AllowedValues.Contains(val))
                    {
                        string allowedVals = string.Empty;
                        foreach (var alval in AllowedValues)
                            allowedVals += alval.ToString() + ", ";
                        throw new ArgumentException($"Values passed for <{Name}> argument should belong to the set: {{{allowedVals[..^2]}}}. Value passed is '{val}'.");
                    }                    
                }
            }

            if (Validator is not null)
            {
                foreach (TArg val in Value)
                    if (!Validator.Invoke(val))
                        throw new ArgumentException($"Value '{val}' does not meet user criteria.");
            }
        }

        /// <summary>
        ///     Provides argument value to command parameters.
        /// </summary>
        internal override void ProvideValue()        
            => ((Action<ICollection<TArg>>)_valueProvider).Invoke(Value);

        /// <summary>
        ///     Resets argument value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<ICollection<TArg>>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TArg> GetEnumerator()
            => Value.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
