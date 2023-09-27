using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents argument that belongs to parameters of command.
    /// </summary>
    public abstract class Argument
    {
        private static int _counter = 0;
        private protected readonly Delegate _valueProvider;

        public Argument(Delegate valueProvider)
        {
            Position = ++_counter;
            Name = $"arg_{Position}";
            _valueProvider = valueProvider;
        }

        /// <summary>
        ///     Name of argument.
        /// </summary>
        public abstract string? Name { get; internal set; }

        /// <summary>
        ///     Description of argument.
        /// </summary>
        public abstract string? Description { get; set; }

        /// <summary>
        ///     Position of argument in command line.
        /// </summary>
        public abstract int Position { get; internal set; }

        /// <summary>
        ///     Format used to parse value-type arguments.
        /// </summary>
        public IFormatProvider? FormatProvider { get; internal set; }

        /// <summary>
        ///     Number style bitmask used to format numeric arguments.
        /// </summary>
        public NumberStyles? NumberStyle { get; internal set; }

        /// <summary>
        ///     In derived class parse string-represented argument from command line to specified type.
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
        ///     In derived class resets argument value to deault.
        /// </summary>
        internal abstract void ResetValue();

        /// <summary>
        ///     Resets arguments count. 
        /// </summary>
        internal static void ResetCounter() => _counter = 0; 
    }
}
