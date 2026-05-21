using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    public class ZipDecorator : ExporterDecorator
    {
        public ZipDecorator(IExporter inner) : base(inner) { }

        public override void Export(List<DeliveryResult> results, string filePath)
        {
            var tempFile = Path.GetTempFileName();
            _inner.Export(results, tempFile);

            var zipPath = Path.ChangeExtension(filePath, ".zip");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            archive.CreateEntryFromFile(tempFile, Path.GetFileName(filePath));
            File.Delete(tempFile);
        }
    }
}
