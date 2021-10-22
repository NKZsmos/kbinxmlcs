using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;

namespace PerformanceTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xp = new CsvExporter(CsvSeparator.CurrentCulture);

            var summary = BenchmarkRunner.Run<ReadingTask>(ManualConfig
                .Create(new DebugBuildConfig())
                .AddExporter(xp)
                .AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig()))
            );
        }
    }

    internal class ReadingTask
    {
        private readonly string _text;

        public ReadingTask()
        {
            _text = File.ReadAllText(
                @"D:\GitHub\ReOsuStoryboardPlayer\ReOsuStoryboardPlayer.Core.UnitTest\TestData\IOSYS feat. 3L - Miracle-Hinacle (_lolipop).osb");
            var ctx = new System.Runtime.Loader.AssemblyLoadContext("old", false);
            var asm1 = ctx.LoadFromAssemblyPath(@"D:\GitHub\Coosu\Tests\CoosuTest\V1.0.0.0\Coosu.Storyboard.dll");

        }

        [Benchmark]
        public object? CommonKbinxmlcs()
        {
            var o = _method1?.Invoke(null, new object?[] { _text });
            return o;
        }

        [Benchmark]
        public object? NKZsmosKbinxmlcs()
        {
            var o = _method2?.Invoke(null, new object?[] { _text });
            return o;
        }
    }
}
