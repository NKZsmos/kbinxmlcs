using static kbinxmlcs.Converters;

namespace kbinxmlcs
{
    public class NodeType
    {
        public int Size 
        {
            get;
        }
        
        public int Count
        {
            get; 
        }

        public string Name
        {
            get;
        }

        public StringToByteDelegate ToBytes
        {
            get;
        }

        public new ByteToStringDelegate ToString
        {
            get;
        }

        public NodeType(int size, int count, string name, 
            StringToByteDelegate stringToByteDelegate, ByteToStringDelegate byteToStringDelegate)
        {
            Size = size;
            Count = count;
            Name = name;
            ToBytes = stringToByteDelegate;
            ToString = byteToStringDelegate;
        }
    }
}