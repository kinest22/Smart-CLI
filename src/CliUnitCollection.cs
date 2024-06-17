using SmartCLI.Commands;
using System.Collections;
using System.Collections.Generic;

namespace SmartCLI
{
    /// <summary>
    ///     Represents circular collection of CLI unit that are subject to search.
    /// </summary>
    public sealed class CliUnitCollection : IEnumerable<ISearchableUnit>  
    {
        private readonly List<ISearchableUnit> _list;
        private int _position;

        /// <summary>
        ///     Creates new instance of <see cref="CliUnitCollection"/>.
        /// </summary>
        public CliUnitCollection()
        {
            _list = new List<ISearchableUnit>();
            _position = -1;
        }

        /// <summary>
        ///     Gets the number of elements contained in colleciton.
        /// </summary>
        public int Count
        {
            get => _list.Count;
        }
            
        /// <summary>
        ///     Adds new <see cref="ISearchableUnit"/> to this collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(ISearchableUnit item)
        {
            _list.Add(item);
        }

        /// <summary>
        ///     Removes all elements from this collection.
        /// </summary>
        public void Clear()
        {
            _position = 0;
            _list.Clear();  
        }

        /// <summary>
        ///     Gets the first element contained in this colleciton.
        /// </summary>
        public ISearchableUnit GetFirst()
        {
            return _list[0];
        }

        /// <summary>
        ///     Gets current element of this colleciton.
        /// </summary>
        public ISearchableUnit GetCurrent()
        {
            return _list[_position];
        }

        /// <summary>
        ///     Gets next or first element of this collection.
        /// </summary>
        public ISearchableUnit GetNext()
        {
            if (_position + 1 >= _list.Count)
                _position = -1;
            return _list[++_position];
        }

        /// <summary>
        ///     Gets previous or last element of this collection.
        /// </summary>
        public ISearchableUnit GetPrevious()
        {
            if (_position - 1 <= -1)
                _position = _list.Count;
            return _list[--_position];
        }

        /// <summary>
        ///     Gets an enumerator that iterates through a collection.
        /// </summary>
        public IEnumerator<ISearchableUnit> GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Gets an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

}
