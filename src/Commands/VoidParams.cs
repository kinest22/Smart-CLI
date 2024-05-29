using System.Collections.Generic;

namespace SmartCLI.Commands
{
    /// <summary>
    ///     Represents base class for specific command parameters with necessary arguments and options.
    /// </summary>
    public class VoidParams 
    {
        /// <summary>
        ///     Collection of suitable commands that can use these parameters.
        /// </summary>
        public Command? Caller { get; internal set; }
    }
}
