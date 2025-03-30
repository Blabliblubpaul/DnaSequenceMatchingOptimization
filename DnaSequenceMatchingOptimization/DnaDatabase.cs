namespace DnaSequenceMatchingOptimization;

public static class DnaDatabase {
    private const string DATASET_PATH = "Datasets/";
    
    private const string DATASET_TINY_PATH = DATASET_PATH + "dataset_tiny.csv";
    private const string DATASET_SMALL_PATH = DATASET_PATH + "dataset_small.csv";
    private const string DATASET_MEDIUM_PATH = DATASET_PATH + "dataset_medium.csv";
    private const string DATASET_LARGE_PATH = DATASET_PATH + "dataset_large.csv";
    private const string DATASET_HUGE_PATH = DATASET_PATH + "dataset_huge.csv";
    
    private const string nucleotides = "AGCT";
    private static readonly Random random = new();

    public const int DNA_SEQUENCE_NUCLEOTIDES = 32;
    
    public const int DATASET_TINY_SIZE = 100;
    public const int DATASET_SMALL_SIZE = 1000;
    public const int DATASET_MEDIUM_SIZE = 10000;
    public const int DATASET_LARGE_SIZE = 100000;
    public const int DATASET_HUGE_SIZE = 1000000;

    #region DATASET_TINY
    public static string[] LoadDatasetTiny() {
        return File.ReadAllLines(DATASET_TINY_PATH);
    }
    
    public static async Task<string[]> LoadDatasetTinyAsync() {
        return await File.ReadAllLinesAsync(DATASET_TINY_PATH);
    }

    public static IEnumerable<string> LoadDatasetTinyLazy() {
        return File.ReadLines(DATASET_TINY_PATH);
    }
    
    public static IAsyncEnumerable<string> LoadDatasetTinyLazyAsync() {
        return File.ReadLinesAsync(DATASET_TINY_PATH);
    }
    #endregion
    
    #region DATASET_SMALL
    public static string[] LoadDatasetSmall() {
        return File.ReadAllLines(DATASET_SMALL_PATH);
    }
    
    public static async Task<string[]> LoadDatasetSmallAsync() {
        return await File.ReadAllLinesAsync(DATASET_SMALL_PATH);
    }

    public static IEnumerable<string> LoadDatasetSmallLazy() {
        return File.ReadLines(DATASET_SMALL_PATH);
    }
    
    public static IAsyncEnumerable<string> LoadDatasetSmallLazyAsync() {
        return File.ReadLinesAsync(DATASET_SMALL_PATH);
    }
    #endregion
    
    #region DATASET_MEDIUM
    public static string[] LoadDatasetMedium() {
        return File.ReadAllLines(DATASET_MEDIUM_PATH);
    }
    
    public static async Task<string[]> LoadDatasetMediumAsync() {
        return await File.ReadAllLinesAsync(DATASET_MEDIUM_PATH);
    }

    public static IEnumerable<string> LoadDatasetMediumLazy() {
        return File.ReadLines(DATASET_MEDIUM_PATH);
    }
    
    public static IAsyncEnumerable<string> LoadDatasetMediumLazyAsync() {
        return File.ReadLinesAsync(DATASET_MEDIUM_PATH);
    }
    #endregion
    
    #region DATASET_LARGE
    public static string[] LoadDatasetLarge() {
        return File.ReadAllLines(DATASET_LARGE_PATH);
    }
    
    public static async Task<string[]> LoadDatasetLargeAsync() {
        return await File.ReadAllLinesAsync(DATASET_LARGE_PATH);
    }

    public static IEnumerable<string> LoadDatasetLargeLazy() {
        return File.ReadLines(DATASET_LARGE_PATH);
    }
    
    public static IAsyncEnumerable<string> LoadDatasetLargeLazyAsync() {
        return File.ReadLinesAsync(DATASET_LARGE_PATH);
    }
    #endregion
    
    #region DATASET_HUGE
    public static string[] LoadDatasetHuge() {
        return File.ReadAllLines(DATASET_HUGE_PATH);
    }
    
    public static async Task<string[]> LoadDatasetHugeAsync() {
        return await File.ReadAllLinesAsync(DATASET_HUGE_PATH);
    }

    public static IEnumerable<string> LoadDatasetHugeLazy() {
        return File.ReadLines(DATASET_HUGE_PATH);
    }
    
    public static IAsyncEnumerable<string> LoadDatasetHugeLazyAsync() {
        return File.ReadLinesAsync(DATASET_HUGE_PATH);
    }
    #endregion
    
    public static string GetRandomDnaSequence() {
        char[] sequence = new char[32];
        
        for (int i = 0; i < 32; i++) {
            sequence[i] = nucleotides[random.Next(0, 4)];
        }

        return new string(sequence);
    }
}