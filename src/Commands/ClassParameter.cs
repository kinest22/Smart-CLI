using System;
using System.Globalization;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents class-typed parameter of command.
    /// </summary>
    /// <typeparam name="TParam">Parameter type.</typeparam>
    public class ClassParameter<TParam> : CommandParameter where TParam : class, IParsable<TParam>
    {
        public ClassParameter(Action<TParam> valueProvider) : base(valueProvider, false)
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
        public override string? Description { get; set; }

        /// <summary>
        ///     Parameter position.
        /// </summary>
        public override int Position { get; internal set; }

        /// <summary>
        ///     Parameter value.
        /// </summary>
        public TParam? Value { get; set; }

        internal override void AcceptParser(Parser parser)
        {
            parser.SetClassValue(this);
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
    }
}
