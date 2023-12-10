using System.Collections.Generic;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents base class for specific command parameters with necessary arguments and options.
    /// </summary>
    public class VoidParams 
    {
        private readonly List<Command> _commands = new List<Command>();
        /// <summary>
        ///     Collection of suitable commands that can use these parameters.
        /// </summary>
        public IReadOnlyList<Command> Commands { get => _commands; }
        
        /// <summary>
        ///     Adds specified command to the collection of suitable commands.
        /// </summary>
        /// <param name="cmd"></param>
        internal void AddCommand(Command cmd)
            => _commands.Add(cmd);
    }
}
