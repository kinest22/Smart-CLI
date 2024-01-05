using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents parameter of command. This could be command argument or command option (starts with hyphens prefix).
    /// </summary>
    public abstract class CommandParameter
    {
        private static int _counter = 0;
        private protected readonly Delegate _valueProvider;

        public CommandParameter(Delegate valueProvider) : this(valueProvider, false)
        {
        }

        public CommandParameter(Delegate valueProvider, bool isOptional)
        {
            Position = ++_counter;
            Name = $"ARG_{Position}";
            _valueProvider = valueProvider;
            IsOptional= isOptional;
        }

        /// <summary>
        ///     Defines if command paramter is optional. False for command argument, true for command option.
        /// </summary>
        public bool IsOptional { get; internal set; }

        /// <summary>
        ///     Name of command paramter.
        /// </summary>
        public abstract string? Name { get; internal set; }

        /// <summary>
        ///     Command parameter alias.
        /// </summary>
        public abstract string? Alias { get; internal set; }

        /// <summary>
        ///     Description of command paramter.
        /// </summary>
        public abstract string? Description { get; set; }

        /// <summary>
        ///     Position of command paramter in command line.
        /// </summary>
        public abstract int Position { get; internal set; }

        /// <summary>
        ///     Format used to parse value-type command paramters.
        /// </summary>
        public IFormatProvider? FormatProvider { get; internal set; }

        /// <summary>
        ///     Number style bitmask used to format numeric command paramters.
        /// </summary>
        public NumberStyles? NumberStyle { get; internal set; }

        /// <summary>
        ///     In derived class parses string-represented command paramter from command line.
        /// </summary>
        /// <param name="strval"></param>
        internal abstract void Parse(string strval);

        /// <summary>
        ///     In derived class validates parsed value subject to constraints (if any).
        /// </summary>
        internal abstract void Validate();

        /// <summary>
        ///     In derived class provides value to parameters member.
        /// </summary>
        internal abstract void ProvideValue();

        /// <summary>
        ///     In derived class resets command paramter value to deault.
        /// </summary>
        internal abstract void ResetValue();

        /// <summary>
        ///     Resets command paramters count. 
        /// </summary>
        internal static void ResetCounter() => _counter = 0; 
    }
}
