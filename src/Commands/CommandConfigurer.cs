using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Collections.Generic;

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
        {
            CommandParameter.ResetCounter();
            _cmd = new Command(new TParams());
        }

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
            parentCmd.AddSubcommand(_cmd);
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
        ///     Specifies numeric argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public NumericArgumentConfigurer<TArg> HasNumericArg<TArg>(Expression<Func<TParams, TArg>> argSelection)
            where TArg : INumber<TArg>
        {
            var setDelegate = ExtractSetterDelegate(argSelection);
            var configurer = new NumericArgumentConfigurer<TArg>(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Specifies date-time argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public DateTimeArgumentConfigurer HasDateTimeArg(Expression<Func<TParams, DateTime>> argSelection)
        {
            var setDelegate = ExtractSetterDelegate(argSelection);
            var configurer = new DateTimeArgumentConfigurer(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Specifies string argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public StringArgumentConfigurer HasStringArg(Expression<Func<TParams, string>> argSelection)
        {
            var setDelegate = ExtractSetterDelegate(argSelection);
            var configurer = new StringArgumentConfigurer(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Specifies collection argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public CollectionArgumentConfigurer<TArg> HasCollectionArg<TArg>(Expression<Func<TParams, ICollection<TArg>>> argSelection)
            where TArg : IParsable<TArg>
        {
            var setDelegate = ExtractSetterDelegate(argSelection);
            var configurer = new CollectionArgumentConfigurer<TArg>(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Returns configured <see cref="Command"/>.
        /// </summary>
        /// <returns></returns>
        internal Command GetCommand()
            => _cmd;


        /// <summary>
        ///     Returns property setter delegate from specified property selection expression.
        /// </summary>
        private Action<TProp> ExtractSetterDelegate<TProp>(Expression<Func<TParams, TProp>> propSelection)
        {
            if (propSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(propSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(propSelection));

            var setter = accessedMember.GetSetMethod();
            return (Action<TProp>)Delegate.CreateDelegate(typeof(Action<TProp>), _cmd.Params, setter!);
        }
    }
}
