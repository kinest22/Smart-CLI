
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents string-typed argument for command parameters
    /// </summary>
    public class StringArgument : Argument
    {
        public StringArgument(Action<string> valueProvider) : base(valueProvider)
        {            
        }

        /// <summary>
        ///     Argument name.
        /// </summary>
        public override string? Name { get; set; }

        /// <summary>
        ///     Argument description.
        /// </summary>
        public override string? Description { get; set; }
        
        /// <summary>
        ///     Argument position in command-line.
        /// </summary>
        public override int Position { get; set; }

        /// <summary>
        ///     Argumnet value;
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        ///     Argument length max value.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        ///     Regular expression pattern. Used to validate passed argument value for specific predefined pattern.
        /// </summary>
        public string? Pattern { get; set; }

        /// <summary>
        ///     Regular expression options used to find mathces for the <see cref="Value"/>. Used in pair with <see cref="Pattern"/>.
        /// </summary>
        public RegexOptions? RegExOptions { get; set; }

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string 
        /// </summary>
        internal override void Parse(string strval)        
            => Value = strval;

        /// <summary>
        ///     Provides argument value to command parameters.
        /// </summary>
        internal override void ProvideValue()        
            => ((Action<string>)_valueProvider).Invoke(Value!);

        /// <summary>
        ///     Resets argument value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<string>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Validates parsed argument value for max length.
        /// </summary>
        internal override void Validate()
        {
            if (MaxLength is not null && Value!.Length > MaxLength)
                throw new ArgumentException($"{Value} exceeds max length of {MaxLength} symbols.");

            RegexOptions regopt = RegExOptions is null
                ? RegexOptions.None
                : RegExOptions.Value;

            if (Pattern is not null && Regex.IsMatch(Value!, Pattern, regopt) is false)
                throw new ArgumentException($"Passed value {Value} does not math regular expression {Pattern}");
        }
    }
}
