using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.StrikeOptions
{
    internal class F16 : StrikeOptions
    {
        string? _bombType;

        public F16(string? name, int capacity, double fuel, List<string>? effectiveTarget, string? bombType) 
            : base(name, capacity, fuel, effectiveTarget)
        {
            this._bombType = bombType;
        }
    }
}
