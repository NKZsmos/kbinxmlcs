using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;

namespace PerformanceTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xp = MarkdownExporter.GitHub;

            var summary = BenchmarkRunner.Run<ReadingTask>(DefaultConfig
                .Instance
                .AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig()))
                .AddExporter(xp)
            );
        }
    }

    public class ReadingTask
    {
        private readonly byte[] _bytes;
        private readonly byte[] _bytesLarge;
        private readonly Type _readerOld;
        private readonly Type _readerNew;

        public ReadingTask()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _bytes = File.ReadAllBytes(@"data\test_case.bin");
            _bytesLarge = File.ReadAllBytes(@"data\test_case2.bin");
            var ctxOld = new System.Runtime.Loader.AssemblyLoadContext("original", false);
            var asm1 = ctxOld.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"original\kbinxmlcs.dll"));
            _readerOld = asm1.GetType("kbinxmlcs.KbinReader");

            var ctxNew = new System.Runtime.Loader.AssemblyLoadContext("nkzsmos", false);
            var asm2 = ctxNew.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"nkzsmos\kbinxmlcs.dll"));
            _readerNew = asm2.GetType("kbinxmlcs.KbinReader");

        }

        [Benchmark]
        public object? Original_400KB()
        {
            var instance = Activator.CreateInstance(_readerOld, _bytes);
            var type = instance.GetType();
            var method = type.GetMethod("Read");
            return method.Invoke(instance, null);
        }

        [Benchmark]
        public object? NKZsmos_400KB()
        {
            var instance = Activator.CreateInstance(_readerNew, _bytes);
            var type = instance.GetType();
            var method = type.GetMethod("ReadLinq");
            return method.Invoke(instance, null);
        }

        [Benchmark]
        public object? Original_400KB_8ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(8)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerOld, _bytes);
                    var type = instance.GetType();
                    var method = type.GetMethod("Read");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? NKZsmos_400KB_8ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(8)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerNew, _bytes);
                    var type = instance.GetType();
                    var method = type.GetMethod("ReadLinq");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? Original_400KB_16ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(16)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerOld, _bytes);
                    var type = instance.GetType();
                    var method = type.GetMethod("Read");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? NKZsmos_400KB_16ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(16)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerNew, _bytes);
                    var type = instance.GetType();
                    var method = type.GetMethod("ReadLinq");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? Original_400KB_24ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(24)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerOld, _bytes);
                    var type = instance.GetType();
                    var method = type.GetMethod("Read");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? NKZsmos_400KB_24ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(24)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerNew, _bytes);
                    var type = instance.GetType();
                    var method = type.GetMethod("ReadLinq");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? Original_3300KB()
        {
            var instance = Activator.CreateInstance(_readerOld, _bytesLarge);
            var type = instance.GetType();
            var method = type.GetMethod("Read");
            return method.Invoke(instance, null);
        }

        [Benchmark]
        public object? NKZsmos_3300KB()
        {
            var instance = Activator.CreateInstance(_readerNew, _bytesLarge);
            var type = instance.GetType();
            var method = type.GetMethod("ReadLinq");
            return method.Invoke(instance, null);
        }

        [Benchmark]
        public object? Original_3300KB_8ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(8)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerOld, _bytesLarge);
                    var type = instance.GetType();
                    var method = type.GetMethod("Read");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? NKZsmos_3300KB_8ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(8)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerNew, _bytesLarge);
                    var type = instance.GetType();
                    var method = type.GetMethod("ReadLinq");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? Original_3300KB_16ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(16)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerOld, _bytesLarge);
                    var type = instance.GetType();
                    var method = type.GetMethod("Read");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? NKZsmos_3300KB_16ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(16)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerNew, _bytesLarge);
                    var type = instance.GetType();
                    var method = type.GetMethod("ReadLinq");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? Original_3300KB_24ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(24)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerOld, _bytesLarge);
                    var type = instance.GetType();
                    var method = type.GetMethod("Read");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }

        [Benchmark]
        public object? NKZsmos_3300KB_24ThreadsX24()
        {
            return new byte[24]
                .AsParallel()
                .WithDegreeOfParallelism(24)
                .Select(k =>
                {
                    var instance = Activator.CreateInstance(_readerNew, _bytesLarge);
                    var type = instance.GetType();
                    var method = type.GetMethod("ReadLinq");
                    return method.Invoke(instance, null);
                })
                .ToArray();
        }
    }
}
