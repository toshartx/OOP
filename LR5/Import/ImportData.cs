using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Import
{
    class ImportData
    {
        public List<Cargo>? Cargos { get; set; }
        public string? TransportType { get; set; }
        public double Distance { get; set; }
    }
}
