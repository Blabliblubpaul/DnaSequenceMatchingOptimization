using BenchmarkDotNet.Running;

namespace DnaSequenceMatchingOptimization;

class Program {
    static void Main() {
        Console.WriteLine(BenchmarkRunner.Run<DnaSequenceMatchingSingleBenchmark>());
        // Console.WriteLine(BenchmarkRunner.Run<DnaSequenceMatchingBenchmarks>());
    }
}
