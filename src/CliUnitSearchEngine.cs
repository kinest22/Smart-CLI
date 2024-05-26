using System;
using System.Collections;
using System.Collections.Generic;
using SmartCLI.Commands;

namespace SmartCLI
{
    /// <summary>
    ///     Provides methods for bidirectional search for CLI basic units.
    /// </summary>
    internal static class CliUnitSearchEngine
    {
        // Hashcode-index dict. 
        // This dict contains collection hashcode as key 
        // and collection element search id as value
        private static readonly Dictionary<int, int> _indexes = new();        

        /// <summary>
        ///     Finds next suitable CLI unit by snippet provided. 
        /// </summary>
        internal static TUnit? SearchForward<TUnit>(string snippet, IReadOnlyList<TUnit> units)
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
        internal static TUnit? SearchBackward<TUnit>(string snippet, IReadOnlyList<TUnit> units)
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
        ///     Registers collection of <see cref="ISearchableUnit"/>
        /// </summary>
        /// <param name="collection">Collection to register.</param>
        /// <exception cref="Exception"> thrown if specified collection already registered.</exception>
        internal static void RegisterUnitCollection<TUnit>(IEnumerable<TUnit> collection)
            where TUnit : ISearchableUnit
        {
            var hash = collection.GetHashCode();
            if (_indexes.TryAdd(hash, 0) == false)
                throw new Exception($"Collection of {typeof(TUnit)} was already registered.");
        }

        /// <summary>
        ///     Returns CLI unit index by collection hashcode.
        /// </summary>
        private static int GetIndex(IEnumerable collection, out int hash)
        {
            hash = collection.GetHashCode();
            if (_indexes.TryGetValue(hash, out int val))
                return val;
            throw new Exception("Collection has never been registered.");
        }
    }
}
