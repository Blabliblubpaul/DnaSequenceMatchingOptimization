using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace DnaSequenceMatchingOptimization;

public static class DnaSequenceMatching1 {
    private static ulong[] database;
    
    public static void SetupTiny() {
        database = new ulong[DnaDatabase.DATASET_TINY_SIZE];
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetTinyLazy()) {
            database[i] = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupSmall() {
        database = new ulong[DnaDatabase.DATASET_SMALL_SIZE];
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetSmallLazy()) {
            database[i] = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupMedium() {
        database = new ulong[DnaDatabase.DATASET_MEDIUM_SIZE];
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetMediumLazy()) {
            database[i] = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupLarge() {
        database = new ulong[DnaDatabase.DATASET_LARGE_SIZE];
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetLargeLazy()) {
            database[i] = SequenceToBinary(sequence);
            i++;
        }
    }
    
    public static void SetupHuge() {
        database = new ulong[DnaDatabase.DATASET_HUGE_SIZE];
        
        int i = 0;
        
        foreach (string sequence in DnaDatabase.LoadDatasetHugeLazy()) {
            database[i] = SequenceToBinary(sequence);
            i++;
        }
    }

    public static DnaSequenceMatchResult MatchDnaSequence(string sequence) {
        ulong binSeq = SequenceToBinary(sequence);
        
        int smallestDistance = 32;
        int index = 0;
        
        for (int i = 0; i < database.Length; i++) {
            int distance = 0;

            ulong unequal = database[i] ^ binSeq;
            
            for (int j = 0; j < 64; j += 2) {
                if ((unequal & (3UL << j)) != 0) {
                    distance++;
                }
            }
            
            if (distance >= smallestDistance) {
                continue;
            }
    
            smallestDistance = distance;
            index = i;
            
            // Early exit if a complete match is found.
            if (distance == 0) {
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
