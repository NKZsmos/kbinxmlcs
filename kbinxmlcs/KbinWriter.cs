﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace kbinxmlcs
{
    /// <summary>
    /// Represents a writer for Konami's binary XML format.
    /// </summary>
    public class KbinWriter
    {
        private readonly XDocument _document;
        private readonly Encoding _encoding;

        private readonly NodeBuffer _nodeBuffer;
        private readonly DataBuffer _dataBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KbinWriter"/> class.
        /// </summary>
        /// <param name="xNode">The XML document to be written as a binary XML.</param>
        /// <param name="encoding">The encoding of the XML document.</param>
        public KbinWriter(XNode xNode, Encoding encoding)
        {
            _document = xNode is XDocument xDoc ? xDoc : new XDocument(xNode);

            _encoding = encoding;
            _nodeBuffer = new NodeBuffer(true, encoding);
            _dataBuffer = new DataBuffer(encoding);
        }

        /// <summary>
        /// Writes all nodes in the XML document.
        /// </summary>
        /// <returns>Returns an array of bytes containing the contents of the binary XML.</returns>
        public byte[] Write()
        {
            Recurse(_document.Root);
            _nodeBuffer.WriteU8(255);
            _nodeBuffer.Pad();
            _dataBuffer.Pad();

            //Write header data
            var output = new BigEndianBinaryBuffer();
            output.WriteU8(0xA0); //Magic
            output.WriteU8(0x42); //Compression flag
            output.WriteU8(EncodingDictionary.ReverseEncodingMap[_encoding]);
            output.WriteU8((byte)~EncodingDictionary.ReverseEncodingMap[_encoding]);

            //Write node buffer length and contents.
            var buffer = _nodeBuffer.ToArray();
            output.WriteS32(buffer.Length);
            output.WriteBytes(buffer);

            //Write data buffer length and contents.
            var array = _dataBuffer.ToArray();
            output.WriteS32(array.Length);
            output.WriteBytes(array);

            return output.ToArray();
        }

        private void Recurse(XElement xElement)
        {
            var typeStr = xElement.Attribute("__type")?.Value;
            var sizeStr = xElement.Attribute("__count")?.Value;

            if (typeStr == null)
            {
                _nodeBuffer.WriteU8(1);
                _nodeBuffer.WriteString(xElement.Name.LocalName);
            }
            else
            {
                var typeid = TypeDictionary.ReverseTypeMap[typeStr];
                if (sizeStr != null)
                    _nodeBuffer.WriteU8((byte)(typeid | 0x40));
                else
                    _nodeBuffer.WriteU8(typeid);

                _nodeBuffer.WriteString(xElement.Name.LocalName);
                var innerText = xElement.Value;
                if (typeStr == "str")
                    _dataBuffer.WriteString(innerText);
                else if (typeStr == "bin")
                    _dataBuffer.WriteBinary(innerText);
                else
                {
                    var type = TypeDictionary.TypeMap[typeid];
                    var value = innerText.Split(' ');
                    var size = (uint)(type.Size * type.Count);

                    if (sizeStr != null)
                    {
                        size *= uint.Parse(sizeStr);
                        _dataBuffer.WriteU32(size);
                    }
                    
                    var loopCount = size / type.Size;
                    for (var i = 0; i < loopCount; i++)
                        _dataBuffer.WriteBytes(type.GetBytes(value[i]));
                }
            }

            foreach (var attribute in xElement
                .Attributes()
                .Where(x => x.Name != "__type" && x.Name != "__size" && x.Name != "__count")
                .OrderBy(x => x.Name.LocalName))
            {
                _nodeBuffer.WriteU8(0x2E);
                _nodeBuffer.WriteString(attribute.Name.LocalName);
                _dataBuffer.WriteString(attribute.Value);
            }

            foreach (var childNode in xElement.Elements())
            {
                Recurse(childNode);
            }

            _nodeBuffer.WriteU8(0xFE);
        }
    }
}
