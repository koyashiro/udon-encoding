namespace Koyashiro.UdonEncoding
{
    using Koyashiro.UdonException;
    using Koyashiro.UdonList;

    public static class UdonUTF32
    {
        private const string EXCEPTION_INVALID_UNICODE_CODEPOINTS = "The byte array contains invalid Unicode code points.";

        public static byte[] GetBytes(char[] chars)
        {
            if (chars == null)
            {
                UdonException.ThrowArgumentNullException(nameof(chars));
            }

            var buf = UdonByteList.New(chars.Length * 4);

            for (var i = 0; i < chars.Length; i++)
            {
                var c1 = chars[i];

                if (c1 < 0xd800)
                {
                    buf.Add((byte)(c1 & 0xff));
                    buf.Add((byte)((c1 >> 8) & 0xff));
                    buf.Add((byte)0x00);
                    buf.Add((byte)0x00);
                }
                else if (c1 < 0xdbff)
                {
                    var c2 = chars[++i];

                    if (0xdc00 <= c2 && c2 < 0xdfff)
                    {
                        var n = (uint)(0x10000 + (c1 - 0xd800) * 0x400 + (c2 - 0xdc00));
                        buf.Add((byte)(n & 0xff));
                        buf.Add((byte)((n >> 8) & 0xff));
                        buf.Add((byte)((n >> 16) & 0xff));
                        buf.Add((byte)((n >> 24) & 0xff));
                    }
                    else
                    {
                        UdonException.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                        return default;
                    }
                }
                else
                {
                    UdonException.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                    return default;
                }
            }

            var bytes = buf.ToArray();

            return bytes;
        }

        public static byte[] GetBytes(string s)
        {
            if (s == null)
            {
                UdonException.ThrowArgumentNullException(nameof(s));
            }

            return GetBytes(s.ToCharArray());
        }

        public static char[] GetChars(byte[] bytes)
        {
            if (bytes == null)
            {
                UdonException.ThrowArgumentNullException(nameof(bytes));
            }

            var buf = UdonCharList.New(bytes.Length / 2);

            for (var i = 0; i < bytes.Length / 4; i++)
            {
                var c = (uint)((bytes[i * 4 + 3] << 24) | (bytes[i * 4 + 2] << 16) | (bytes[i * 4 + 1] << 8) | bytes[i * 4]);

                if (0x10ffff < c)
                {
                    UdonException.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                    return default;
                }

                if (c < 0x10000)
                {
                    buf.Add((char)c);
                }
                else
                {
                    buf.Add((char)((c - 0x10000) / 0x400 + 0xd800));
                    // buf.Add((char)((c - 0x10000) % 0x400 + 0xd800));
                    buf.Add((char)((c - 0x10000) - (0x400 * ((c - 0x10000) / 0x400)) + 0xdc00));
                }
            }

            var chars = buf.ToArray();

            return chars;
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes == null)
            {
                UdonException.ThrowArgumentNullException(nameof(bytes));
            }

            var chars = GetChars(bytes);
            var s = new string(chars);

            return s;
        }
    }
}
