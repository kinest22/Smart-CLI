using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for string-typed parameter.
    /// </summary>
    public class StringArgumentConfigurer : ArgumentConfigurer<StringArgumentConfigurer, StringParameter>
    {
        public StringArgumentConfigurer(Action<string> valueProvider) : base(new StringParameter(valueProvider))
            => _configurer = this;

        /// <summary>
        ///     Sets max length for parameter value;
        /// </summary>
        /// <param name="length">Maximum string length</param>
        /// <returns></returns>
        public StringArgumentConfigurer WithMaxLength(int length)
        {
            _param.MaxLength = length;
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
            _param.Pattern = pattern;
            _param.RegExOptions = regexOptions;
            return this;
        }
    }
}
