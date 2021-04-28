using System;
using System.Buffers.Binary;

namespace kbinxmlcs
{
    public static class BitConverterHelper
    {
        public static ushort GetBigEndianUInt16(Span<byte> value)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(value);
        }

        public static short GetBigEndianInt16(Span<byte> value)
        {
            return BinaryPrimitives.ReadInt16BigEndian(value);
        }

        public static uint GetBigEndianUInt32(Span<byte> value)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(value);
        }

        public static int GetBigEndianInt32(Span<byte> value)
        {
            return BinaryPrimitives.ReadInt32BigEndian(value);
        }

        public static ulong GetBigEndianUInt64(Span<byte> value)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(value);
        }

        public static long GetBigEndianInt64(Span<byte> value)
        {
            return BinaryPrimitives.ReadInt64BigEndian(value);
        }
        public static float GetBigEndianSingle(Span<byte> value)
        {
#if NETSTANDARD2_1
            return BinaryPrimitivesExt.ReadSingleBigEndian(value);
#elif NETSTANDARD2_0
            var arr = ReverseArray(value);
            return BitConverter.ToSingle(arr.ToArray(), 0);
#endif
        }

#if NETSTANDARD2_0
        public static float GetBigEndianSingleWithoutCopy(Span<byte> value)
        {
            var arr = ReverseSourceArrayNonCopy(value);
            return BitConverter.ToSingle(arr.ToArray(), 0);
        }
#endif

        public static double GetBigEndianDouble(Span<byte> value)
        {
            return BinaryPrimitivesExt.ReadDoubleBigEndian(value);
        }

        public static Span<byte> GetBigEndianBytes(ushort value)
        {
            var array = new byte[sizeof(ushort)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            return span;
        }

        public static Span<byte> GetBigEndianBytes(short value)
        {
            var array = new byte[sizeof(short)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            return span;
        }

        public static Span<byte> GetBigEndianBytes(uint value)
        {
            var array = new byte[sizeof(uint)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            return span;
        }

        public static Span<byte> GetBigEndianBytes(int value)
        {
            var array = new byte[sizeof(int)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            return span;
        }

        public static Span<byte> GetBigEndianBytes(ulong value)
        {
            var array = new byte[sizeof(ulong)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            return span;
        }

        public static Span<byte> GetBigEndianBytes(long value)
        {
            var array = new byte[sizeof(long)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            return span;
        }

        public static Span<byte> GetBigEndianBytes(float value)
        {
#if NETSTANDARD2_1
            var array = new byte[sizeof(float)];
            var span = new Span<byte>(array);
            BinaryPrimitivesExt.WriteSingleBigEndian(span, value);
            return span;
#elif NETSTANDARD2_0
            return ReverseSourceArrayNonCopy(BitConverter.GetBytes(value));
#endif
        }

        public static Span<byte> GetBigEndianBytes(double value)
        {
            var array = new byte[sizeof(double)];
            var span = new Span<byte>(array);
            BinaryPrimitivesExt.WriteDoubleBigEndian(span, value);
            return span;
        }

#if NETSTANDARD2_0

        private static Span<byte> ReverseSourceArrayNonCopy(Span<byte> source)
        {
            source.Reverse();
            return source;
        }

        private static Span<byte> ReverseArray(Span<byte> source)
        {
            var arr = new Span<byte>(source.ToArray());
            arr.Reverse();
            return arr;
        }
#endif
    }
}