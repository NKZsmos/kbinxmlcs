using System;
using System.Collections.Generic;
using System.Linq;

namespace kbinxmlcs
{
    internal static class Sixbit
    {
        private const string Charset = "0123456789:ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";

        private static readonly Dictionary<char, byte> CharsetMapping = Charset
            .Select((k, i) => (i, k))
            .ToDictionary(k => k.k, k => (byte)k.i);

        internal static byte[] Encode(string input)
        {
            var buffer = input.Select(k => CharsetMapping[k]).ToArray();
            var output = new byte[(int)Math.Ceiling(buffer.Length * 6.0 / 8)];

            for (var i = 0; i < buffer.Length * 6; i++)
                output[i / 8] = (byte)(output[i / 8] |
                    ((buffer[i / 6] >> (5 - (i % 6)) & 1) << (7 - (i % 8))));

            return output;
        }

        internal static string Decode(byte[] buffer, int length)
        {
            var output = new byte[length];

            for (var i = 0; i < length * 6; i++)
                output[i / 6] = (byte)(output[i / 6] |
                    (((buffer[i / 8] >> (7 - (i % 8))) & 1) << (5 - (i % 6))));

            return new string(output.Select(x => Charset[x]).ToArray());
        }
    }
}
