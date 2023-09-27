using System;
using System.Collections.Generic;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents console command.
    /// </summary>
    public class Command
    {
        private static int _cmdCounter = 1;
        private readonly VoidParams _params;

        public Command(VoidParams @params)
        {
            _cmdCounter++;
            _params = @params;
            Arguments = new List<Argument>();
        }

        /// <summary>
        ///     Command name. Serves as a command unique identifier within command space.
        ///     Appears when help is used.
        /// </summary>
        public string Name { get; internal set; } = $"Command_{_cmdCounter}";

        /// <summary>
        ///     Command description. Appears when help is used.
        /// </summary>
        public string Description { get; internal set; } = string.Empty;

        /// <summary>
        ///     Identifies whether the command is hidden. Hidden commands do not appear when help is used.
        /// </summary>
        public bool IsHidden { get; internal set; }

        /// <summary>
        ///     Identifies whether the command requires any significant time to execute.
        /// </summary>
        public bool IsAwaitable { get; internal set; }

        /// <summary>
        ///     Mark displayed while the command is being executed. Specified only when <see cref="IsAwaitable"/> is true.
        /// </summary>
        public string? AwaitMark { get; internal set; }

        /// <summary>
        ///     Routine process to be processed when command is called.
        /// </summary>
        internal Action<VoidParams>? TargetRoutine { get; set; }

        /// <summary>
        ///     Command to which the current command is considered to be a child command.
        /// </summary>
        public Command? ParentCommand { get; set; }

        /// <summary>
        ///     Set of subcommands for the current command.
        /// </summary>
        public List<Command>? Subcommands { get; set; }

        /// <summary>
        ///     List of arguments of command.
        /// </summary>
        public List<Argument> Arguments { get; set; }

        /// <summary>
        ///     Parameters instance used by command as set of arguments.
        /// </summary>
        internal VoidParams Params => _params;

        /// <summary>
        ///     Executes the <see cref="TargetRoutine"/> of the command.
        /// </summary>
        public void Execute(string input)
        {
            char[] wschars = new char[] { ' ', '\t' };
            int argn = Arguments.Count;
            string[] tokens = input.Split(wschars, argn);
            
            for (int i = 0; i < tokens.Length; i++)
            {
                Arguments[i].Parse(tokens[i]);
                Arguments[i].Validate();
                Arguments[i].ProvideValue();
            }

            TargetRoutine?.Invoke(_params);

            for (int i = 0; i < tokens.Length; i++)
            {
                Arguments[i].ResetValue();                
            }
        }
    }
}
