using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.StrikeOptions
{
    abstract class StrikeOptions
    {
        private string? _name;
        int _capacity;
        double _fuelConsumption;
        List<string>? _effectiveTarget;

        public StrikeOptions(string? name, int capacity, double fuel, List<string>? effectiveTarget)
        {
            this._name = name;
            this._capacity = capacity;
            this._fuelConsumption = fuel;
            this._effectiveTarget = effectiveTarget;
        }

        public void updateCapacity()
        {
            if(_capacity > 0)
            {
                _capacity--;
            }
            else
            {
                Console.WriteLine("No more capacity left");
            }
        }

        public bool isAvailable()
        {
            if (_capacity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getCapacity() { return _capacity; }
        public string? getName() { return _name; }
    }
}
