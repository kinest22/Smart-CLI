using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents class-typed argument for command parameters.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    public class ClassArgument<TArg> : Argument where TArg : class, IParsable<TArg>
    {
        public ClassArgument(Action<TArg> valueProvider) : base(valueProvider)
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
        ///     Argumnet position.
        /// </summary>
        public override int Position { get; set; }
        
        /// <summary>
        ///     Argumnet value.
        /// </summary>
        public TArg? Value { get; set; }

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string.
        /// </summary>
        /// <param name="strval"></param>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            Value = TArg.TryParse(strval, fmt, out TArg? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TArg).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Provides argument value to command parameters.
        /// </summary>
        internal override void ProvideValue()        
            => ((Action<TArg>)_valueProvider).Invoke(Value!);

        /// <summary>
        ///     Validates parsed argument value for constraints.
        /// </summary>
        internal override void Validate()
        {
        }
    }
}
