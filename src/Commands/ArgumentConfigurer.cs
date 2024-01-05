
namespace SmartCLI.Commands
{
    /// <summary>
    ///     
    /// </summary>
    /// <typeparam name="TConfigurer"></typeparam>
    /// <typeparam name="TArg"></typeparam>
    public abstract class ArgumentConfigurer<TConfigurer, TArg> : CommandParameterConfigurer<TConfigurer, TArg>
        where TConfigurer : ArgumentConfigurer<TConfigurer, TArg>
        where TArg : CommandParameter
    {
        public ArgumentConfigurer(TArg arg) : base(arg) { }

        /// <summary>
        ///     Sets position for the command parameter in command-line input.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithPosition(int position)
        {
            _param.Position = position;
            return _configurer;
        }
    }
}
