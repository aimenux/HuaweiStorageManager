using System;

namespace Lib.Models
{
    public sealed class Date : IEquatable<Date>, IComparable
    {
        public Date(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static implicit operator DateTime?(Date date)
        {
            return date?.Value;
        }

        public static implicit operator DateTime(Date date)
        {
            return date?.Value ?? default;
        }

        public static implicit operator Date(DateTime value)
        {
            return new Date(value);
        }

        public static implicit operator Date(DateTime? value)
        {
            return value is null ? null : new Date(value.Value);
        }

        public bool Equals(Date other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Date)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj is Date date)
            {
                return DateTime.Compare(Value, date.Value);
            }

            return 0;
        }
    }
}