using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Command space fluent builder.
    /// </summary>
    public class CommandSpaceConfigurer
    {
        private readonly CommandSpace _cmdspace;

        public CommandSpaceConfigurer()
            => _cmdspace = new CommandSpace();

        /// <summary>
        ///     Specifies the name of command space.
        /// </summary>
        /// <returns></returns>
        public CommandSpaceConfigurer HasName(string name)
        {
            _cmdspace.Name = name;
            return this;
        }

        /// <summary>
        ///     Specifies the description of command space.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public CommandSpaceConfigurer HasDescription(string description)
        {
            _cmdspace.Description = description;
            return this;
        }

        /// <summary>
        ///     Configures new <see cref="Command"/> and adds it to command space.
        /// </summary>
        /// <param name="configAction">
        ///     Command space configuration action.
        /// </param>
        /// <returns>
        ///     <see cref="Command"/> to be configured.
        /// </returns>
        public Command NewCommand<TParams>(Action<CommandConfigurer<TParams>> configAction)
            where TParams : VoidParams, new()
        {
            var cmdConfigurer = new CommandConfigurer<TParams>();
            configAction.Invoke(cmdConfigurer);
            var cmd = cmdConfigurer.GetCommand();
            cmd.CommandSpace = _cmdspace;
            if (cmd.ParentCommand is null)
                _cmdspace.Commands.Add(cmd);
            return cmd;
        }


        /// <summary>
        ///     Returns configured <see cref="CommandSpace"/>.
        /// </summary>
        internal CommandSpace GetConfigured()
            => _cmdspace;
    }
}
