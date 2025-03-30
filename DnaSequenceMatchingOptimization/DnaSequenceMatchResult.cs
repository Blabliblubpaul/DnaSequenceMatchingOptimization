namespace DnaSequenceMatchingOptimization;

public readonly struct DnaSequenceMatchResult {
    public readonly int matchIndex;
    public readonly double matchAccuracy;
    
    public DnaSequenceMatchResult(int matchIndex, double matchAccuracy) {
        this.matchIndex = matchIndex;
        this.matchAccuracy = matchAccuracy;
    }
}