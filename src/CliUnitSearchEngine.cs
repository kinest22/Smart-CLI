using System;
using System.Collections;
using System.Collections.Generic;
using SmartCLI.Commands;

namespace SmartCLI
{
    /// <summary>
    ///     Provides methods for bidirectional search for CLI basic units.
    /// </summary>
    internal class CliUnitSearchEngine
    {
        private static readonly CliUnitSearchEngine _engine = new();
        private readonly Dictionary<int, CliUnitTrie> _tries;
        private readonly List<ISearchableUnit> _unitsFound;
        private CliUnitTrie? _currentTrie;
        private int _index;

        static CliUnitSearchEngine()
        {
        }

        private CliUnitSearchEngine()
        {
            _tries = new();
            _unitsFound = new();
        }

        public static CliUnitSearchEngine Create()
            => _engine;



        internal ISearchableUnit? GetNext(string wildcard, IReadOnlyList<ISearchableUnit> units)
        {

            return null;
        }


        internal static ISearchableUnit? GetPrevious(string wildcard, IReadOnlyList<ISearchableUnit> units)
        {

            return null;
        }







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


        private bool TryFindSubTrie(string wildcard, IEnumerable<ISearchableUnit> collection)
        {
            _currentTrie = null;
            var hash = collection.GetHashCode();
            if (_tries.TryGetValue(hash, out _currentTrie))
            {
                foreach (char c in wildcard)
                {
                    if (_currentTrie.SubTries.ContainsKey(c))
                    {
                        _currentTrie = _currentTrie.SubTries[c];
                    }
                    else
                    {
                        _currentTrie = null;
                        return false;
                    }
                }
            }
            return false;
        }

        private int FetchTerminalUnits(CliUnitTrie trie)
        {
            _unitsFound.Clear();

            if (trie.IsTerminal)
                _unitsFound.Add(trie.Unit!);

            foreach (char c in trie.SubTries.Keys)
                FetchTerminalUnits(trie.SubTries[c]);

            return _unitsFound.Count;
        }
    }
}
