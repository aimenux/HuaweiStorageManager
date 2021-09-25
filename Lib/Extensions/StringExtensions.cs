namespace Lib.Extensions
{
    public static class StringExtensions
    {
        public static string GetValueOrEmpty(this string value) => value ?? string.Empty;
    }
}