using Lib.Models;

namespace Lib.Extensions
{
    public static class DateExtensions
    {
        public static string GetValueOrEmpty(this Date date)
        {
            return date?.Value.ToString("F") ?? string.Empty;
        }
    }
}