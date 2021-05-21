## kbinxmlcs

A tool for decoding Konami's binary XML format.

## Optimization compared to origin version

### Writer

**Speed**:

- Single-thread: around 2.4x faster
- Multi-thread: 3x~ faster (Great CPU usage)

**Memory**:

- ~10% lower (No GC)

### Reader

**Speed**:

- Single-thread `ReadXmlByte()`: around 1.76x faster
- Single-thread `ReadLinq()`: around 1.25x faster
- Multi-thread `ReadXmlByte()`: around 3.88x faster (Great CPU usage)
- Multi-thread `ReadLinq()`: around 2.25x faster (Great CPU usage)

**Memory**:

- `ReadXmlByte()` around 41% lower (Almost no GC)
- `ReadLinq()` around 57% lower (GC when LoadXml())

## Usage in C#:

```cs
using System;
using System.IO;
using kbinxmlcs;

public class Program
{
    static void Main(string[] args)
    {
        byte[] data = File.ReadAllBytes("test.kbin");
        byte[] xmlBytes;
        Encoding encoding;
        using (var xmlReader = new KbinReader(data))
        {
            xmlBytes = xmlReader.ReadXmlByte();
            encoding = xmlReader.Encoding;
        }

        //Console.WriteLine(encoding.GetString(xmlBytes));

        XElement xElement;
        using (var memoryStream = new MemoryStream(xmlBytes))
        {
            xElement = XElement.Load(memoryStream);
        }

        var elements = xElement.Descendants();
    }
}
```
