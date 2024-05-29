using SmartCLI.Commands;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

namespace SmartCLI
{
    internal class Parser
    {
        private static string _currentInput = string.Empty; // < -------- should be nullable ??

        private readonly string[] _tokens;

        private int _tokid;

        public Parser()
        {
            // change split method signature
            _tokens = _currentInput.Split();
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="input"></param>
        internal static void GetCurrentInput(string input)
        {
            _currentInput = input;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="TNumeric"></typeparam>
        /// <param name="param"></param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ParsingException"></exception>
        internal void SetNumericValue<TNumeric> (NumericParameter<TNumeric> param)
            where TNumeric : struct, INumber<TNumeric>
        {
            TNumeric num 
                = TNumeric.TryParse(_tokens[_tokid], param.NumberStyle ?? default, param.FormatProvider, out TNumeric parsed) is false
                ? throw new FormatException($"Cannot parse '{_tokens[_tokid]}' as {typeof(TNumeric).Name}.")
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

        /// <summary>
        ///     
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="ParsingException"></exception>
        internal void SetDateTimeValue(DateTimeParameter param)
        {
            var date = DateTime.TryParse(_tokens[_tokid], param.FormatProvider, out DateTime parsed) is false
                ? throw new ParsingException($"Cannot parse '{_tokens[_tokid]}' as {typeof(DateTime).Name}.")
                : parsed;

            if (param.StartDate is not null && date < param.StartDate)
                throw new ParsingException($"Value passed for <{param.Name}> parameter should be greater or equal than {param.StartDate}. Value passed is {param.Value}.");

            if (param.EndDate is not null && date > param.EndDate)
                throw new ParsingException($"Value passed for <{param.Name}> parameter should be less or equal than {param.EndDate}. Value passed is {param.Value}.");

            param.Value = date;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="param"></param>
        /// <exception cref="ParsingException"></exception>
        internal void SetEnumValue<TEnum>(EnumParameter<TEnum> param)
            where TEnum : struct, Enum
        {
            var enm = Enum.TryParse(_tokens[_tokid], param.IgnoreCase, out TEnum parsed) is false
                ? throw new ParsingException($"Cannot parse '{_tokens[_tokid]}' as {typeof(TEnum).Name}.")
                : parsed;
            param.Value = enm;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="ParsingException"></exception>
        internal void SetStringValue(StringParameter param)
        {
            if (param.MaxLength is not null && _tokens[_tokid].Length > param.MaxLength)
                throw new ParsingException($"{_tokens[_tokid]} exceeds max length of {param.MaxLength} symbols.");

            RegexOptions regopt = param.RegExOptions is null
                ? RegexOptions.None
                : param.RegExOptions.Value;

            if (param.Pattern is not null && Regex.IsMatch(_tokens[_tokid]!, param.Pattern, regopt) is false)
                throw new ParsingException($"Passed value {_tokens[_tokid]} does not math regular expression {param.Pattern}");
            
            param.Value = _tokens[_tokid];
        }

        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="param"></param>
        /// <exception cref="ParsingException"></exception>
        internal void SetClassValue<TClass>(ClassParameter<TClass> param)
            where TClass : class, IParsable<TClass>
        {
            var cls = TClass.TryParse(_tokens[_tokid], param.FormatProvider, out TClass? parsed) is false
                ? throw new FormatException($"Cannot parse '{_tokens[_tokid]}' as {typeof(TClass).Name}.")
                : parsed;

            param.Value = cls;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="param"></param>
        internal void SetBoolValue(BoolParameter param)
        {

        }

        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="param"></param>
        internal void SetCollectionValue<TItem>(CollectionParameter<TItem> param)
            where TItem : IParsable<TItem>
        {

        }
    }







    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message) { }
    }

}
