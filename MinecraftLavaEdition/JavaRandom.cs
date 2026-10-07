using System;

namespace MinecraftLavaEdition
{
    public class JavaRandom
    {
        private const long Multiplier = 0x5DEECE66DL;
        private const long Addend = 0xBL;
        private const long Mask = (1L << 48) - 1;

        private long seed;

        public JavaRandom(long seed)
        {
            SetSeed(seed);
        }

        public void SetSeed(long seed)
        {
            this.seed = (seed ^ Multiplier) & Mask;
        }

        private int Next(int bits)
        {
            seed = (seed * Multiplier + Addend) & Mask;
            return (int)((ulong)seed >> (48 - bits));
        }

        public int NextInt()
        {
            return Next(32);
        }

        public int NextInt(int bound)
        {
            if (bound <= 0)
                throw new ArgumentException("bound must be positive");

            if ((bound & -bound) == bound)
                return (int)((bound * (long)Next(31)) >> 31);

            int bits, val;
            do
            {
                bits = Next(31);
                val = bits % bound;
            } while (bits - val + (bound - 1) < 0);

            return val;
        }

        public float NextFloat()
        {
            return Next(24) / (float)(1 << 24);
        }

        public double NextDouble()
        {
            return (((long)Next(26) << 27) + Next(27)) / (double)(1L << 53);
        }

        public bool NextBoolean()
        {
            return Next(1) != 0;
        }
    }
}