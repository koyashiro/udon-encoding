# UdonEncoding

Unicode encoder/decoder for UdonSharp.

## Installation

```sh
vpm add repo https://vpm.koyashiro.net/repos.json
```

## Examples

```cs
using UnityEngine;
using UdonSharp;
using Koyashiro.UdonEncoding;

public class UdonEncodingSample : UdonSharpBehaviour
{
    public void Start()
    {
        var s = UdonUTF8.GetString(new byte[] { 0x66, 0x6f, 0x78, 0xf0, 0x9f, 0xa6, 0x8a });
        Debug.Log(s); // fox🦊

        var bytes = UdonUTF8.GetBytes("fox🦊");
        Debug.Log(bytes.Length); // 7
        Debug.Log($"0x{bytes[0]:x}"); // 0x66
        Debug.Log($"0x{bytes[1]:x}"); // 0x6f
        Debug.Log($"0x{bytes[2]:x}"); // 0x78
        Debug.Log($"0x{bytes[3]:x}"); // 0xf0
        Debug.Log($"0x{bytes[4]:x}"); // 0x9f
        Debug.Log($"0x{bytes[5]:x}"); // 0xa6
        Debug.Log($"0x{bytes[6]:x}"); // 0x8a
    }
}
```
