using System;
using System.IO;

namespace kbinxmlcs
{
    public class BigEndianBinaryBuffer
    {
        protected Stream _stream;

        public BigEndianBinaryBuffer(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
        }

        public BigEndianBinaryBuffer()
        {
            _stream = new MemoryStream();
        }

        public virtual Span<byte> ReadBytes(int count)
        {
#if NETSTANDARD2_1
            var span = count <= 128
                ? stackalloc byte[count]
                : new byte[count];
            _stream.Read(span);
            return span.ToArray();
#elif NETSTANDARD2_0
            var buffer = new byte[count];
            _stream.Read(buffer, 0, count);
            return buffer;
#endif
        }

        public virtual void WriteBytes(Span<byte> buffer)
        {
#if NETSTANDARD2_1
            _stream.Write(buffer);
#elif NETSTANDARD2_0
            _stream.Write(buffer.ToArray(), 0, buffer.Length);
#endif
        }

        public virtual void WriteS8(sbyte value) => WriteBytes(new[] { (byte)value });

        public virtual void WriteS16(short value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        public virtual void WriteS32(int value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        public virtual void WriteS64(long value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        public virtual void WriteU8(byte value) => WriteBytes(new[] { value });

        public virtual void WriteU16(ushort value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        public virtual void WriteU32(uint value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        public virtual void WriteU64(ulong value) => WriteBytes(BitConverterHelper.GetBigEndianBytes(value));

        public virtual sbyte ReadS8() => (sbyte)ReadBytes(sizeof(byte))[0];

        public virtual short ReadS16() => BitConverterHelper.GetBigEndianInt16(ReadBytes(sizeof(short)));

        public virtual int ReadS32() => BitConverterHelper.GetBigEndianInt32(ReadBytes(sizeof(int)));

        public virtual long ReadS64() => BitConverterHelper.GetBigEndianInt64(ReadBytes(sizeof(short)));

        public virtual byte ReadU8() => ReadBytes(sizeof(byte))[0];

        public virtual ushort ReadU16() => BitConverterHelper.GetBigEndianUInt16(ReadBytes(sizeof(short)));

        public virtual uint ReadU32() => BitConverterHelper.GetBigEndianUInt32(ReadBytes(sizeof(int)));

        public virtual ulong ReadU64() => BitConverterHelper.GetBigEndianUInt64(ReadBytes(sizeof(long)));

        internal void Pad()
        {
            while (_stream.Length % 4 != 0)
                _stream.WriteByte(0);
        }

        public byte[] ToArray()
        {
            if (_stream is MemoryStream ms1)
                return ms1.ToArray();

            _stream.Position = 0;
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        public int Length => (int)_stream.Length;
    }
}
