using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    abstract class ExporterDecorator : IExporter
    {
        protected readonly IExporter _exporter;
        protected ExporterDecorator(IExporter inner)
        {
            _exporter = inner;
        }
        public abstract void Export(List<DeliveryResult> results, string filePath);
        
    }
}
