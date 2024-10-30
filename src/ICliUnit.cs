using System.Collections.Generic;

namespace SmartCLI
{
    /// <summary>
    ///     Represents basic CLI unit that is subject to search.
    /// </summary>
    public interface ICliUnit
    {
        /// <summary>
        ///     CLI unit name that is used for search.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Identifies whether the CLI unit is hidden for search.
        /// </summary>
        public bool IsHidden { get; }
         
        /// <summary>
        ///     Collection of subunits.
        /// </summary>
        public IEnumerable<ICliUnit> SubUnits { get; }

        /// <summary>
        ///     Parent CLI unit.
        /// </summary>
        public ICliUnit? ParentUnit { get; }

        /// <summary>
        ///     Identifies whether the CLI unit is command parameter.
        /// </summary>
        public bool IsParameter { get; }

        /// <summary>
        ///     Identifies whether the CLI unit is command argument.
        ///     Used only when CLI unit is parameter.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        ///     Identifies whether the CLI unit is command option.
        ///     Used only when CLI unit is parameter.
        /// </summary>
        public bool IsOptional { get; }
    }
}
