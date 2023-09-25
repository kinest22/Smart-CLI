using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for string-typed argument.
    /// </summary>
    public class StringArgumentConfigurer : ArgumentConfigurer<StringArgumentConfigurer, StringArgument>
    {
        public StringArgumentConfigurer(Action<string> valueProvider) : base(new StringArgument(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Sets max length for argument value;
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public StringArgumentConfigurer WithMaxLength(int length)
        {
            _arg.MaxLength = length;
            return this;
        }

        /// <summary>
        ///     Sets regular expression for value to match.
        /// </summary>
        /// <param name="pattern">Regex pattern.</param>
        /// <param name="regexOptions">Regex options.</param>
        /// <returns></returns>
        public StringArgumentConfigurer WithRegex(string pattern, RegexOptions? regexOptions = null)
        {
            _arg.Pattern = pattern;
            _arg.RegExOptions = regexOptions;
            return this;
        }
    }
}
