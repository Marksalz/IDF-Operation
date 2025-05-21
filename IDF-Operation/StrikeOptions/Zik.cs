using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.StrikeOptions
{
    internal class Zik : StrikeOptions  
    {
        public Zik(string? name, int capacity, double fuel, List<string>? effectiveTarget)
            : base(name, capacity, fuel, effectiveTarget) {}
    }
}
