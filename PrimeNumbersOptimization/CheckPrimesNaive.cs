using BenchmarkDotNet.Attributes;

namespace PrimeNumbersOptimization;

public class CheckPrimesNaive {
    [Benchmark]
    [IterationCount(5)]
    [InvocationCount(100)]
    public int[] CheckPrimes() {
        List<int> primes = new(Program.PRIME_RANGE);

        // Iterate through all numbers
        for (int i = 2; i < primes.Capacity + 1; i++) {
            bool isPrime = true;
            
            // If i is divided by any prime, it is not a prime
            foreach (int p in primes) {
                // Check for division
                if (i % p == 0) {
                    isPrime = false;
                    break;
                }
            }

            // Add to list
            if (isPrime) {
                primes.Add(i);
            }
        }
        
        return primes.ToArray();
    }
}