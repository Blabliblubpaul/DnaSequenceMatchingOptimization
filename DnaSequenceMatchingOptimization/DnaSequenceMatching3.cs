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

        int smallestDistance = 32;
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

            int distance0 = BitOperations.PopCount(unequal4Squashed[0]);
            int distance1 = BitOperations.PopCount(unequal4Squashed[1]);
            int distance2 = BitOperations.PopCount(unequal4Squashed[2]);
            int distance3 = BitOperations.PopCount(unequal4Squashed[3]);
            
            if (distance0 < smallestDistance) {
                smallestDistance = distance0;
                index = i;
            }
            
            if (distance1 < smallestDistance) {
                smallestDistance = distance1;
                index = i + 1;
            }
            
            if (distance2 < smallestDistance) {
                smallestDistance = distance2;
                index = i + 2;
            }
            
            if (distance3 < smallestDistance) {
                smallestDistance = distance3;
                index = i + 3;
            }
            
            // Early exit if a complete match is found.
            if (smallestDistance == 0) {
                break;
            }
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
