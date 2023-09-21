using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Command fluent builder.
    /// </summary>
    public class CommandConfigurer<TParams>
        where TParams : VoidParams, new()
    {
        private readonly Command _cmd;

        public CommandConfigurer()
            => _cmd = new Command(() => new TParams());

        /// <summary>
        ///     Specifies the name (identifier) of the command which is used to call the command in the CLI. 
        ///     Command name is considered to be unique within command space.
        ///     Command name appears when help is used.
        /// </summary>
        /// <param name="name">Command name.</param>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> HasName(string name)
        {
            _cmd.Name = name;
            return this;
        }

        /// <summary>
        ///     Specifies the description of the command which is shown when help is called. 
        /// </summary>
        /// <param name="description"></param>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> HasDescription(string description)
        {
            _cmd.Description = description;
            return this;
        }

        /// <summary>
        ///     Sets parrent command for the current command.
        /// </summary>
        /// <param name="parentCmd"></param>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> IsSubcommandOf(Command parentCmd)
        {
            _cmd.ParentCommand = parentCmd;
            parentCmd.Subcommands ??= new();
            parentCmd.Subcommands.Add(_cmd);
            return this;
        }

        /// <summary>
        ///     Specifies routine process to be processed when command is called.
        /// </summary>
        /// <param name="routine">Action delegate for the routine process.</param>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> HasRoutine(Action<TParams> routine)
        {
            _cmd.TargetRoutine = (args) => routine.Invoke((TParams)args);
            return this;
        }

        /// <summary>
        ///     Marks command as hidden. Hidden commands do not shown when help is called.
        /// </summary>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> IsHidden()
        {
            _cmd.IsHidden = true;
            return this;
        }

        /// <summary>
        ///     Configures the command as awaitable (requires any significant time to execute) and sets 
        ///     the mark to be shown while command is beign executed (optionaly). Default mark is 'processing'.
        /// </summary>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> IsAwaitable(string awaitMark = "processing")
        {
            _cmd.IsAwaitable = true;
            _cmd.AwaitMark = awaitMark;
            return this;
        }

        /// <summary>
        ///     Returns configured <see cref="Command"/>.
        /// </summary>
        /// <returns></returns>
        internal Command GetCommand()
            => _cmd;
    }
}
