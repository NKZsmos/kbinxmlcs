using System;
using System.Linq;
using System.Net;

namespace kbinxmlcs
{
    public static class Converters
    {
        public delegate byte[] StringToByteDelegate(string str);
        public delegate string ByteToStringDelegate(byte[] bytes);

        public static byte[] U8ToBytes(string str) => new[] { byte.Parse(str) };
        public static byte[] S8ToBytes(string str) => new[] { (byte)sbyte.Parse(str) };
        public static byte[] U16ToBytes(string str) => BitConverterHelper.GetBigEndianBytes(ushort.Parse(str));
        public static byte[] S16ToBytes(string str) => BitConverterHelper.GetBigEndianBytes(short.Parse(str));
        public static byte[] U32ToBytes(string str) => BitConverterHelper.GetBigEndianBytes(uint.Parse(str));
        public static byte[] S32ToBytes(string str) => BitConverterHelper.GetBigEndianBytes(int.Parse(str));
        public static byte[] U64ToBytes(string str) => BitConverterHelper.GetBigEndianBytes(ulong.Parse(str));
        public static byte[] S64ToBytes(string str) => BitConverterHelper.GetBigEndianBytes(long.Parse(str));
        public static byte[] SingleToBytes(string input) => BitConverterHelper.GetBigEndianBytes(float.Parse(input));
        public static byte[] DoubleToBytes(string input) => BitConverterHelper.GetBigEndianBytes(double.Parse(input));
        public static byte[] Ip4ToBytes(string input) => IPAddress.Parse(input).GetAddressBytes();

        // TODO: huge optimize
        public static string U8ToString(byte[] bytes) => bytes[0].ToString();
        public static string S8ToString(byte[] bytes) => ((sbyte)bytes[0]).ToString();
        public static string U16ToString(byte[] bytes) => BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0).ToString();
        public static string S16ToString(byte[] bytes) => BitConverter.ToInt16(bytes.Reverse().ToArray(), 0).ToString();
        public static string U32ToString(byte[] bytes) => BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0).ToString();
        public static string S32ToString(byte[] bytes) => BitConverter.ToInt32(bytes.Reverse().ToArray(), 0).ToString();
        public static string U64ToString(byte[] bytes) => BitConverter.ToUInt64(bytes.Reverse().ToArray(), 0).ToString();
        public static string S64ToString(byte[] bytes) => BitConverter.ToInt64(bytes.Reverse().ToArray(), 0).ToString();
        public static string SingleToString(byte[] buffer) => BitConverter.ToSingle(buffer.Reverse().ToArray(), 0).ToString("0.000000");
        public static string DoubleToString(byte[] buffer) => BitConverter.ToDouble(buffer.Reverse().ToArray(), 0).ToString("0.000000");
        public static string Ip4ToString(byte[] buffer) => new IPAddress(buffer).ToString();
    }
}