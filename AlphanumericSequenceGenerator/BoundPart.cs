using System;

namespace AlphanumericSequenceGenerator
{
    internal struct BoundPart
    {
        public string part;
        public int index;
        public int length;
        public string groupType;

        public BoundPart(string part, int index, int length, string groupType)
        {
            this.part = part;
            this.index = index;
            this.length = length;
            this.groupType = groupType;
        }

        public override bool Equals(object obj)
        {
            return obj is BoundPart other &&
                   part == other.part &&
                   index == other.index &&
                   length == other.length &&
                   groupType == other.groupType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(part, index, length, groupType);
        }

        public void Deconstruct(out string part, out int index, out int length, out string groupType)
        {
            part = this.part;
            index = this.index;
            length = this.length;
            groupType = this.groupType;
        }

        public static implicit operator (string part, int index, int length, string groupType)(BoundPart value)
        {
            return (value.part, value.index, value.length, value.groupType);
        }

        public static implicit operator BoundPart((string part, int index, int length, string groupType) value)
        {
            return new BoundPart(value.part, value.index, value.length, value.groupType);
        }
    }
}
