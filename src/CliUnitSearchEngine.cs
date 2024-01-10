using SmartCLI.Commands;
using System;
using System.Collections.Generic;

namespace SmartCLI
{
    /// <summary>
    ///     Provides methods for bidirectional search for CLI basic units.
    /// </summary>
    internal static class CliUnitSearchEngine
    {
        /// <summary>
        ///     Hashcode-index dict.
        /// </summary>
        private static readonly Dictionary<int, int> _indexes = new();        

        /// <summary>
        ///     Finds next suitable CLI unit by snippet provided. 
        /// </summary>
        public static TUnit? SearchForward<TUnit>(string snippet, IReadOnlyList<TUnit> units)
            where TUnit : class, ISearchableUnit
        {
            int index = GetIndex(units, out int hash);

            if (index >= units.Count - 1)
                _indexes[hash] = 0;

            for (int i = index; i < units.Count; i++, _indexes[hash]++)            
                if (units[i].IsHidden is false
                    && units[i].Name.StartsWith(snippet, StringComparison.OrdinalIgnoreCase)
                    ) return units[i];                
            
            return null;    
        }

        /// <summary>
        ///     Finds previous suitable CLI unit by snippet provided. 
        /// </summary>
        public static TUnit? SearchBackward<TUnit>(string snippet, IReadOnlyList<TUnit> units)
            where TUnit : class, ISearchableUnit
        {
            int index = GetIndex(units, out int hash);

            if (index <= 0)
                _indexes[hash] = units.Count - 1;

            for (int i = index; i >= 0; i--, _indexes[hash]--)
                if (units[i].IsHidden is false
                    && units[i].Name.StartsWith(snippet, StringComparison.OrdinalIgnoreCase)
                    ) return units[i];

            return null;
        }

        /// <summary>
        ///     Returns CLI unit index by collection hashcode.
        /// </summary>
        private static int GetIndex(object obj, out int hash)
        {
            hash = obj.GetHashCode();
            if (_indexes.TryGetValue(hash, out int val))
                return val;
            _indexes.Add(hash, 0);
            return 0;
        }
    }
}
