using SmartCLI.Commands;
using System;
using System.Linq;
using System.Numerics;

namespace SmartCLI
{
    internal class Parser
    {
        private static string _currentInput = string.Empty; // < -------- should be nullable ??

        private readonly string[] _tokens;

        private int _tokindex;

        public Parser()
        {
            // change split method signature
            _tokens = _currentInput.Split();
        }

        internal static void GetCurrentInput(string input)
        {
            _currentInput = input;
        }

        internal void SetNumericValue<TNumeric> (NumericParameter<TNumeric> param)
            where TNumeric : INumber<TNumeric>
        {
            TNumeric num 
                = TNumeric.TryParse(_tokens[_tokindex], param.NumberStyle ?? default, param.FormatProvider, out TNumeric? parsed) is false
                ? throw new FormatException($"Cannot parse '{}' as {typeof(TNumeric).Name}.")
                : parsed;

            if (param.MinValue is not null && num < param.MinValue[0])
                throw new ParsingException($"Value passed for <{param.Name}> parameter should be greater or equal than {param.MinValue[0]}. Value passed is {num}.");

            if (param.MaxValue is not null && num > param.MaxValue[0])
                throw new ParsingException($"Value passed for <{param.Name}> parameter should be less or equal than {param.MaxValue[0]}. Value passed is {num}.");

            if (param.AllowedValues is not null && !param.AllowedValues.Contains(num))
            {
                string allowedVals = string.Join(", ", param.AllowedValues);
                throw new ParsingException($"Value passed for <{param.Name}> parameter should belong to the set: {allowedVals}. Value passed is {num}.");
            }

            param.Value = num;
        }

    }







    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message) { }
    }

}
