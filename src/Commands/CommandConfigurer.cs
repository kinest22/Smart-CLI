using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Collections.Generic;
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
        {
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
            _cmd.ParentUnit = parentCmd;
            parentCmd.AddSubcommand(_cmd);
            return this;
        }

        /// <summary>
        ///     Specifies routine to be executed when command is called.
        /// </summary>
        /// <param name="routine">Action delegate for the routine process.</param>
        /// <returns><see cref="CommandConfigurer{TParams}"/></returns>
        public CommandConfigurer<TParams> HasRoutine(Action<TParams> routine)
        {
            _cmd.TargetRoutine = (args) => routine.Invoke((TParams)args);
            return this;
        }

        /// <summary>
        ///     Specifies asynchronous routine to be executed when command is called.
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public CommandConfigurer<TParams> HasRoutine(Func<TParams, Task> routine)
        {
            _cmd.IsAwaitable = true;
            _cmd.AsyncTargetRoutine = async (args) => await routine.Invoke((TParams)args);
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
        ///     Specifies numeric argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type.</typeparam>
        /// <param name="argSelection">Argumnet property selector expression.</param>
        public NumericArgumentConfigurer<TArg> HasNumericArg<TArg>(Expression<Func<TParams, TArg>> argSelection)
            where TArg : struct, INumber<TArg>
        {
            var setDelegate = GetSetter(argSelection);
            var configurer = new NumericArgumentConfigurer<TArg>(setDelegate, false);
            var parameter = configurer.GetParameter();
            _cmd.AddArgument(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies date-time argument for command params.
        /// </summary>
        /// <param name="argSelection">Argumnet property selector expression.</param>
        public DateTimeArgumentConfigurer HasDateTimeArg(Expression<Func<TParams, DateTime>> argSelection)
        {
            var setDelegate = GetSetter(argSelection);
            var configurer = new DateTimeArgumentConfigurer(setDelegate, false);
            var parameter = configurer.GetParameter();
            _cmd.AddArgument(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies string argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type.</typeparam>
        /// <param name="argSelection">Argumnet property selector expression.</param>
        public StringArgumentConfigurer HasStringArg(Expression<Func<TParams, string>> argSelection)
        {
            var setDelegate = GetSetter(argSelection);
            var configurer = new StringArgumentConfigurer(setDelegate);
            var parameter = configurer.GetParameter();
            _cmd.AddArgument(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies collection argument for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type.</typeparam>
        /// <param name="argSelection">Argumnet property selector expression.</param>
        public CollectionArgumentConfigurer<TArg> HasCollectionArg<TArg>(Expression<Func<TParams, ICollection<TArg>>> argSelection)
            where TArg : IParsable<TArg>
        {
            var setDelegate = GetSetter(argSelection);
            var configurer = new CollectionArgumentConfigurer<TArg>(setDelegate, false);
            var parameter = configurer.GetParameter();
            _cmd.AddArgument(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies flag option for command params.
        /// </summary>
        /// <param name="optSelection">Option property selector expression.</param>
        public FlagOptionConfigurer HasFlagOpt(Expression<Func<TParams, bool>> optSelection)
        {
            var setDelegate = GetSetter(optSelection);
            var configurer = new FlagOptionConfigurer(setDelegate);
            var parameter = configurer.GetParameter();
            _cmd.AddOption(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies numeric option for command params.
        /// </summary>
        /// <typeparam name="TOpt">Option value type.</typeparam>
        /// <param name="optSelection">Option property selector expression.</param>
        /// <returns></returns>
        public NumericOptionConfigurer<TOpt> HasNumericOpt<TOpt>(Expression<Func<TParams, TOpt?>> optSelection)
            where TOpt : struct, INumber<TOpt>
        {
            var setDelegate = GetSetter(optSelection);
            var configurer = new NumericOptionConfigurer<TOpt>(setDelegate, true);
            var parameter = configurer.GetParameter();
            _cmd.AddOption(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies date-time option for command params.
        /// </summary>
        /// <param name="argSelection">Argumnet property selector expression.</param>
        public DateTimeOptionConfigurer HasDateTimeOpt(Expression<Func<TParams, DateTime?>> argSelection)
        {
            var setDelegate = GetSetter(argSelection);
            var configurer = new DateTimeOptionConfigurer(setDelegate, true);
            var parameter = configurer.GetParameter();
            _cmd.AddOption(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Specifies collection option for command params.
        /// </summary>
        /// <typeparam name="TArg">Argument type.</typeparam>
        /// <param name="argSelection">Argumnet property selector expression.</param>
        public CollectionOptionConfigurer<TArg> HasCollectionOpt<TArg>(Expression<Func<TParams, ICollection<TArg>>> argSelection)
            where TArg : IParsable<TArg>
        {
            var setDelegate = GetSetter(argSelection);
            var configurer = new CollectionOptionConfigurer<TArg>(setDelegate, false);
            var parameter = configurer.GetParameter();
            _cmd.AddOption(parameter);
            parameter.ParentUnit = _cmd;
            return configurer;
        }

        /// <summary>
        ///     Returns configured <see cref="Command"/>.
        /// </summary>
        /// <returns></returns>
        internal Command GetCommand()
        {
            CommandParameter.ResetCounters();
            return _cmd;
        }

        /// <summary>
        ///     Returns non-nullable property setter delegate from specified property selection expression.
        /// </summary>
        private Action<TProp> GetSetter<TProp>(Expression<Func<TParams, TProp>> propSelection)
        {
            if (propSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(propSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(propSelection));

            var setter = accessedMember.GetSetMethod();
            return (Action<TProp>)Delegate.CreateDelegate(typeof(Action<TProp>), _cmd.Params, setter!);
        }

        /// <summary>
        ///     Returns nullable property setter delegate from specified property selection expression.
        /// </summary>
        private Action<TProp?> GetSetter<TProp>(Expression<Func<TParams, TProp?>> propSelection)
            where TProp : struct
        {
            if (propSelection.Body is not MemberExpression memberExpression)
                throw new ArgumentException("Lambda must be a simple property access", nameof(propSelection));

            if (memberExpression.Member is not PropertyInfo accessedMember)
                throw new ArgumentException("Lambda must be a simple property access", nameof(propSelection));

            var setter = accessedMember.GetSetMethod();
            return (Action<TProp?>)Delegate.CreateDelegate(typeof(Action<TProp?>), _cmd.Params, setter!);
        }
    }
}
