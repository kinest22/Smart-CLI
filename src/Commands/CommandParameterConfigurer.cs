
namespace SmartCLI.Commands
{
    /// <summary>
    ///     Command parameter fluent configurer.
    /// </summary>
    /// <typeparam name="TConfigurer">Configurer type.</typeparam>
    /// <typeparam name="TParameter">Parameter type.</typeparam>
    public abstract class CommandParameterConfigurer<TConfigurer, TParameter> 
        where TConfigurer : CommandParameterConfigurer<TConfigurer, TParameter>
        where TParameter : CommandParameter
    {
        private protected readonly TParameter _param;
        private protected TConfigurer _configurer = null!;

        public CommandParameterConfigurer(TParameter param)
            => _param = param;

        /// <summary>
        ///     Sets the name for the command parameter.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithName(string name)
        {
            _param.Name = name.ToUpper();
            return _configurer;
        }

        /// <summary>
        ///     Sets description for the command parameter.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithDescription(string description)
        {
            _param.Description = description;
            return _configurer;
        }        

        /// <summary>
        ///     Returns command parameter configured.
        /// </summary>
        internal CommandParameter GetParameter()
            => _param;
    }
}
