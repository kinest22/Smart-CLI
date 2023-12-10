using System;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for command parameter that represents class of specified type.
    /// </summary>
    /// <typeparam name="TClass">Class parameter type.</typeparam>
    public class ClassParameterConfigurer<TClass> : CommandParameterConfigurer<ClassParameterConfigurer<TClass>, ClassParameter<TClass>>
        where TClass : class, IParsable<TClass>
    {
        public ClassParameterConfigurer(ClassParameter<TClass> arg) : base(arg)
        {
        }
    }
}
