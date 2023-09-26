
namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for argument.
    /// </summary>
    /// <typeparam name="TConfigurer">Configurer type.</typeparam>
    /// <typeparam name="TArgWrapper">Argument type.</typeparam>
    public class ArgumentConfigurer<TConfigurer, TArgWrapper> 
        where TConfigurer : ArgumentConfigurer<TConfigurer, TArgWrapper>
        where TArgWrapper : Argument
    {
        private protected readonly TArgWrapper _arg;
        private protected TConfigurer _configurer = null!;

        public ArgumentConfigurer(TArgWrapper arg)
            => _arg = arg;

        /// <summary>
        ///     Sets the name for the argument.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithName(string name)
        {
            _arg.Name = name;
            return _configurer;
        }

        /// <summary>
        ///     Sets description for the argument.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithDescription(string description)
        {
            _arg.Description = description;
            return _configurer;
        }

        /// <summary>
        ///     Sets position for the argument in command-line input.
        /// </summary>
        /// <returns><see cref="TConfigurer"/></returns>
        public TConfigurer WithPosition(int position)
        {
            _arg.Position = position;
            return _configurer;
        }

        public Argument GetArgument()
            => _arg;
    }
}
