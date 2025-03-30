using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace DnaSequenceMatchingOptimization;

public static unsafe class DnaSequenceMatching3 {
    private const ulong bitMask = 0x5555555555555555UL;
    private static readonly Vector256<ulong> bitMask4 = Vector256.Create(bitMask);

    private const int BATCH_SIZE = 4;

    private static IntPtr dbOrigin;
    private static ulong* database;

    private static int dbSize;
    
    public static void SetupTiny() {
        AllocateDatabase(DnaDatabase.DATASET_TINY_SIZE);
        dbSize = DnaDatabase.DATASET_TINY_SIZE;
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetTinyLazy()) {
            *(database + i) = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupSmall() {
        AllocateDatabase(DnaDatabase.DATASET_SMALL_SIZE);
        dbSize = DnaDatabase.DATASET_SMALL_SIZE;
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetSmallLazy()) {
            *(database + i) = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupMedium() {
        AllocateDatabase(DnaDatabase.DATASET_MEDIUM_SIZE);
        dbSize = DnaDatabase.DATASET_MEDIUM_SIZE;
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetMediumLazy()) {
            *(database + i) = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupLarge() {
        AllocateDatabase(DnaDatabase.DATASET_LARGE_SIZE);
        dbSize = DnaDatabase.DATASET_LARGE_SIZE;
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetLargeLazy()) {
            *(database + i) = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupHuge() {
        AllocateDatabase(DnaDatabase.DATASET_HUGE_SIZE);
        dbSize = DnaDatabase.DATASET_HUGE_SIZE;
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetHugeLazy()) {
            *(database + i) = SequenceToBinary(sequence);
            i++;
        }
    }

    private static void AllocateDatabase(int size) {
        dbOrigin = Marshal.AllocHGlobal(size * sizeof(ulong) + 31);
        
        database = (ulong*)(((ulong)dbOrigin + 31) & ~31UL);
    }

    public static DnaSequenceMatchResult MatchDnaSequence(string sequence) {
        // Create a local copy of bitMask4 to leverage faster stack lookup
        Vector256<ulong> bitMask4 = DnaSequenceMatching3.bitMask4;
        
        ulong binSeq = SequenceToBinary(sequence);

        Vector256<ulong> binSeq4 = Vector256.Create(binSeq);

        Vector256<int> smallestDistance4 = Vector256.Create(int.MaxValue);
        // int smallestDistance = 32;
        int index = 0;

        for (int i = 0; i < dbSize; i += BATCH_SIZE) {
            Vector256<ulong> dbSeq4;

            if (i + 3 < dbSize) {
                dbSeq4 = Vector256.LoadAligned(database + i);
            } else {
                dbSeq4 = Vector256.Create(
                    i < dbSize ? database[i] : 0,
                    i + 1 < dbSize ? database[i + 1] : 0,
                    i + 2 < dbSize ? database[i + 2] : 0,
                    i + 3 < dbSize ? database[i + 3] : 0
                );
            }
            
            Vector256<ulong> unequal4 = Vector256.Xor(dbSeq4, binSeq4);
            // Squash: (value | (value >> 1)) & bitMask;
            Vector256<ulong> unequal4Squashed = Vector256.BitwiseAnd(Vector256.BitwiseOr(unequal4, Vector256.ShiftRightLogical(unequal4, 1)), bitMask4);
            
            Vector256<int> distances = Vector256.Create(
                BitOperations.PopCount(unequal4Squashed[0]),
                BitOperations.PopCount(unequal4Squashed[1]),
                BitOperations.PopCount(unequal4Squashed[2]),
                BitOperations.PopCount(unequal4Squashed[3]),
                int.MaxValue,
                int.MaxValue,
                int.MaxValue,
                int.MaxValue
            );
            
            // Early exit if a complete match is found.
            if (Vector256.EqualsAny(distances, Vector256<int>.Zero)) {
                smallestDistance4 = distances;
                index += 4;
                break;
            }
            
            // TODO: solve comparison problem
            // TODO: Idea: Use Avx2.HorizontalMin, only for ushort -> could work since distance max 32

            smallestDistance4 = Vector256.Min(distances, smallestDistance4);

            // Index?
        }

        int smallestDistance = smallestDistance4[0];
        int index_ = index;

        if (smallestDistance4[1] > smallestDistance) {
            smallestDistance = smallestDistance4[1];
            index = index_ + 1;
        }
        
        if (smallestDistance4[2] > smallestDistance) {
            smallestDistance = smallestDistance4[2];
            index = index_ + 2;
        }
        
        if (smallestDistance4[3] > smallestDistance) {
            smallestDistance = smallestDistance4[3];
            index = index_ + 3;
        }
    
        double accuracy = 1 - smallestDistance / 32.0d;
        
        return new DnaSequenceMatchResult(index, accuracy);
    }

    private static ulong SequenceToBinary(string sequence) {
        ulong sequence_ = 0;
            
        for (int j = 0; j < 32; j++) {
            switch (sequence[j]) {
                case 'A': //00 
                    break;
                case 'G': //01 
                    sequence_ |= 1UL << 2 * j;
                    break;
                case 'C': //10 
                    sequence_ |= 1UL << 2 * j + 1;
                    break;
                case 'T': //11 
                    sequence_ |= 1UL << 2 * j;
                    sequence_ |= 1UL << 2 * j + 1;
                    break;
            }
        }
        
        return sequence_;
    }
}
