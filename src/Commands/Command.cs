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
        private readonly Func<VoidParams> _paramsInitializer;

        public Command(Func<VoidParams> paramsCreationAction)
        {
            _cmdCounter++;
            _paramsInitializer = paramsCreationAction;
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
        ///     Executes the <see cref="TargetRoutine"/> of the command.
        /// </summary>
        public void ExecuteSolely()
        {
            TargetRoutine?.Invoke(_paramsInitializer());
        }
    }
}
