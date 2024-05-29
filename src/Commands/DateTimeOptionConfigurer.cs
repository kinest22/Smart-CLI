using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCLI.Commands
{
    public class DateTimeOptionConfigurer : OptionConfigurer<DateTimeOptionConfigurer, DateTimeParameter>
    {
        public DateTimeOptionConfigurer(Action<DateTime?> valueProvider, bool isOptional)
        : base(new DateTimeParameter(valueProvider, isOptional))
            => _configurer = this;
    }
}
