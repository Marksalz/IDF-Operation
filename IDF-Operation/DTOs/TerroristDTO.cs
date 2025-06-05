using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.DTOs
{
    public class TerroristDTO
    {
        public string name { get; set; }
        public int rank { get; set; }
        public bool alive { get; set; }
        public List<string> weapons { get; set; }
    }
}
