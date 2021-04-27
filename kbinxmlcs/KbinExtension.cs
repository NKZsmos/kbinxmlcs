using System;
using System.Text;
using System.Xml.Linq;

namespace kbinxmlcs
{
    public static class KbinExtension
    {
        public static string ToStringWithDeclaration(this XDocument xDocument, SaveOptions options = SaveOptions.DisableFormatting)
        {
            if (options == SaveOptions.DisableFormatting)
            {
                return xDocument.Declaration + xDocument.ToString(options);
            }

            return xDocument.Declaration + Environment.NewLine + xDocument.ToString(options);
        }

        public static KbinWriter FromKbinEncoding(XNode node, KbinEncodings enumencoding)
        {
            Encoding encoding = null;
            switch (enumencoding)
            {
                case KbinEncodings.ShiftJIS:
                    encoding = Encoding.GetEncoding("shift_jis");
                    break;
                case KbinEncodings.ASCII:
                    encoding = Encoding.ASCII;
                    break;
                case KbinEncodings.ISO_8859_1:
                    encoding = Encoding.GetEncoding("iso-8859-1");
                    break;
                case KbinEncodings.EUCJP:
                    encoding = Encoding.GetEncoding("EUC-JP");
                    break;
                case KbinEncodings.UTF8:
                    encoding = Encoding.UTF8;
                    break;
                default:
                    break;
            }

            return new KbinWriter(node, encoding);
        }
    }
}
