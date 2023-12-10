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
        public NumericParameterConfigurer<TArg> HasNumericArg<TArg>(Expression<Func<TParams, TArg>> argSelection)
            where TArg : INumber<TArg>
        {
            if (argSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            var setter = accessedMember.GetSetMethod();
            var setDelegate = (Action<TArg>)Delegate.CreateDelegate(typeof(Action<TArg>), _cmd.Params, setter!);

            var configurer = new NumericParameterConfigurer<TArg>(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Specifies date-time argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public DateTimeParameterConfigurer HasDateTimeArg(Expression<Func<TParams, DateTime>> argSelection)
        {
            if (argSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            var setter = accessedMember.GetSetMethod();
            var setDelegate = (Action<DateTime>)Delegate.CreateDelegate(typeof(Action<DateTime>), _cmd.Params, setter!);

            var configurer = new DateTimeParameterConfigurer(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Specifies string argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public StringParameterConfigurer HasStringArg(Expression<Func<TParams, string>> argSelection)
        {

            if (argSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            var setter = accessedMember.GetSetMethod();
            var setDelegate = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), _cmd.Params, setter!);

            var configurer = new StringParameterConfigurer(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }

        /// <summary>
        ///     Specifies collection argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type</typeparam>
        /// <param name="argSelection">Argumnet property selector expression</param>
        /// <exception cref="ArgumentException"></exception>
        public CollectionParameterConfigurer<TArg> HasCollectionArg<TArg>(Expression<Func<TParams, ICollection<TArg>>> argSelection)
            where TArg : IParsable<TArg>
        {
            if (argSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(argSelection));

            var setter = accessedMember.GetSetMethod();
            var setDelegate = (Action<ICollection<TArg>>)Delegate.CreateDelegate(typeof(Action<ICollection<TArg>>), _cmd.Params, setter!);

            var configurer = new CollectionParameterConfigurer<TArg>(setDelegate);
            _cmd.AddArgument(configurer.GetParameter());
            return configurer;
        }


        public NumericParameterConfigurer<TOpt> HasNumericOpt<TOpt>(Expression<Func<TParams, TOpt>> optSelection)
            where TOpt : INumber<TOpt>
        {
            if (optSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(optSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(optSelection));

            var setter = accessedMember.GetSetMethod();
            var setDelegate = (Action<TOpt>)Delegate.CreateDelegate(typeof(Action<TOpt>), _cmd.Params, setter!);

            var configurer = new NumericParameterConfigurer<TOpt>(setDelegate);
            var z = configurer.GetParameter();
            z.IsOptional= true;
            z.Name = "--laksjdfhlkjsah";
            _cmd.AddArgument(z);
            return configurer;
        }


        /// <summary>
        ///     Returns configured <see cref="Command"/>.
        /// </summary>
        /// <returns></returns>
        internal Command GetCommand()
            => _cmd;
    }
}
