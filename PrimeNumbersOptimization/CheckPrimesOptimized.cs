using BenchmarkDotNet.Attributes;

namespace PrimeNumbersOptimization;

/// <summary>
/// Optimized using the sieve of Eratosthenes.
/// </summary>
public class CheckPrimesOptimized {
    [Benchmark]
    [IterationCount(5)]
    [InvocationCount(100)]
    public int[] CheckPrimes() {
        // Initialize the 'sieve'
        bool[] sieve = Enumerable.Repeat(true, Program.PRIME_RANGE).ToArray();
        // Set 0, 1 to false (those are no primes)
        sieve[0] = sieve[1] = false;
        
        // Iterate up to Sqrt(PRIMES_RANGE)
        // -> x^0.5 is faster than sqrt(x)
        int max = (int)Math.Pow(Program.PRIME_RANGE, 0.5);
        for (int i = 2; i <= max; i++) {
            // If i is a prime, mark all multiples of i as prime.
            if (sieve[i]) {
                // Mark all multiples of j
                // -> i * i is the smallest number that
                // cannot have been marked already
                for (int j = i * i; j < sieve.Length; j += i) {
                    sieve[j] = false;
                }
            }
        }
        
        // Filter for primes
        List<int> primes = new(max);
        for (int i = 0; i < sieve.Length; i++) {
            if (sieve[i]) {
                primes.Add(i);
            }
        }

        return primes.ToArray();
    }
}