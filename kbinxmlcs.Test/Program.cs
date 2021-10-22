﻿using System;
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

            TestRead();
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
            var xmlText = File.ReadAllText("test.xml");
            Console.WriteLine("Read file: " + sw.Elapsed);

            sw.Restart();
            XDocument xDocument = XDocument.Parse(xmlText);
            Console.WriteLine("Parse: " + sw.Elapsed);


            int count = 10;

            sw.Restart();
            object listLock = new object();
            var list = new List<object>();
            //var kbinWriter = new KbinWriter(xDocument, KbinEncodings.ShiftJIS.ToEncoding());
            //byte[] bytes1 = kbinWriter.Write();
            new int[count - 1 + 1].AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount + 1).ForAll(k =>
            {
                var kbinWriter = new KbinWriter(xDocument, KbinEncodings.ShiftJIS.ToEncoding());
                var allBytes = kbinWriter.Write();
                lock (listLock)
                {
                    list.Add(kbinWriter);
                    list.Add(allBytes);
                }
            });

            Console.WriteLine($"new writer {count} time(s): " + sw.Elapsed);

            Console.ReadKey(true);
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
