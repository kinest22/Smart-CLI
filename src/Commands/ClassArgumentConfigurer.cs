using System;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for command parameter that represents class of specified type.
    /// </summary>
    /// <typeparam name="TClass">Class parameter type.</typeparam>
    public class ClassArgumentConfigurer<TClass> : ArgumentConfigurer<ClassArgumentConfigurer<TClass>, ClassParameter<TClass>>
        where TClass : class, IParsable<TClass>
    {
        public ClassArgumentConfigurer(ClassParameter<TClass> arg) : base(arg)
        {
        }
    }
}
