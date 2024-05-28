using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents console command.
    /// </summary>
    public class Command : ISearchableUnit
    {
        private static int _cmdCounter = 1;
        private readonly VoidParams _params;
        private readonly List<CommandParameter> _args;
        private readonly List<CommandParameter> _opts;
        private readonly List<Command> _subcmds;


        public Command(VoidParams @params)
        {
            _cmdCounter++;
            _params = @params;
            _params.AddCommand(this);
            _args = new List<CommandParameter>();
            _opts = new List<CommandParameter>();
            _subcmds = new List<Command>();
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
        ///     Routine process to be executed when command is called.
        /// </summary>
        internal Action<VoidParams>? TargetRoutine { get; set; }

        /// <summary>
        ///     Async routine to be executed when command is called.
        /// </summary>
        internal Func<VoidParams, Task>? AsyncTargetRoutine { get; set; }

        /// <summary>
        ///     Command to which the current command is considered to be a child command.
        /// </summary>
        public Command? ParentCommand { get; internal set; }

        /// <summary>
        ///     Command space to which this command belongs to.
        /// </summary>
        public CommandSpace? CommandSpace { get; internal set; } 

        /// <summary>
        ///     Set of subcommands for the current command.
        /// </summary>
        public IReadOnlyList<Command> Subcommands { get => _subcmds; }

        /// <summary>
        ///     List of arguments of command.
        /// </summary>
        public IReadOnlyList<CommandParameter> Arguments { get => _args; }

        /// <summary>
        ///     List of options of command.
        /// </summary>
        public IReadOnlyList<CommandParameter> Options { get => _opts; }

        /// <summary>
        ///     Parameters instance used by command as set of arguments and options.
        /// </summary>
        internal VoidParams Params => _params;

        /// <summary>
        ///     Executes the <see cref="TargetRoutine"/> of the command.
        /// </summary>
        [Obsolete("Access modifier should be changed.")]
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

            _params.Caller = this;
            if (IsAwaitable)
            {
                AsyncTargetRoutine?
                    .Invoke(_params)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                TargetRoutine?.Invoke(_params);
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                Arguments[i].ResetValue();                
            }
            _params.Caller = null;
        }

        /// <summary>
        ///     Adds specified command as command's subcommand.
        /// </summary>
        internal void AddSubcommand(Command cmd)
            => _subcmds.Add(cmd);

        /// <summary>
        ///     Adds specified argument to the collection of command arguments.
        /// </summary>
        internal void AddArgument(CommandParameter arg)
            => _args.Add(arg);

        /// <summary>
        ///     Adds specified option to the collection of command options.
        /// </summary>
        internal void AddOption(CommandParameter opt)
            => _opts.Add(opt);
    }
}
