using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents class-typed parameter of command.
    /// </summary>
    /// <typeparam name="TParam">Parameter type.</typeparam>
    public class ClassParameter<TParam> : CommandParameter where TParam : class, IParsable<TParam>
    {
        public ClassParameter(Action<TParam> valueProvider) : base(valueProvider)
        {
        }

        /// <summary>
        ///     Parameter name.
        /// </summary>
        public override string? Name { get; internal set; }

        /// <summary>
        ///     Parameter description.
        /// </summary>
        public override string? Description { get; set; }

        /// <summary>
        ///     Parameter position.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Parameter value.
        /// </summary>
        public TParam? Value { get; set; }

        /// <summary>
        ///     Parses <see cref="Value"/> from specified string.
        /// </summary>
        /// <param name="strval">string value.</param>
        /// <exception cref="FormatException"></exception>
        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            Value = TParam.TryParse(strval, fmt, out TParam? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TParam).Name}.")
                : parsed;
        }

        /// <summary>
        ///     Provides parameter value to command parameters.
        /// </summary>
        internal override void ProvideValue()        
            => ((Action<TParam>)_valueProvider).Invoke(Value!);

        /// <summary>
        ///     Resets parameter value.
        /// </summary>
        internal override void ResetValue()
            => ((Action<TParam>)_valueProvider).Invoke(default!);

        /// <summary>
        ///     Validates parsed parameter value for constraints.
        /// </summary>
        internal override void Validate()
        {
        }
    }
}
