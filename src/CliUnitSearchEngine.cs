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



        internal static TUnit? Search<TUnit>(string snippet, IReadOnlyList<TUnit> units, SearchDirection direction)
            where TUnit : class, ISearchableUnit
        {
            int hash = units.GetHashCode();
            int currind = _indexes[hash];

            for(int i = currind; CheckForClause(i, units.Count, direction); i += (int)direction, _indexes[hash] += (int)direction)
            {
                if (!units[i].IsHidden && units[i].Name.StartsWith(snippet, StringComparison.OrdinalIgnoreCase))
                    return units[i];
            }           

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

        private static bool CheckForClause(int i, int n, SearchDirection dir)
        {
            if (dir == SearchDirection.Forward && i < n) return true;
            else if (dir == SearchDirection.Backward && i >= 0) return true;
            else return false;
        }
    }

    internal enum SearchDirection
    {
        None = 0,
        Forward = 1,
        Backward = -1,
    }




    public class Trie
    {

        private readonly CliUnitTrie _root;





        public Trie(string[] dict)
        {
            _root = new TrieNode("");
            foreach (string s in dict)
                InsertWord(s);
        }


        private void InsertWord(string s)
        {
            CliUnitTrie curr = _root;
            for (int i = 0; i < s.Length; i++)
            {
                if (!curr.Children.ContainsKey(s[i]))
                {

                    curr.Children.Add(s[i], new TrieNode(s.Substring(0, i + 1)));
                }
                curr = curr.Children[s[i]];
                if (i == s.Length - 1)
                    curr.IsTerminal = true;
            }
        }

        public List<string> GetWordsForPrefix(string pre)
        {
            List<string> results = new();
            CliUnitTrie curr = _root;
            foreach (char c in pre.ToCharArray())
            {
                if (curr.Children.ContainsKey(c))
                {
                    curr = curr.Children[c];
                }
                else
                {
                    return results;
                }
            }


            FindAllChildWords(curr, results);
            return results;
        }

        private void FindAllChildWords(CliUnitTrie n, List<string> results)
        {
            if (n.IsTerminal)
                results.Add(n.Prefix);
            foreach (var c in n.Children.Keys)
            {
                FindAllChildWords(n.Children[c], results);
            }
        }
    }


}
