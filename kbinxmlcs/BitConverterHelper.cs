using System;
using System.Buffers.Binary;

namespace kbinxmlcs
{
    public static class BitConverterHelper
    {
        public static ushort GetBigEndianUInt16(byte[] value)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(new ReadOnlySpan<byte>(value));
        }

        public static short GetBigEndianInt16(byte[] value)
        {
            return BinaryPrimitives.ReadInt16BigEndian(new ReadOnlySpan<byte>(value));
        }

        public static uint GetBigEndianUInt32(byte[] value)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(new ReadOnlySpan<byte>(value));
        }

        public static int GetBigEndianInt32(byte[] value)
        {
            return BinaryPrimitives.ReadInt32BigEndian(new ReadOnlySpan<byte>(value));
        }

        public static ulong GetBigEndianUInt64(byte[] value)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(new ReadOnlySpan<byte>(value));
        }

        public static long GetBigEndianInt64(byte[] value)
        {
            return BinaryPrimitives.ReadInt64BigEndian(new ReadOnlySpan<byte>(value));
        }
        public static float GetBigEndianSingle(byte[] value)
        {
#if NETSTANDARD2_1
            return BinaryPrimitivesExt.ReadSingleBigEndian(new ReadOnlySpan<byte>(value));
#elif NETSTANDARD2_0
            var arr = ReverseArray(value);
            return BitConverter.ToSingle(arr, 0);
#endif
        }

#if NETSTANDARD2_0
        public static float GetBigEndianSingleWithoutCopy(byte[] value)
        {
            var arr = ReverseSourceArrayNonCopy(value);
            return BitConverter.ToSingle(arr, 0);
        }
#endif

        public static double GetBigEndianDouble(byte[] value)
        {
            return BinaryPrimitivesExt.ReadDoubleBigEndian(new ReadOnlySpan<byte>(value));
        }

        public static byte[] GetBigEndianBytes(ushort value)
        {
            var array = new byte[sizeof(ushort)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            return array;
        }

        public static byte[] GetBigEndianBytes(short value)
        {
            var array = new byte[sizeof(short)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            return array;
        }

        public static byte[] GetBigEndianBytes(uint value)
        {
            var array = new byte[sizeof(uint)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            return array;
        }

        public static byte[] GetBigEndianBytes(int value)
        {
            var array = new byte[sizeof(int)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            return array;
        }

        public static byte[] GetBigEndianBytes(ulong value)
        {
            var array = new byte[sizeof(ulong)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            return array;
        }

        public static byte[] GetBigEndianBytes(long value)
        {
            var array = new byte[sizeof(long)];
            var span = new Span<byte>(array);
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            return array;
        }

        public static byte[] GetBigEndianBytes(float value)
        {
#if NETSTANDARD2_1
            var array = new byte[sizeof(float)];
            var span = new Span<byte>(array);
            BinaryPrimitivesExt.WriteSingleBigEndian(span, value);
            return array;
#elif NETSTANDARD2_0
            return ReverseSourceArrayNonCopy(BitConverter.GetBytes(value));
#endif
        }

        public static byte[] GetBigEndianBytes(double value)
        {
            var array = new byte[sizeof(double)];
            var span = new Span<byte>(array);
            BinaryPrimitivesExt.WriteDoubleBigEndian(span, value);
            return array;
        }

#if NETSTANDARD2_0

        private static byte[] ReverseSourceArrayNonCopy(byte[] source)
        {
            Array.Reverse(source);
            return source;
        }

        private static byte[] ReverseArray(byte[] source)
        {
            byte[] arr = new byte[source.Length];
            Array.Copy(source, 0, arr, 0, source.Length);
            Array.Reverse(arr);
            return arr;
        }
#endif
    }
}