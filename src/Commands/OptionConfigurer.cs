
namespace SmartCLI.Commands
{
    public abstract class OptionConfigurer<TConfigurer, TOpt> : CommandParameterConfigurer<TConfigurer, TOpt>
        where TConfigurer : OptionConfigurer<TConfigurer, TOpt>
        where TOpt : CommandParameter
    {
        public OptionConfigurer(TOpt opt) : base(opt)         
            => _param.Name = $"--opt{CommandParameter.OptCounter}";
        

        public TConfigurer WithAlias(string alias)
        {
            _param.Alias = alias;
            return _configurer;
        }

        public TConfigurer IsHidden(bool hidden)
        {
            _param.IsHidden = hidden;
            return _configurer;
        }
    }
}
