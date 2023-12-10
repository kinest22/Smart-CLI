
namespace SmartCLI.Commands
{
    /// <summary>
    ///     Command parameter fluent configurer.
    /// </summary>
    /// <typeparam name="TConfigurer">Configurer type.</typeparam>
    /// <typeparam name="TArgWrapper">Parameter type.</typeparam>
    public class CommandParameterConfigurer<TConfigurer, TArgWrapper> 
        where TConfigurer : CommandParameterConfigurer<TConfigurer, TArgWrapper>
        where TArgWrapper : CommandParameter
    {
        private protected readonly TArgWrapper _arg;
        private protected TConfigurer _configurer = null!;

        public CommandParameterConfigurer(TArgWrapper arg)
            => _arg = arg;

        /// <summary>
        ///     Sets the name for the command parameter.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithName(string name)
        {
            _arg.Name = name.ToUpper();
            return _configurer;
        }

        /// <summary>
        ///     Sets description for the command parameter.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithDescription(string description)
        {
            _arg.Description = description;
            return _configurer;
        }

        /// <summary>
        ///     Sets position for the command parameter in command-line input.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithPosition(int position)
        {
            _arg.Position = position;
            return _configurer;
        }

        /// <summary>
        ///     Returns command parameter configured.
        /// </summary>
        internal CommandParameter GetArgument()
            => _arg;
    }
}
