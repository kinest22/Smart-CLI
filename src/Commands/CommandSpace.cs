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
    public class CommandSpace : ICliUnit
    {
        private static int _spaceCounter = 1;
        private readonly List<Command> _commands;

        internal CommandSpace()
        {
            _commands = new List<Command>();
            _spaceCounter++;
        }

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
        public IReadOnlyList<Command> Commands => _commands;

        /// <summary>
        ///     Gets value that indicates whether <see cref="CommandSpace"/> is hidden or not.
        /// </summary>
        public bool IsHidden { get; internal set; }


        public IEnumerable<ICliUnit> SubUnits 
            => _commands;

        public bool IsParameter 
            => false;

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

        /// <summary>
        ///     Adds specified command to command space.
        /// </summary>
        /// <param name="cmd"></param>
        internal void AddCommand(Command cmd)
            => _commands.Add(cmd);
    }
}
