﻿using System;
using System.Collections.Generic;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Fluent configurer for parameter that represents collection of specified type.
    /// </summary>
    /// <typeparam name="TParam">Collection element type.</typeparam>
    public class CollectionParameterConfigurer<TParam> : CommandParameterConfigurer<CollectionParameterConfigurer<TParam>, CollectionParameter<TParam>>
        where TParam : IParsable<TParam>
    {
        public CollectionParameterConfigurer(Action<ICollection<TParam>> valueProvider) : base(new CollectionParameter<TParam>(valueProvider))
            => _configurer = this;
        
        /// <summary>
        ///     Sets max capacity collection can contain. 
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public CollectionParameterConfigurer<TParam> WithMaxCapacity(int capacity)
        {
            _param.MaxCapacity = capacity;
            return this;
        }

        /// <summary>
        ///     Sets parameter's allowed values.
        /// </summary>
        /// <returns><see cref="CollectionParameterConfigurer{TArg}"/></returns>
        public CollectionParameterConfigurer<TParam> WithAllowedValues(params TParam[] values)
        {
            _param.AllowedValues = values;
            return this;
        }

        /// <summary>
        ///     Sets additional transformation action that will be used for parsed values.
        /// </summary>
        /// <returns><see cref="CollectionParameterConfigurer{TArg}"/></returns>
        public CollectionParameterConfigurer<TParam> WithTransformation(Func<TParam, TParam> trnasformationAction)
        {
            _param.Transformer = trnasformationAction;
            return this;
        }

        /// <summary>
        ///     Sets additional validation action that will be called during validation.
        /// </summary>
        /// <returns><see cref="CollectionParameterConfigurer{TArg}"/></returns>
        public CollectionParameterConfigurer<TParam> WithValidation(Predicate<TParam> validationAction)
        {
            _param.Validator = validationAction;
            return this;
        }
    }
}
