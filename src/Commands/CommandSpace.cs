﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents logically grouped set of commands.
    /// </summary>
    public class CommandSpace
    {
        private static int _spaceCounter = 1;

        internal CommandSpace()
            => _spaceCounter++;

        /// <summary>
        ///     Command space name. Appears when help is used.
        /// </summary>
        public string Name { get; internal set; } = $"Command_space_{_spaceCounter}";
        /// <summary>
        ///     Command space description. Appears when help is used.
        /// </summary>
        public string Description { get; internal set; } = string.Empty;
        /// <summary>
        ///     Set of commands combined in command space.
        /// </summary>
        public List<Command> Commands { get; internal set; } = new();

        /// <summary>
        ///     Configures new <see cref="CommandSpace"/> using <see cref="CommandSpaceConfigurer"/>.
        /// </summary>
        /// <param name="configAction">
        ///     Command space configuration action.
        /// </param>
        /// <returns>
        ///     New instance of <see cref="CommandSpace"/>.
        /// </returns>
        public static CommandSpace ConfigureNew(Action<CommandSpaceConfigurer> configAction)
        {
            var configurer = new CommandSpaceConfigurer();
            configAction.Invoke(configurer);
            return configurer.GetConfigured();
        }
    }
}
