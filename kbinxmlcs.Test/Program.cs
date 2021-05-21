using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            var count = 4000000;
            var str = "asdfasdfasdfasdf";
            var rawTime =
                Sixbit.sixcode_encode_test(str, count);
            Console.WriteLine($"raw: {rawTime}ms");
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                Sixbit.Encode(str);
            }
            sw.Stop();
            Console.WriteLine($"net: {sw.ElapsedMilliseconds}ms");
            //TestRead();
            //TestWrite();
        }

        private static void TestRead()
        {
            byte[] data = File.ReadAllBytes("test.kbin");
            byte[] xmlBytes;
            Encoding encoding;
            using (var xmlReader = new KbinReader(data))
            {
                xmlBytes = xmlReader.ReadXmlByte();
                encoding = xmlReader.Encoding;
            }

            //Console.WriteLine(encoding.GetString(xmlBytes));

            XElement xElement;
            using (var memoryStream = new MemoryStream(xmlBytes))
            {
                xElement = XElement.Load(memoryStream);
            }

            var elements = xElement.Descendants();

            var sw = Stopwatch.StartNew();
            var bytes = File.ReadAllBytes("test.kbin");
            Console.WriteLine("Read file: " + sw.Elapsed);

            int count = 200;

            //var kbinReader1 = new KbinReader(bytes);
            //var doc1 = kbinReader1.ReadLinq();
            sw.Restart();
            object listLock = new object();
            var list = new List<object>();
            new int[count - 1 + 1].AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount + 1).ForAll(k =>
            {
                var kbinReader = new KbinReader(bytes);
                var doc = kbinReader.ReadLinq();
                lock (listLock)
                {
                    list.Add(kbinReader);
                    list.Add(doc);
                }
            });

            Console.WriteLine($"new reader {count} time(s): " + sw.Elapsed);
        }

        private static void TestWrite()
        {
            var sw = Stopwatch.StartNew();
            //var baseFolder = @"G:\GitHub\kbinxml-rs";
            var baseFolder = @".";
            //var file = Path.Combine(baseFolder, "common.get_music_info.xml");
            var file = Path.Combine(baseFolder, "huge.xml");
            var xmlText = File.ReadAllText(file);
            Console.WriteLine("Read file: " + sw.Elapsed);

            sw.Restart();
            XDocument xDocument = XDocument.Parse(xmlText);
            Console.WriteLine("Parse: " + sw.Elapsed);

            int count = 50;
            object listLock = new object();
            var list = new List<object>();
            var kbinWriter1 = new KbinWriter(xmlText, KbinEncodings.ShiftJIS.ToEncoding());
            var allBytes1 = kbinWriter1.WriteRs();
            var allBytes2 = kbinWriter1.Write();

            sw.Restart();
            new int[count - 1 + 1].AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount + 1).ForAll(k =>
            {
                var kbinWriter = new KbinWriter(xmlText, KbinEncodings.ShiftJIS.ToEncoding());
                var allBytes = kbinWriter.WriteRs();
                lock (listLock)
                {
                    list.Add(kbinWriter);
                    list.Add(allBytes);
                }
            });
            Console.WriteLine($"p/invoke writer {count} time(s): " + sw.Elapsed);
            //Console.ReadKey(true);
            list.Clear();
            sw.Restart();
            new int[count - 1 + 1].AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount + 1).ForAll(k =>
            {
                var kbinWriter = new KbinWriter(xmlText, KbinEncodings.ShiftJIS.ToEncoding());
                var allBytes = kbinWriter.Write();
                lock (listLock)
                {
                    list.Add(kbinWriter);
                    list.Add(allBytes);
                }
            });
            Console.WriteLine($"legacy writer {count} time(s): " + sw.Elapsed);
            //Console.ReadKey(true);
            //list.Clear();
            //sw.Restart();
            //new int[count - 1 + 1].AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount + 1).ForAll(k =>
            //{
            //    XDocument xDoc = XDocument.Parse(xmlText);
            //    var kbinWriter = new KbinWriter(xDoc, KbinEncodings.ShiftJIS.ToEncoding());
            //    var allBytes = kbinWriter.Write();
            //    lock (listLock)
            //    {
            //        list.Add(kbinWriter);
            //        list.Add(allBytes);
            //    }
            //});
            //Console.WriteLine($"legacy writer with linq {count} time(s): " + sw.Elapsed);

            //Console.ReadKey(true);
        }

        public static string PrintXML(string xml)
        {
            string result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(xml);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                string formattedXml = sReader.ReadToEnd();

                result = formattedXml;
            }
            catch (XmlException)
            {
                // Handle the exception
            }

            mStream.Close();
            writer.Close();

            return result;
        }
    }
}
