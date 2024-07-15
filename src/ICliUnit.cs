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
         
        public IEnumerable<ICliUnit> SubUnits { get; }

        public bool IsParameter { get; }
    }
}
