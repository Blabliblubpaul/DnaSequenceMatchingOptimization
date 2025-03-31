using BenchmarkDotNet.Attributes;

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162 // Unreachable code detected

namespace DnaSequenceMatchingOptimization;

public class DnaSequenceMatchingSingleBenchmark {
    /// <summary>
    /// <list type="table">
    ///     <item><term>Tiny: 0</term></item>
    ///     <item><term>Small: 1</term></item>
    ///     <item><term>Medium: 2</term></item>
    ///     <item><term>Large: 3</term></item>
    ///     <item><term>Huge: 4</term></item>
    /// </list>
    /// </summary>
    private const int DB_SIZE = 0;

    private const bool USE_TINY_TESTSET = true;

    private readonly List<string> testset = [..GetTestSet()];

    [ParamsSource(nameof(TestSet))]
    public string dnaSequence;

    public IEnumerable<string> TestSet() => testset.ToArray();

    private static string[] GetTestSet() {
        return USE_TINY_TESTSET switch {
            true => File.ReadAllLines("Testsets/testset_tiny.csv"),
            false => File.ReadAllLines("Testsets/testset.csv")
        };
    }

    [IterationSetup(Target = nameof(Benchmark0))]
    public void Setup0() {
        switch (DB_SIZE) {
            case 0:
                DnaSequenceMatching0.SetupTiny();
                break;
            
            case 1:
                DnaSequenceMatching0.SetupSmall();
                break;
            
            case 2:
                DnaSequenceMatching0.SetupMedium();
                break;
            
            case 3:
                DnaSequenceMatching0.SetupLarge();
                break;
            
            case 4:
                DnaSequenceMatching0.SetupHuge();
                break;
        }
    }

    [Benchmark]
    [IterationCount(5)]
    public void Benchmark0() {
        DnaSequenceMatching0.MatchDnaSequence(dnaSequence);
    }
}