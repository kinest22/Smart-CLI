using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI
{
    internal readonly struct CliExecutionContext
    {
        public ICliUnit CommandSpace { get; init; }
        public ICliUnit Command { get; init; }
        public IEnumerable<string> Arguments { get; init; }
        public IReadOnlyDictionary<ICliUnit, string> Options { get; init; }
    }
}
