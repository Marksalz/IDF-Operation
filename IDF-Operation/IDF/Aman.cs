using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.IDF
{
    internal class Aman
    {
        private Hamas.Terrorist? _terrorist;
        private string? _location;
        private DateTime? _timestamp;

        public Aman(Hamas.Terrorist? terrorist, string? location, DateTime timestamp)
        {
            this._terrorist = terrorist;
            this._location = location;
            this._timestamp = timestamp;
        }

        public string getTerroristName()
        {
            return this._terrorist?.getName() ?? string.Empty;
        }

        public string getLocation()
        {
            return this._location ?? string.Empty;
        }

        public override string ToString()
        {
            return $"Report from {_timestamp?.ToString() ?? "unknown time"} on the terrorist: {_terrorist?.ToString() ?? "unknown terrorist"}." +
                $"Last known location is: {_location ?? "unknown location"}";
        }
    }
}
