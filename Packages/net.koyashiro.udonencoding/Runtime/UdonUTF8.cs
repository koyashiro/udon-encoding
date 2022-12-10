using System;
using Koyashiro.UdonEncoding.Internal;

namespace Koyashiro.UdonEncoding
{
    public static class UdonUTF8
    {
        private const string EXCEPTION_INVALID_UNICODE_CODEPOINTS = "The byte array contains invalid Unicode code points.";

        public static byte[] GetBytes(char[] chars)
        {
            if (chars == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(chars));
                return default;
            }

            var utf32bytes = UdonUTF32.GetBytes(chars);

            var buf = new byte[chars.Length * 4];
            var count = 0;

            for (var i = 0; i < utf32bytes.Length / 4; i++)
            {
                var uintChar = (uint)((utf32bytes[4 * i + 3] << 24) | (utf32bytes[4 * i + 2] << 16) | (utf32bytes[4 * i + 1] << 8) | utf32bytes[4 * i]);

                if (uintChar < 0x80)
                {
                    buf[count++] = (byte)uintChar;
                }
                else if (uintChar < 0x800)
                {
                    buf[count++] = (byte)(0xc0 | uintChar >> 6);
                    buf[count++] = (byte)(0x80 | uintChar & 0x3f);
                }
                else if (uintChar < 0x10000)
                {
                    buf[count++] = (byte)(0xe0 | uintChar >> 12);
                    buf[count++] = (byte)(0x80 | uintChar >> 6 & 0x3f);
                    buf[count++] = (byte)(0x80 | uintChar & 0x3f);
                }
                else
                {
                    buf[count++] = (byte)(0xf0 | uintChar >> 18);
                    buf[count++] = (byte)(0x80 | uintChar >> 12 & 0x3f);
                    buf[count++] = (byte)(0x80 | uintChar >> 6 & 0x3f);
                    buf[count++] = (byte)(0x80 | uintChar & 0x3f);
                }
            }

            var bytes = new byte[count];
            Array.Copy(buf, bytes, count);

            return bytes;
        }

        public static byte[] GetBytes(string s)
        {
            if (s == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(s));
                return default;
            }

            return GetBytes(s.ToCharArray());
        }

        public static char[] GetChars(byte[] bytes)
        {
            if (bytes == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(bytes));
                return default;
            }

            var buf = new byte[bytes.Length * 4];
            var count = 0;

            for (var i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i];

                if (b < 0x80)
                {
                    buf[count++] = (byte)(b & 0xff);
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
                }
                else if (b < 0xe0)
                {
                    var c = (uint)(((b & 0x1f) << 6) | (bytes[++i] & 0x3f));
                    buf[count++] = (byte)(c & 0xff);
                    buf[count++] = (byte)((c >> 8) & 0xff);
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
                }
                else if (b < 0xf0)
                {
                    var c = (uint)(((b & 0x0f) << 12) | ((bytes[++i] & 0x3f) << 6) | (bytes[++i] & 0x3f));
                    buf[count++] = (byte)(c & 0xff);
                    buf[count++] = (byte)((c >> 8) & 0xff);
                    buf[count++] = (byte)((c >> 16) & 0xff);
                    buf[count++] = (byte)0x00;
                }
                else if (b < 0xf8)
                {
                    var c = (uint)(((b & 0x07) << 18) | ((bytes[++i] & 0x3f) << 12) | ((bytes[++i] & 0x3f) << 6) | (bytes[++i] & 0x3f));
                    buf[count++] = (byte)(c & 0xff);
                    buf[count++] = (byte)((c >> 8) & 0xff);
                    buf[count++] = (byte)((c >> 16) & 0xff);
                    buf[count++] = (byte)((c >> 24) & 0xff);
                }
                else
                {
                    ExceptionHelper.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                    return default;
                }
            }

            var utf32Bytes = new byte[count];
            Array.Copy(buf, utf32Bytes, count);
            var chars = UdonUTF32.GetChars(utf32Bytes);

            return chars;
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(bytes));
                return default;
            }

            var chars = GetChars(bytes);
            var s = new string(chars);

            return s;
        }
    }
}
