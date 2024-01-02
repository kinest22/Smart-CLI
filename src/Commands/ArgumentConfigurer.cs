
namespace SmartCLI.Commands
{
    public class ArgumentConfigurer<TConfigurer, TArg> : CommandParameterConfigurer<TConfigurer, TArg>
        where TConfigurer : ArgumentConfigurer<TConfigurer, TArg>
        where TArg : CommandParameter
    {
        public ArgumentConfigurer(TArg arg) : base(arg) { }

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
