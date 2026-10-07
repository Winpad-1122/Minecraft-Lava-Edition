using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public class Snapshot
    {
        public int Tick;
        public Dictionary<Position, int> Lava;

        public Snapshot(int tick, Dictionary<Position, int> lava)
        {
            Tick = tick;
            Lava = new Dictionary<Position, int>(lava);
        }

        public bool SameAs(Snapshot? other)
        {
            if (other == null) return false;
            if (Lava.Count != other.Lava.Count) return false;

            foreach (var kv in Lava)
            {
                if (!other.Lava.TryGetValue(kv.Key, out int v)) return false;
                if (v != kv.Value) return false;
            }
            return true;
        }
    }
}