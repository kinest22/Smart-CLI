using SmartCLI.Commands;
using System.Collections;
using System.Collections.Generic;

namespace SmartCLI
{
    public class CliCommandHierarchy : IEnumerable<CommandSpace>
    {
        private readonly List<CommandSpace> _hierarchy;

        public CliCommandHierarchy()        
            => _hierarchy = new();        

        public IEnumerator<CommandSpace> GetEnumerator()        
            => _hierarchy.GetEnumerator();
        

        IEnumerator IEnumerable.GetEnumerator()        
            => GetEnumerator();
        

        internal void Add(CommandSpace space)        
            => _hierarchy.Add(space);
        


        //internal void AddToHierarchy(CliCommandContext context)
        //{
        //}
    }
}
