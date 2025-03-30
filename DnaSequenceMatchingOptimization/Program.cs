using BenchmarkDotNet.Running;

namespace DnaSequenceMatchingOptimization;

class Program {
    static void Main(string[] args) {
        Console.WriteLine(BenchmarkRunner.Run<DnaSequenceMatchingBenchmarks>());
    }
}
