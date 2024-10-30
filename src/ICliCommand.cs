using System.Collections.Generic;

namespace SmartCLI
{
    internal interface ICliCommand<T> where T : ICliUnit
    {
        public bool IsAwaitable { get; }
        public IReadOnlyList<T> Arguments { get; }
        public IReadOnlyList<T> Options { get; }
    }
}
