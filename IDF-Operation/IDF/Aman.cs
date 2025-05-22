using IDF_Operation.Hamas;
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

        public Terrorist? GetTerrorist()
        {
            return this._terrorist;
        }

        public string getTerroristName()
        {
            return this._terrorist?.getName() ?? string.Empty;
        }

        public string getLocation()
        {
            return this._location ?? string.Empty;
        }

        public DateTime GetTimestamp()
        {
            return this._timestamp ?? DateTime.MinValue;
        }

        public override string ToString()
        {
            return $"Report from {_timestamp?.ToString() ?? "unknown time"} on the terrorist:\n{_terrorist?.ToString() ?? "unknown terrorist"} \n" +
                $"Last known location is: {_location ?? "unknown location"}\n";
        }
    }
}
