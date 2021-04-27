using System;
using System.Collections.Generic;
using System.Linq;

namespace kbinxmlcs
{
    internal class BigEndianBinaryBuffer
    {
        protected List<byte> Buffer;
        protected int Offset = 0;

        internal BigEndianBinaryBuffer(byte[] buffer) => Buffer = new List<byte>(buffer);

        internal BigEndianBinaryBuffer() => Buffer = new List<byte>();

        internal virtual byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            Buffer.CopyTo(Offset, buffer, 0, count);
            Offset += count;

            return buffer;
        }

        internal virtual void WriteBytes(byte[] buffer)
        {
            Buffer.InsertRange(Offset, buffer);
            Offset += buffer.Length;
        }

        internal virtual void WriteS8(sbyte value) => WriteBytes(new byte[] { (byte)value });

        internal virtual void WriteS16(short value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        internal virtual void WriteS32(int value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        internal virtual void WriteS64(long value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        internal virtual void WriteU8(byte value) => WriteBytes(new byte[] { value });

        internal virtual void WriteU16(ushort value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        internal virtual void WriteU32(uint value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        internal virtual void WriteU64(ulong value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        internal virtual sbyte ReadS8() => (sbyte)ReadBytes(sizeof(byte))[0];

        internal virtual short ReadS16() => BitConverterHelper.GetBigEndianInt16(ReadBytes(sizeof(short)));

        internal virtual int ReadS32() => BitConverterHelper.GetBigEndianInt32(ReadBytes(sizeof(int)));

        internal virtual long ReadS64() => BitConverterHelper.GetBigEndianInt64(ReadBytes(sizeof(short)));

        internal virtual byte ReadU8() => ReadBytes(sizeof(byte))[0];

        internal virtual ushort ReadU16() => BitConverterHelper.GetBigEndianUInt16(ReadBytes(sizeof(short)));

        internal virtual uint ReadU32() => BitConverterHelper.GetBigEndianUInt32(ReadBytes(sizeof(int)));

        internal virtual ulong ReadU64() => BitConverterHelper.GetBigEndianUInt64(ReadBytes(sizeof(long)));
        
        internal void Pad()
        {
            while (Buffer.Count % 4 != 0)
                Buffer.Add(0);
        }

        internal byte[] ToArray() => Buffer.ToArray();

        internal int Length => Buffer.Count;

        internal byte this[int index] => Buffer[index];
    }
}
