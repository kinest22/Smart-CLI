using System;
using System.Collections.Generic;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for argument that reprosents collection of specified type.
    /// </summary>
    /// <typeparam name="TArg">Collection element type.</typeparam>
    internal class CollectionArgumentConfigurer<TArg> : ArgumentConfigurer<CollectionArgumentConfigurer<TArg>, CollectionArgument<TArg>>
        where TArg : IParsable<TArg>
    {
        public CollectionArgumentConfigurer(Action<ICollection<TArg>> valueProvider) : base(new CollectionArgument<TArg>(valueProvider))
            => _configurer = this;
        
        /// <summary>
        ///     Sets max capacity collection can contain. 
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public CollectionArgumentConfigurer<TArg> WithMaxCapacity(int capacity)
        {
            _arg.MaxCapacity = capacity;
            return this;
        }

        /// <summary>
        ///     Sets argument's allowed values.
        /// </summary>
        /// <returns><see cref="CollectionArgumentConfigurer{TArg}"/></returns>
        public CollectionArgumentConfigurer<TArg> WithAllowedValues(params TArg[] values)
        {
            _arg.AllowedValues = values;
            return this;
        }
    }
}
