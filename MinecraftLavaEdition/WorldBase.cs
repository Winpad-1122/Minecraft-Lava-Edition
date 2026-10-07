using System;
using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public abstract class WorldBase
    {
        public Dictionary<Position, int> Lava { get; } = new();
        protected readonly HashSet<Position> solid = new();
        protected readonly HashSet<Position> airLike = new();

        public SortedSet<NextTickListEntry> PendingTicks { get; } = new();

        public int CurrentTick { get; set; } = 0;

        public string VersionName { get; protected set; } = "";
        public long Seed { get; set; } = 12345L;

        public JavaRandom Rand { get; private set; }

        protected WorldBase()
        {
            Rand = new JavaRandom(Seed);
        }

        public void ResetRandom()
        {
            Rand = new JavaRandom(Seed);
        }

        public void AddLavaSource(Position p)
        {
            if (p.Y != -1)
            {
                Lava[p] = 0;
                ScheduleUpdate(p, 30);
            }
        }

        public void AddSolid(Position p) => solid.Add(p);
        public void AddAirLike(Position p) => airLike.Add(p);

        public bool IsSolidMaterial(Position p) => p.Y == -1 || solid.Contains(p);
        public bool IsAirLike(Position p) => p.Y != -1 && airLike.Contains(p);
        public bool IsBlocking(Position p) => p.Y == -1 || solid.Contains(p) || airLike.Contains(p);
        public bool IsLava(Position p) => Lava.ContainsKey(p);
        public int GetMetadata(Position p) => Lava.TryGetValue(p, out int m) ? m : -1;

        public virtual void Tick() { }

        public virtual void UpdateTick(
            Position pos, int meta,
            Dictionary<Position, int> oldLava,
            Dictionary<Position, int> nextLava)
        { }

        public virtual int TickRate(Position pos) => 30;

        public void ScheduleUpdate(Position pos, int delay)
        {
            foreach (var e in PendingTicks)
            {
                if (e.X == pos.X && e.Y == pos.Y && e.Z == pos.Z)
                    return;
            }

            var entry = new NextTickListEntry(pos.X, pos.Y, pos.Z, 0);
            entry.SetScheduledTime(CurrentTick + delay);
            entry.SetPriority(0);
            PendingTicks.Add(entry);
        }

        public bool PendingTicksContains(Position pos)
        {
            foreach (var e in PendingTicks)
            {
                if (e.X == pos.X && e.Y == pos.Y && e.Z == pos.Z)
                    return true;
            }
            return false;
        }

        public abstract Vec3D GetFlowVector(Position pos, Dictionary<Position, int> snapshot);

        public virtual float GetFlowDirection(Position pos, Dictionary<Position, int> snapshot)
        {
            var v = GetFlowVector(pos, snapshot);
            if (v.X == 0 && v.Z == 0) return -1000f;
            return (float)(Math.Atan2(v.Z, v.X) - Math.PI * 0.5);
        }
    }
}