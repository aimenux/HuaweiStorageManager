using System;

namespace Lib.Extensions
{
    public static class IntegerExtensions
    {
        private enum SizeUnits
        {
            Bytes, Kilobytes, Megabytes, Gigabytes
        }

        private static string ToSize(this long value, SizeUnits unit)
        {
            var size = (value / Math.Pow(1024, (int)unit)).ToString("0.00");
            return $"{size} {unit}";
        }

        public static string FriendlySize(this long value)
        {
            return value switch
            {
                < 1024 => ToSize(value, SizeUnits.Bytes),
                < 1024 * 1024 => ToSize(value, SizeUnits.Kilobytes),
                _ => ToSize(value, SizeUnits.Megabytes)
            };
        }
    }
}
