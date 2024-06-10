using System.Collections.Generic;
using SmartCLI.Commands;
using System;

namespace SmartCLI
{
    public class CliUnitTrie
    {
        private CliUnitTrie()
        {
            SubTries = new();
        }

        private CliUnitTrie(ISearchableUnit? unit) : this()
        {            
            Unit = unit;
            if (unit != null)
                IsTerminal = true;
        }

        public bool IsTerminal { get; init; }

        public ISearchableUnit? Unit { get; init; }
        
        public Dictionary<char, CliUnitTrie> SubTries { get; set; }

        public static CliUnitTrie CreateRootTrie()
            => new(null);

        public void PopulateWith(ISearchableUnit unit)
        {
            var currTrie = this;
            var unitName = unit.Name;

            for (int i = 0; i < unitName.Length; i++)
            {
                if (!currTrie.SubTries.ContainsKey(unitName[i]))
                {
                    var subTrie = i == unitName.Length - 1
                        ? new CliUnitTrie(unit)
                        : new CliUnitTrie(null);
                    currTrie.SubTries.Add(unitName[i], subTrie);
                }
                currTrie = currTrie.SubTries[unitName[i]];
            }
        }
    }
}
