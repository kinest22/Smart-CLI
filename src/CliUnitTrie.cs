using System.Collections.Generic;
using SmartCLI.Commands;
using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace SmartCLI
{
    /// <summary>
    ///     Represents prefix trie for CLI units that are subject to search.
    /// </summary>
    public class CliUnitTrie : IReadOnlyDictionary<char, CliUnitTrie>
    {
        // child nodes
        private readonly Dictionary<char, CliUnitTrie> _childTries; 

        private CliUnitTrie()
        {
            _childTries = new();
        }

        private CliUnitTrie(ICliUnit? unit) : this()
        {            
            Unit = unit;
            if (unit != null)
                IsTerminal = true;
        }

        /// <summary>
        ///     Gets child <see cref="CliUnitTrie"/> associated with specified char key.
        /// </summary>
        /// <param name="key">Char key.</param>
        /// <returns>Child <see cref="CliUnitTrie"/></returns>
        public CliUnitTrie this[char key] 
            => _childTries[key];

        /// <summary>
        ///     Gets the value that indicates that current <see cref="CliUnitTrie"/> is terminal.
        /// </summary>
        public bool IsTerminal { get; init; }

        /// <summary>
        ///     Gets CLI unit associated with this <see cref="CliUnitTrie"/>. 
        ///     If this <see cref="CliUnitTrie"/> is not ternial associated CLI unit is null.
        /// </summary>
        public ICliUnit? Unit { get; init; }
        
        /// <summary>
        ///     Gets all keys contained in this <see cref="CliUnitTrie"/>.
        /// </summary>
        public IEnumerable<char> Keys 
            => _childTries.Keys;

        /// <summary>
        ///     Gets all child <see cref="CliUnitTrie"/>s.
        /// </summary>
        public IEnumerable<CliUnitTrie> Values 
            => _childTries.Values;

        /// <summary>
        ///     Gets the number of child <see cref="CliUnitTrie"/>s. 
        /// </summary>
        public int Count 
            => _childTries.Count;

        /// <summary>
        ///     Creates new <see cref="CliUnitTrie"/>.
        /// </summary>
        public static CliUnitTrie CreateRootTrie()
            => new(null);

        /// <summary>
        ///     Determines whether the <see cref="CliUnitTrie"/> contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(char key)
            => _childTries.ContainsKey(key);

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="CliUnitTrie"/>.
        /// </summary>
        public IEnumerator<KeyValuePair<char, CliUnitTrie>> GetEnumerator()
            => GetEnumerator();

        /// <summary>
        ///     Populates the <see cref="CliUnitTrie"/> with specified CLI unit.
        /// </summary>
        public void PopulateWith(ICliUnit unit)
        {
            var currTrie = this;
            var unitName = unit.Name;

            for (int i = 0; i < unitName.Length; i++)
            {
                if (!currTrie._childTries.ContainsKey(unitName[i]))
                {
                    var subTrie = i == unitName.Length - 1
                        ? new CliUnitTrie(unit)
                        : new CliUnitTrie(null);
                    currTrie._childTries.Add(unitName[i], subTrie);
                }
                currTrie = currTrie._childTries[unitName[i]];
            }
        }

        /// <summary>
        ///     Gets the <see cref="CliUnitTrie"/> associated with specified char.
        /// </summary>
        /// <param name="key">Char key.</param>
        /// <param name="value"></param>
        /// <returns>true if this <see cref="CliUnitTrie"/> contains an element with specified char key; otherwise false.</returns>
        public bool TryGetValue(char key, [MaybeNullWhen(false)] out CliUnitTrie value)
            => _childTries.TryGetValue(key, out value);

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="CliUnitTrie"/>.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => _childTries.GetEnumerator();
    }
}
