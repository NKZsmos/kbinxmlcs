using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace kbinxmlcs
{
    /// <summary>
    /// Represents a reader for Konami's binary XML format.
    /// </summary>
    public class KbinReader
    {
        private static readonly Type TypeControlType = typeof(ControlType);

        public Encoding Encoding { get; }

        private readonly NodeBuffer _nodeBuffer;
        private readonly DataBuffer _dataBuffer;

        private readonly XDocument _xDocument = new XDocument();
        private XElement _currentElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="KbinReader"/> class.
        /// </summary>
        /// <param name="buffer">An array of bytes containing the contents of a binary XML.</param>
        public KbinReader(byte[] buffer)
        {
            //Read header section.
            var binaryBuffer = new BigEndianBinaryBuffer(buffer);
            var signature = binaryBuffer.ReadU8();
            var compressionFlag = binaryBuffer.ReadU8();
            var encodingFlag = binaryBuffer.ReadU8();
            var encodingFlagNot = binaryBuffer.ReadU8();

            //Verify magic.
            if (signature != 0xA0)
                throw new KbinException($"Signature was invalid. 0x{signature.ToString("X2")} != 0xA0");

            //Encoding flag should be an inverse of the fourth byte.
            if ((byte)~encodingFlag != encodingFlagNot)
                throw new KbinException($"Third byte was not an inverse of the fourth. {~encodingFlag} != {encodingFlagNot}");

            var compressed = compressionFlag == 0x42;
            Encoding = EncodingDictionary.EncodingMap[encodingFlag];

            //Get buffer lengths and load.
            var span = new Span<byte>(buffer);
            var nodeLength = binaryBuffer.ReadS32();
            _nodeBuffer = new NodeBuffer(span.Slice(8, nodeLength).ToArray(), compressed, Encoding);

            var dataLength = BitConverterHelper.GetBigEndianInt32(span.Slice(nodeLength + 8, 4));
            _dataBuffer = new DataBuffer(span.Slice(nodeLength + 12, dataLength).ToArray(), Encoding);
            _xDocument.Declaration = new XDeclaration("1.0", Encoding.WebName, null);
        }

        /// <summary>
        /// Reads all nodes in the binary XML.
        /// </summary>
        /// <returns>Returns the XML document.</returns>
        public XDocument ReadLinq()
        {
            while (true)
            {
                var nodeType = _nodeBuffer.ReadU8();

                //Array flag is on the second bit
                var array = (nodeType & 0x40) > 0;
                nodeType = (byte)(nodeType & ~0x40);
                NodeType propertyType;

                if (Enum.IsDefined(TypeControlType, nodeType))
                {
                    switch ((ControlType)nodeType)
                    {
                        case ControlType.NodeStart:
                            var newElement = new XElement(_nodeBuffer.ReadString());

                            if (_currentElement != null)
                                _currentElement.Add(newElement);
                            else
                                _xDocument.Add(newElement);

                            _currentElement = newElement;
                            break;

                        case ControlType.Attribute:
                            var value = _dataBuffer.ReadString(_dataBuffer.ReadS32());
                            _currentElement.SetAttributeValue(_nodeBuffer.ReadString(), value);
                            break;

                        case ControlType.NodeEnd:
                            _currentElement = _currentElement.Parent;
                            break;

                        case ControlType.FileEnd:
                            return _xDocument;
                    }
                }
                else if (TypeDictionary.TypeMap.TryGetValue(nodeType, out var propertyType))
                {
                    var elementName = _nodeBuffer.ReadString();
                    var element = new XElement(elementName);
                    _currentElement.Add(element);
                    _currentElement = element;
                    _currentElement.SetAttributeValue("__type", propertyType.Name);

                    int arraySize;
                    if (array || propertyType.Name == "str" || propertyType.Name == "bin")
                        arraySize = _dataBuffer.ReadS32(); //Total size.
                    else
                        arraySize = propertyType.Size * propertyType.Count;

                    if (propertyType.Name == "str")
                        _currentElement.Value = _dataBuffer.ReadString(arraySize);
                    else if (propertyType.Name == "bin")
                    {
                        _currentElement.Value = _dataBuffer.ReadBinary(arraySize);
                        _currentElement.SetAttributeValue("__size", arraySize.ToString());
                    }
                    else
                    {
                        if (array)
                        {
                            var size = (arraySize / (propertyType.Size * propertyType.Count)).ToString();
                            _currentElement.SetAttributeValue("__count", size);
                        }

                        var span = _dataBuffer.ReadBytes(arraySize);
                        var stringBuilder = new StringBuilder();
                        var loopCount = arraySize / propertyType.Size;
                        for (var i = 0; i < loopCount; i++)
                        {
                            var subSpan = span.Slice(i * propertyType.Size, propertyType.Size);
                            stringBuilder.Append(propertyType.GetString(subSpan));
                            if (i != loopCount - 1) stringBuilder.Append(" ");
                        }

                        _currentElement.Value = stringBuilder.ToString();
                    }
                }
                else
                {
                    throw new KbinException($"Unknown node type: {nodeType}");
                }
            }
        }

        [Obsolete("Poor performance. Use \"" + nameof(ReadLinq) + "()\" instead.")]
        public XmlDocument Read()
        {
            var xDocument = ReadLinq();
            var xmlElement = new XmlDocument();
            xmlElement.LoadXml(xDocument.ToStringWithDeclaration());
            return xmlElement;
        }
    }
}
