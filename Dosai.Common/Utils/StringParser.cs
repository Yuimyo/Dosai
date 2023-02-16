namespace Dosai.Common.Utils
{
    public static class StringParser
    {
        public static int ToInt(string value) => int.Parse(value);
        public static uint ToUInt(string value) => uint.Parse(value);
        public static string ToString(string value) => value;
        public static TimeSpan ToTimeSpan(string value) => TimeSpan.Parse(value);
    }
}
