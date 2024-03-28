using System;
using Koyashiro.UdonEncoding.Internal;

namespace Koyashiro.UdonEncoding
{
    [Obsolete("UdonUTF32 is obsolete. Use System.Text.Encoding instead.")]
    public static class UdonUTF32
    {
        private const string EXCEPTION_INVALID_UNICODE_CODEPOINTS = "The byte array contains invalid Unicode code points.";

        public static byte[] GetBytes(char[] chars)
        {
            if (chars == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(chars));
            }

            var buf = new byte[chars.Length * 4];
            var count = 0;

            for (var i = 0; i < chars.Length; i++)
            {
                var c1 = chars[i];

                if (char.IsHighSurrogate(c1))
                {
                    if (i + 1 >= chars.Length)
                    {
                        ExceptionHelper.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                        return default;
                    }

                    var c2 = chars[++i];

                    if (char.IsLowSurrogate(c2))
                    {
                        var n = char.ConvertToUtf32(c1, c2);
                        buf[count++] = (byte)(n & 0xff);
                        buf[count++] = (byte)((n >> 8) & 0xff);
                        buf[count++] = (byte)((n >> 16) & 0xff);
                        buf[count++] = (byte)((n >> 24) & 0xff);
                    }
                    else
                    {
                        ExceptionHelper.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                        return default;
                    }
                }
                else
                {
                    buf[count++] = (byte)(c1 & 0xff);
                    buf[count++] = (byte)((c1 >> 8) & 0xff);
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
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
            }

            return GetBytes(s.ToCharArray());
        }

        public static char[] GetChars(byte[] bytes)
        {
            if (bytes == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(bytes));
            }

            var buf = new char[bytes.Length / 2];
            var count = 0;

            for (var i = 0; i < bytes.Length / 4; i++)
            {
                var c = (bytes[i * 4 + 3] << 24) | (bytes[i * 4 + 2] << 16) | (bytes[i * 4 + 1] << 8) | bytes[i * 4];

                if (c < 0 || 0x10ffff < c || (0xd800 <= c && c < 0xe000))
                {
                    ExceptionHelper.ThrowArgumentException(EXCEPTION_INVALID_UNICODE_CODEPOINTS);
                    return default;
                }

                var s = char.ConvertFromUtf32(c);

                if (c < 0x10000)
                {
                    buf[count++] = s[0];
                }
                else
                {
                    buf[count++] = s[0];
                    buf[count++] = s[1];
                }
            }

            var chars = new char[count];
            Array.Copy(buf, chars, count);

            return chars;
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(bytes));
            }

            var chars = GetChars(bytes);
            var s = new string(chars);

            return s;
        }

        public static bool TryGetChars(byte[] bytes, out char[] output)
        {
            if (bytes == null)
            {
                output = default;
                return false;
            }

            var buf = new char[bytes.Length / 2];
            var count = 0;

            for (var i = 0; i < bytes.Length / 4; i++)
            {
                var c = (bytes[i * 4 + 3] << 24) | (bytes[i * 4 + 2] << 16) | (bytes[i * 4 + 1] << 8) | bytes[i * 4];

                if (c < 0 || 0x10ffff < c || (0xd800 <= c && c < 0xe000))
                {
                    output = default;
                    return false;
                }

                var s = char.ConvertFromUtf32(c);

                if (c < 0x10000)
                {
                    buf[count++] = s[0];
                }
                else
                {
                    buf[count++] = s[0];
                    buf[count++] = s[1];
                }
            }

            output = new char[count];
            Array.Copy(buf, output, count);

            return true;
        }

        public static bool TryGetString(byte[] bytes, out string output)
        {
            if (!TryGetChars(bytes, out var chars))
            {
                output = default;
                return false;
            }

            output = new string(chars);

            return true;
        }
    }
}
