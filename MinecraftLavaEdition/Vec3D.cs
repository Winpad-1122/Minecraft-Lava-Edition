using System;

namespace MinecraftLavaEdition
{
    public struct Vec3D
    {
        public double X;
        public double Y;
        public double Z;

        public Vec3D(double x, double y, double z) { X = x; Y = y; Z = z; }

        public Vec3D Add(double dx, double dy, double dz)
            => new Vec3D(X + dx, Y + dy, Z + dz);

        public Vec3D Normalize()
        {
            double len = Math.Sqrt(X * X + Y * Y + Z * Z);
            if (len < 1e-9) return new Vec3D(0, 0, 0);
            return new Vec3D(X / len, Y / len, Z / len);
        }

        public override string ToString() => $"({X:F4}, {Y:F4}, {Z:F4})";
    }
}