using UdonSharp;
using Koyashiro.UdonTest;

namespace Koyashiro
{
    namespace UdonEncoding.Test
    {
        public class UdonUTF8Test : UdonSharpBehaviour
        {
            public void Start()
            {
                Assert.Equal(new byte[] { 0x61, 0x62, 0x63 }, UdonUTF8.GetBytes("abc"));
                Assert.Equal(new byte[] { 0xc2, 0xa5, 0xc2, 0xa6, 0xc2, 0xa7 }, UdonUTF8.GetBytes("¥¦§"));
                Assert.Equal(new byte[] { 0xe3, 0x81, 0x82, 0xe3, 0x81, 0x84, 0xe3, 0x81, 0x86 }, UdonUTF8.GetBytes("あいう"));
                Assert.Equal(new byte[] { 0xf0, 0x9f, 0xa4, 0x94, 0xf0, 0x9f, 0x8d, 0x9b, 0xf0, 0x9f, 0x8d, 0xa3 }, UdonUTF8.GetBytes("🤔🍛🍣"));

                Assert.Equal("abc", UdonUTF8.GetString(new byte[] { 0x61, 0x62, 0x63 }));
                Assert.Equal("¥¦§", UdonUTF8.GetString(new byte[] { 0xc2, 0xa5, 0xc2, 0xa6, 0xc2, 0xa7 }));
                Assert.Equal("あいう", UdonUTF8.GetString(new byte[] { 0xe3, 0x81, 0x82, 0xe3, 0x81, 0x84, 0xe3, 0x81, 0x86 }));
                Assert.Equal("🤔🍛🍣", UdonUTF8.GetString(new byte[] { 0xf0, 0x9f, 0xa4, 0x94, 0xf0, 0x9f, 0x8d, 0x9b, 0xf0, 0x9f, 0x8d, 0xa3 }));

                string output;
                Assert.True(UdonUTF8.TryGetString(new byte[] { 0x61, 0x62, 0x63 }, out output));
                Assert.Equal("abc", output);
                Assert.True(UdonUTF8.TryGetString(new byte[] { 0xc2, 0xa5, 0xc2, 0xa6, 0xc2, 0xa7 }, out output));
                Assert.Equal("¥¦§", output);
                Assert.True(UdonUTF8.TryGetString(new byte[] { 0xe3, 0x81, 0x82, 0xe3, 0x81, 0x84, 0xe3, 0x81, 0x86 }, out output));
                Assert.Equal("あいう", output);
                Assert.True(UdonUTF8.TryGetString(new byte[] { 0xf0, 0x9f, 0xa4, 0x94, 0xf0, 0x9f, 0x8d, 0x9b, 0xf0, 0x9f, 0x8d, 0xa3 }, out output));
                Assert.Equal("🤔🍛🍣", output);
            }
        }
    }
}
