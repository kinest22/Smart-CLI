using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents argument that belongs to parameters of command.
    /// </summary>
    public abstract class Argument
    {
        /// <summary>
        ///     Name of argument.
        /// </summary>
        public abstract string? Name { get; set; }

        /// <summary>
        ///     description of argument.
        /// </summary>
        public abstract string? Description { get; set; }

        /// <summary>
        ///     Position of argument in command line.
        /// </summary>
        public abstract int Position { get; set; }

        /// <summary>
        ///     Format used to parse value types arguments.
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
        public abstract void Parse(string strval);
    }
}
