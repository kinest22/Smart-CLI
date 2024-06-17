using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using SmartCLI.Commands;

[assembly: InternalsVisibleTo("demo")]
namespace SmartCLI
{
    /// <summary>
    ///     Represents .
    /// </summary>
    internal sealed class CliUnitSearchEngine
    {
        private static readonly CliUnitSearchEngine _engine = new();
        private readonly Dictionary<int, CliUnitTrie> _tries;
        private CliUnitTrie? _currentTrie;

        static CliUnitSearchEngine()
        {
        }

        /// <summary>
        ///     Creates new instance of <see cref="CliUnitSearchEngine"/>.
        /// </summary>
        private CliUnitSearchEngine()
        {
            _tries = new();
        }

        /// <summary>
        ///     Creates singleton instance of <see cref="CliUnitSearchEngine"/>.
        /// </summary>
        /// <returns>Instance of <see cref="CliUnitSearchEngine"/>.</returns>
        internal static CliUnitSearchEngine Create()
            => _engine;

        /// <summary>
        ///     Performs search by specified wildcard (prefix) within specified collection of CLI unis.
        /// </summary>
        /// <param name="wildcard">Wildcard (prefix)</param>
        /// <param name="collection">Collection of CLI unis</param>
        /// <returns>Number of CLI units found.</returns>
        internal int FindByWildcard(string wildcard, IEnumerable<ISearchableUnit> collection, in CliUnitCollection searchResults)
        {
            // searchResults list is considered to be empty
            int len = 0;
            int hash = collection.GetHashCode();
            if (TryFindTrie(wildcard, hash, out _currentTrie))
            {
                len = FetchTerminalUnits(_currentTrie!, in searchResults);
            }
            return len;
        }

        /// <summary>
        ///     Registrates CLI unit collection and builds its <see cref="CliUnitTrie"/> for futher search.
        /// </summary>
        /// <param name="collection">CLI unit collection.</param>
        internal void RegisterUnitCollection(IEnumerable<ISearchableUnit> collection)
        {
            var hash = collection.GetHashCode();
            if (_tries.ContainsKey(hash))
                throw new Exception("Collection already registered.");

            var root = CliUnitTrie.CreateRootTrie();
            foreach (var unit in collection)
                root.PopulateWith(unit);

            _tries.Add(hash, root);
        }

        /// <summary>
        ///     Gets <see cref="CliUnitTrie"/> associated with specified wildcard (prefix) and CLI unit collection hash code.
        /// </summary>
        /// <param name="wildcard">Wildcard (prefix)</param>
        /// <param name="hash">CLI unit collection hash code.</param>
        /// <param name="trie"><see cref="CliUnitTrie"/> found.</param>
        /// <returns>True if <see cref="CliUnitTrie"/> was found; otherwise false.</returns>
        private bool TryFindTrie(string wildcard, int hash, out CliUnitTrie? trie)
        {
            if (_tries.TryGetValue(hash, out trie))
            {
                foreach (char c in wildcard)
                {
                    if (trie.ContainsKey(c))
                    {
                        trie = trie[c];
                    }
                    else
                    {
                        trie = null;
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Fetches all units from all terminal <see cref="CliUnitTrie"/>s of specified trie.
        /// </summary>
        /// <returns>Number of CLI unts found.</returns>
        private int FetchTerminalUnits(CliUnitTrie trie, in CliUnitCollection found)
        {
            if (trie.IsTerminal)
                found.Add(trie.Unit!);

            foreach (char c in trie.Keys)
                FetchTerminalUnits(trie[c], in found);

            return found.Count;
        }
    }
}
