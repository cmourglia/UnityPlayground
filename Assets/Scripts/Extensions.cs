public static class Extensions
{
    public static float Remap(this float value, float low1, float high1, float low2, float high2)
    {
        return ((value - low1) / (high1 - low1) * (high2 - low2)) + low2;
    }
}
