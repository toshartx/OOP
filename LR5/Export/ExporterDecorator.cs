using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    public abstract class ExporterDecorator : IExporter
    {
        protected readonly IExporter _inner;
        protected ExporterDecorator(IExporter inner)
        {
            _inner = inner;
        }
        public abstract void Export(List<DeliveryResult> results, string filePath);
        
    }
}
