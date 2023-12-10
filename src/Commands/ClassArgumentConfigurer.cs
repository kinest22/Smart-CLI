using System;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for argument that represents class of specified type.
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    public class ClassArgumentConfigurer<TClass> : CommandParameterConfigurer<ClassArgumentConfigurer<TClass>, ClassArgument<TClass>>
        where TClass : class, IParsable<TClass>
    {
        public ClassArgumentConfigurer(ClassArgument<TClass> arg) : base(arg)
        {
        }
    }
}
