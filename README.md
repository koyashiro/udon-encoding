# UdonEncoding

Unicode encoder/decoder for UdonSharp.

## Installation

To use this package, you need to add [my package repository](https://github.com/koyashiro/vpm-repos).
Please read more details [here](https://github.com/koyashiro/vpm-repos#installation).

Please install this package with [Creator Companion](https://vcc.docs.vrchat.com/) or [VPM CLI](https://vcc.docs.vrchat.com/vpm/cli/).

### Creator Companion

1. Enable the `koyashiro` package repository.

   ![image](https://user-images.githubusercontent.com/6698252/230629434-048cde39-a0ec-4c53-bfe2-46bde2e6a57a.png)

2. Find `UdonEncoding` from the list of packages and install any version you want.

### VPM CLI

1. Execute the following command to install the package.

   ```sh
   vpm add package net.koyashiro.udonencoding
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
