using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kbinxmlcs
{
    internal class BigEndianBinaryBuffer
    {
        //protected List<byte> Buffer;
        //protected int Offset = 0;
        protected MemoryStream _stream;

        internal BigEndianBinaryBuffer(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
        }

        internal BigEndianBinaryBuffer()
        {
            _stream = new MemoryStream();
        }

        internal virtual byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            _stream.Read(buffer, 0, count);

            return buffer;
        }

        internal virtual void WriteBytes(Span<byte> buffer)
        {
            _stream.Write(buffer.ToArray(), 0, buffer.Length);
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
            while (_stream.Length % 4 != 0)
                _stream.WriteByte(0);
        }

        internal byte[] ToArray()
        {
            return _stream.ToArray();
        }

        internal int Length => (int) _stream.Length;

        //internal byte this[int index] => Buffer[index];
    }
}
