﻿using System;
using System.Text.RegularExpressions;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents string-typed parameter of command.
    /// </summary>
    public class StringParameter : CommandParameter
    {
        public StringParameter(Action<string> valueProvider) : base(valueProvider, false)
        {   
            Name = string.Empty;
        }

        /// <summary>
        ///     Parameter name.
        /// </summary>
        public override string Name { get; internal set; }

        /// <summary>
        ///     Parameter alias (for option case only)
        /// </summary>
        public override string? Alias { get; internal set; }

        /// <summary>
        ///     Parameter description.
        /// </summary>
        public override string? Description { get; internal set; }

        /// <summary>
        ///     Parameter position in command-line.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Parameter value;
        /// </summary>
        public string? Value { get; internal set; }

        /// <summary>
        ///     Parameter length max value.
        /// </summary>
        public int? MaxLength { get; internal set; }

        /// <summary>
        ///     Regular expression pattern. Used to validate passed parameter value for specific predefined pattern.
        /// </summary>
        public string? Pattern { get; internal set; }

        /// <summary>
        ///     Regular expression options used to find mathces for the <see cref="Value"/>. Used in pair with <see cref="Pattern"/>.
        /// </summary>
        public RegexOptions? RegExOptions { get; internal set; }

        internal override void AcceptParser(Parser parser)
        {
            parser.SetStringValue(this);
        }

        /// <summary>
        ///     Provides Parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()        
            => ((Action<string>)_valueProvider).Invoke(Value!);

        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<string>)_valueProvider).Invoke(default!);
    }
}
