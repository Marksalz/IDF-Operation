using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.Hamas
{
    internal class Hamas
    {
        private DateTime _dateOfFoundation = new DateTime(1987, 1, 1);
        private Terrorist commander;
        private List<Terrorist> terrorists = new List<Terrorist>();

        public Hamas(Terrorist commander)
        {
            this.commander = commander;
        }

        public void AddTerroristListAsync(List<Terrorist> terrorists)
        {
            foreach (Terrorist terrorist in terrorists)
                this.terrorists.Add(terrorist);
        }

        public List<Terrorist> GetTerrorists()
        {
            return terrorists;
        }
    }
}
