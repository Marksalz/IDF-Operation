using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.DTOs
{
    public class RootDTO
    {
        public List<TerroristDTO> terrorists { get; set; }
        public List<AmanReportDTO> aman_reports { get; set; }
    }
}
