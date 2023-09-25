using System;
using System.Globalization;
using System.Text;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents argument that belongs to parameters of command.
    /// </summary>
    public abstract class Argument
    {
        private protected readonly Delegate _valueProvider;

        public Argument(Delegate valueProvider)
            => _valueProvider = valueProvider;

        /// <summary>
        ///     Name of argument.
        /// </summary>
        public abstract string? Name { get; set; }

        /// <summary>
        ///     Description of argument.
        /// </summary>
        public abstract string? Description { get; set; }

        /// <summary>
        ///     Position of argument in command line.
        /// </summary>
        public abstract int Position { get; set; }

        /// <summary>
        ///     Format used to parse value-type arguments.
        /// </summary>
        public IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        ///     Number style bitmask used to format numeric arguments.
        /// </summary>
        public NumberStyles? NumberStyle { get; set; }

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
        ///     Provides value to parameters member.
        /// </summary>
        internal abstract void ProvideValue();
    }
}
