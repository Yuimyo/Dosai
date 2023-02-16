namespace Dosai.Common.Utils
{
    public static class MathHelper
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (max < value) return max;
            return value;
        }

        public static double Lerp(double min, double max, double current)
        {
            return min + (max - min) * current;
        }
    }
}
