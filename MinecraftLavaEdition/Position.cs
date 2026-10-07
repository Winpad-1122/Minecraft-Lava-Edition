using System;

namespace MinecraftLavaEdition
{
    public readonly struct Position : IEquatable<Position>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Position(int x, int y, int z) { X = x; Y = y; Z = z; }

        public bool Equals(Position other)
            => X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object? obj)
            => obj is Position other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static bool operator ==(Position a, Position b) => a.Equals(b);
        public static bool operator !=(Position a, Position b) => !a.Equals(b);

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}