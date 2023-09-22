﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    public class ClassArgument<TArg> : Argument where TArg : IParsable<TArg>
    {
        public ClassArgument(Action valueProvider) : base(valueProvider)
        {
        }

        public override string? Name { get; set; }
        public override string? Description { get; set; }
        public override int Position { get; set; }
        public TArg? Value { get; set; }

        internal override void Parse(string strval)
        {
            var fmt = FormatProvider is null
                ? CultureInfo.InvariantCulture
                : FormatProvider;

            Value = TArg.TryParse(strval, fmt, out TArg? parsed) is false
                ? throw new FormatException($"Cannot parse '{strval}' as {typeof(TArg).Name}.")
                : parsed;
        }

        internal override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
