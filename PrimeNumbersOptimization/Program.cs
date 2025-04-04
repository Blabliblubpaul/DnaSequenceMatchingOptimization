namespace PrimeNumbersOptimization;

class Program {
    public const int PRIME_RANGE = 10000;
    
    static void Main(string[] args) {
        #region Execute
        
        
        ExecuteNaive();
        // ExecuteOptimized();
        
        
        #endregion
        
        #region Benchmarks
        
        
        // BenchmarkRunner.Run<CheckPrimesNaive>();
        // BenchmarkRunner.Run<CheckPrimesOptimized>();
        
        
        #endregion
    }

    private static void ExecuteNaive() {
        CheckPrimesNaive cpn = new();
        
        int[] primes = cpn.CheckPrimes();
        
        PrintArray(primes);
    }
    
    private static void ExecuteOptimized() {
        CheckPrimesOptimized cpo = new();
        
        int[] primes = cpo.CheckPrimes();
        
        PrintArray(primes);
    }

    private static void PrintArray(in int[] array) {
        Console.Write("[");
        
        Console.Write(string.Join(", ", array));
        
        Console.Write("]");
    }
}