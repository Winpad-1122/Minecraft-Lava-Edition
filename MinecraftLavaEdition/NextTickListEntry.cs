using System;

namespace MinecraftLavaEdition
{
    public class NextTickListEntry
        : IComparable<NextTickListEntry>, IEquatable<NextTickListEntry>
    {
        private static long nextTickEntryID = 0L;

        public int X, Y, Z;
        public int BlockId;
        public long ScheduledTime;
        public int Priority;
        private long tickEntryID;

        public NextTickListEntry(int x, int y, int z, int blockId)
        {
            X = x; Y = y; Z = z;
            BlockId = blockId;
            tickEntryID = nextTickEntryID++;
        }

        public NextTickListEntry SetScheduledTime(long time)
        {
            ScheduledTime = time;
            return this;
        }

        public void SetPriority(int p)
        {
            Priority = p;
        }

        public bool Equals(NextTickListEntry? other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y && Z == other.Z
                && BlockId == other.BlockId;
        }
        public static void ResetId()
        {
            nextTickEntryID = 0L;
        }

        public override bool Equals(object? obj)
            => obj is NextTickListEntry other && Equals(other);

        public override int GetHashCode()
            => (X * 1024 * 1024 + Z * 1024 + Y) * 256;

        public int CompareTo(NextTickListEntry? other)
        {
            if (other == null) return 1;

            if (ScheduledTime < other.ScheduledTime) return -1;
            if (ScheduledTime > other.ScheduledTime) return 1;

            if (Priority != other.Priority)
                return Priority - other.Priority;

            if (tickEntryID < other.tickEntryID) return -1;
            if (tickEntryID > other.tickEntryID) return 1;

            return 0;
        }

        public override string ToString()
            => $"{BlockId}: ({X}, {Y}, {Z}), {ScheduledTime}, {Priority}, {tickEntryID}";
    }
}