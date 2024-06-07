using System.Collections.Generic;
using SmartCLI.Commands;

namespace SmartCLI
{
    public class CliUnitTrie<TUnit>
        where TUnit : class, ISearchableUnit
    {
        public CliUnitTrie(TUnit? unit)
        {
            SubTries = new();
            Unit = unit;
            if (unit != null)
                IsTerminal = true;
        }

        public bool IsTerminal { get; init; }

        public TUnit? Unit { get; init; }
        
        public Dictionary<char, CliUnitTrie<TUnit>> SubTries { get; set; }

        public static CliUnitTrie<TUnit> CreateRootTrie()
            => new(null);
    }



    internal sealed class CliUnitSearchEngine2
    {
        private static readonly CliUnitSearchEngine2 _engine = new();

        // Hashcode-index dict. 
        // This dict contains collection hashcode as key 
        // and collection element search id as value
        private readonly Dictionary<int, CliUnitTrie<ISearchableUnit>> _tries;
        private readonly List<ISearchableUnit> _unitsAvailable;

        static CliUnitSearchEngine2()
        {
        }

        private CliUnitSearchEngine2()
        {
            _tries = new();
            _unitsAvailable = new();
        }

        public static CliUnitSearchEngine2 Create()
            => _engine;

        internal void RegisterUnitCollection<TUnit>(IEnumerable<TUnit> collection)
            where TUnit : class, ISearchableUnit
        {
            var root = CliUnitTrie<TUnit>.CreateRootTrie();            
            _tries.Add(collection.GetHashCode(), root);
        }

        private void AddUnit<TUnit>(TUnit unit)
            where TUnit : class, ISearchableUnit
        {
            var root = CliUnitTrie<TUnit>.CreateRootTrie();
            var currTrie = root;
            var unitName = unit.Name;

            for (int i = 0; i < unitName.Length; i++)
            {
                if (!currTrie.SubTries.ContainsKey(unitName[i]))
                {
                    if (i == unitName.Length - 1)
                    {
                        var newTrie = new CliUnitTrie<TUnit>(unit);
                        currTrie.SubTries.Add(unitName[i], newTrie);                        
                    }
                    else
                    {
                        var newTrie = new CliUnitTrie<TUnit>(null);
                        currTrie.SubTries.Add(unitName[i], newTrie);
                    }
                }
                currTrie = currTrie.SubTries[unitName[i]];
            }
        }

    }

}
