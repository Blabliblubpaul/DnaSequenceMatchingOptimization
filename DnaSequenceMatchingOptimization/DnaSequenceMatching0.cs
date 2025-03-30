namespace DnaSequenceMatchingOptimization;

public static class DnaSequenceMatching0 {
    private static string[] database;
    
    public static void SetupTiny() {
        database = DnaDatabase.LoadDatasetTiny();
    }
    
    public static void SetupSmall() {
        database = DnaDatabase.LoadDatasetSmall();
    }
    
    public static void SetupMedium() {
        database = DnaDatabase.LoadDatasetMedium();
    }
    
    public static void SetupLarge() {
        database = DnaDatabase.LoadDatasetLarge();
    }
    
    public static void SetupHuge() {
        database = DnaDatabase.LoadDatasetHuge();
    }

    public static DnaSequenceMatchResult MatchDnaSequence(string sequence) {
        int smallestDistance = 32;
        int index = 0;
        
        for (int i = 0; i < database.Length; i++) {
            int distance = 0;

            string dbSequence = database[i];
            for (int j = 0; j < 32; j++) {
                if (dbSequence[j] != sequence[j]) {
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
}