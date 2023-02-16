namespace Dosai.Common.Utils
{
    public static class StringHelper
    {
        public static string Omit(this string text, int maxLength, bool hasDots = true)
        {
            if (hasDots)
                maxLength -= 3;
            if (maxLength < 0) 
                throw new ArgumentOutOfRangeException();
            if (hasDots && text.Length > maxLength)
                return text.Substring(0, Math.Min(text.Length, maxLength)) + "...";
            else
                return text.Substring(0, Math.Min(text.Length, maxLength));
        }
    }
}
