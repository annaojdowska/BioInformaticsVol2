namespace BioInfo
{
    public static class Penalty
    {
        private static double _gapStart = -1;
        private static double _gapExtend = -0.5;

        public static double Value(int gapLength) => _gapStart + _gapExtend*gapLength;
    }
}