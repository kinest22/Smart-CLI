using System;
using System.Globalization;
using System.Numerics;

namespace SmartCLI.Commands
{
    public class NumericArgument<TArg> : Argument where TArg : INumber<TArg>
    {
        public override string? Name { get; set; }
        public override string? Description { get; set; }
        public override int Position { get; set; }
        TArg? Value { get; set; }

        public override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            var nstl = NumberStyle is null
                ? NumberStyles.None
                : NumberStyle.Value; 

            Value = TArg.TryParse(strval, nstl, fmt, out TArg? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TArg).Name}.")
                : parsed;
        }
    }
}
