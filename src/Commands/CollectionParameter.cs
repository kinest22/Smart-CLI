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
        public CollectionParameter(Action<ICollection<TParam>> valueProvider) : base(valueProvider)
        {
        }

        /// <summary>
        ///     Name of parameter.
        /// </summary>
        public override string? Name { get; internal set; }

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
        ///     Parses collection elements from specified string.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            string[] tokens = strval.Split(' ');
            List<TParam> values = new();
            foreach(string token in tokens)
            {
                var val = TParam.TryParse(token, fmt, out TParam? parsed) is false
                ? throw new FormatException($"Cannot parse collection element '{token}' as {typeof(TParam).Name}.")
                : parsed;

                if (Transformer is not null)
                    val = Transformer.Invoke(val);

                values.Add(val);
            }
            Value = values;
        }

        /// <summary>
        ///     Validates parsed parameter value for min, max and allowed values if they are specified.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        internal override void Validate()
        {
            if (MaxCapacity is not null && Value.Count > MaxCapacity)
                throw new ArgumentException($"Argument <{Name}> should contain no more than {MaxCapacity} elements, but it contains {Value.Count} elements.");


            if (Value.Count is not 0 && AllowedValues is not null)
            {
                foreach (TParam val in Value)
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
                foreach (TParam val in Value)
                    if (!Validator.Invoke(val))
                        throw new ArgumentException($"Value '{val}' does not meet user criteria.");
            }
        }

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
    }
}
