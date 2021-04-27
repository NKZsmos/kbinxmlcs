using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace kbinxmlcs.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var sw = Stopwatch.StartNew();
            var xmlText = File.ReadAllText("test.xml");
            Console.WriteLine("Read xml: " + sw.Elapsed);

            sw.Restart();
            var xDocument = XDocument.Parse(xmlText);
            Console.WriteLine("Parse: " + sw.Elapsed);

            sw.Restart();
            var kbinWriter = KbinExtension.FromKbinEncoding(xDocument, KbinEncodings.ShiftJIS);

            int count = 10;
            for (int i = 0; i < count; i++)
            {
                var allBytes = kbinWriter.Write();
            }

            Console.WriteLine($"Convert(new) {count} time(s): " + sw.Elapsed);
        }
    }
}
