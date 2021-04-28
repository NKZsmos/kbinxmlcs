## kbinxmlcs

A tool for decoding Konami's binary XML format.

## Optimization compared to origin version

Writer:
Speed:
Single-thread: around 2.4x faster
Multi-thread: 3x~ faster
Memory: ~10% lower

Reader:
Speed:
Single-thread: around 1.73x faster
Multi-thread: around 1.73x faster
Memory: around 57% lower

## Usage in C#:

```cs
using System;
using System.IO;
using kbinxmlcs;

public class Program
{
    static void Main(string[] args)
    {
        byte[] data = File.ReadAllBytes("test.bin");
        XmlReader xmlReader = new XmlReader(data);
        Console.WriteLine(XmlReader.Read().OuterXml);
    }
}
```
