using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace kbinxmlcs
{
    public static class Sixbit
    {
        private const string Charset = "0123456789:ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";

        private static readonly Dictionary<char, byte> CharsetMapping = Charset
            .Select((k, i) => (i, k))
            .ToDictionary(k => k.k, k => (byte)k.i);

#if NETSTANDARD2_1 || NET5_0_OR_GREATER|| NET47_OR_GREATER
        [DllImport("kbinxmlrs", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern long sixcode_encode_test([MarshalAs(UnmanagedType.LPUTF8Str)] string str, int count);
#endif
        public static byte[] Encode(string input)
        {
            var buffer = new byte[input.Length].Select((x, i) => (byte)Charset.IndexOf(input[i])).ToArray();
            var output = new byte[(int)Math.Ceiling(buffer.Length * 6.0 / 8)];

            //for (var i = 0; i < buffer.Length * 6; i++)
            //    output[i / 8] = (byte)(output[i / 8] |
            //        ((buffer[i / 6] >> (5 - (i % 6)) & 1) << (7 - (i % 8))));

            //var encode = output.Slice(0, output.Length);
            return output.ToArray();
        }

        public static string Decode(Span<byte> buffer, int length)
        {
            var output = new byte[length];

            for (var i = 0; i < length * 6; i++)
                output[i / 6] = (byte)(output[i / 6] |
                    (((buffer[i / 8] >> (7 - (i % 8))) & 1) << (5 - (i % 6))));

            return new string(output.Select(x => Charset[x]).ToArray());
        }
    }
}
