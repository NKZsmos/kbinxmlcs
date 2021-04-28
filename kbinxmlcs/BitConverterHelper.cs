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
            var length = sizeof(ushort);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            return span.ToArray();
        }

        public static Span<byte> GetBigEndianBytes(short value)
        {
            var length = sizeof(short);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            return span.ToArray();
        }

        public static Span<byte> GetBigEndianBytes(uint value)
        {
            var length = sizeof(uint);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            return span.ToArray();
        }

        public static Span<byte> GetBigEndianBytes(int value)
        {
            var length = sizeof(int);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            return span.ToArray();
        }

        public static Span<byte> GetBigEndianBytes(ulong value)
        {
            var length = sizeof(ulong);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            return span.ToArray();
        }

        public static Span<byte> GetBigEndianBytes(long value)
        {
            var length = sizeof(long);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            return span.ToArray();
        }

        public static Span<byte> GetBigEndianBytes(float value)
        {
#if NETSTANDARD2_1
            var length = sizeof(float);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitivesExt.WriteSingleBigEndian(span, value);
            return span.ToArray();
#elif NETSTANDARD2_0
            return ReverseSourceArrayNonCopy(BitConverter.GetBytes(value));
#endif
        }

        public static Span<byte> GetBigEndianBytes(double value)
        {
            var length = sizeof(double);
            Span<byte> span = stackalloc byte[length];
            BinaryPrimitivesExt.WriteDoubleBigEndian(span, value);
            return span.ToArray();
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