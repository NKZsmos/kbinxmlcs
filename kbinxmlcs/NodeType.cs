using static kbinxmlcs.Converters;

namespace kbinxmlcs
{
    internal class NodeType
    {
        public int Size { get; }

        public int Count { get; }

        public string Name { get; }

        public StringToByteDelegate GetBytes { get; }

        public ByteToStringDelegate GetString { get; }

        public NodeType(int size, int count, string name,
            StringToByteDelegate stringToByteDelegate, ByteToStringDelegate byteToStringDelegate)
        {
            Size = size;
            Count = count;
            Name = name;
            GetBytes = stringToByteDelegate;
            GetString = byteToStringDelegate;
        }
    }
}