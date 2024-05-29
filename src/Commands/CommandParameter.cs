using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents parameter of command. This could be command argument or command option (starts with hyphens prefix).
    /// </summary>
    public abstract class CommandParameter : ISearchableUnit
    {
        private protected readonly Delegate _valueProvider;

        public CommandParameter(Delegate valueProvider, bool isOptional)
        {
            _valueProvider = valueProvider;
            IsOptional= isOptional;
            if (IsOptional)
            {
                OptCounter++;
            }         
            else
            {
                ArgCounter++;
                Position = ArgCounter;
            }            
        }

        /// <summary>
        ///     Argument counter.
        /// </summary>
        internal static int ArgCounter { get; private set; } = 0;

        /// <summary>
        ///     Option counter.
        /// </summary>
        internal static int OptCounter { get; private set; } = 0;

        /// <summary>
        ///     Defines if command paramter is optional. False for command argument, true for command option.
        /// </summary>
        public bool IsOptional { get; internal set; }

        /// <summary>
        ///     Identifies whether the parameter is hidden. Applied to option paramteres only. Hidden options do not appear when help is used.
        /// </summary>
        public bool IsHidden { get; internal set; }

        /// <summary>
        ///     Name of command paramter.
        /// </summary>
        public abstract string Name { get; internal set; }

        /// <summary>
        ///     Command parameter alias.
        /// </summary>
        public abstract string? Alias { get; internal set; }

        /// <summary>
        ///     Description of command paramter.
        /// </summary>
        public abstract string? Description { get; internal set; }

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
        ///     Accepts parser (visitor) to get parsed value.
        /// </summary>
        /// <param name="parser"></param>
        internal abstract void AcceptParser(Parser parser);

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
        internal static void ResetCounters() 
            => ArgCounter = OptCounter = 0; 
    }
}
