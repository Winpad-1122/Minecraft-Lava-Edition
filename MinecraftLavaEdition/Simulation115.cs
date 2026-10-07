using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public static class Simulation115
    {
        public static List<Snapshot> Run(WorldBase world)
        {
            var snapshots = new List<Snapshot>();
            snapshots.Add(new Snapshot(0, world.Lava));

            int maxTick = 100000;
            int stableTicks = 0;

            while (world.CurrentTick < maxTick)
            {
                var due = new List<NextTickListEntry>();

                while (world.PendingTicks.Count > 0)
                {
                    var first = world.PendingTicks.Min;
                    if (first == null) break;
                    if (first.ScheduledTime > world.CurrentTick) break;

                    world.PendingTicks.Remove(first);
                    due.Add(first);
                }

                if (due.Count == 0)
                {
                    if (world.PendingTicks.Count == 0) break;

                    var next = world.PendingTicks.Min;
                    if (next == null) break;

                    world.CurrentTick = (int)next.ScheduledTime;
                    continue;
                }

                foreach (var entry in due)
                {
                    var pos = new Position(entry.X, entry.Y, entry.Z);

                    if (!world.Lava.TryGetValue(pos, out int meta)) continue;

                    var oldLava = new Dictionary<Position, int>(world.Lava);
                    var nextLava = new Dictionary<Position, int>(oldLava);

                    world.UpdateTick(pos, meta, oldLava, nextLava);

                    world.Lava.Clear();
                    foreach (var kv in nextLava) world.Lava[kv.Key] = kv.Value;

                    if (world.Lava.ContainsKey(pos) &&
                        !world.PendingTicksContains(pos))
                    {
                        world.ScheduleUpdate(pos, world.TickRate(pos));
                    }

                    NotifyNeighbors(world, pos);
                }

                world.CurrentTick++;

                snapshots.Add(new Snapshot(world.CurrentTick, world.Lava));

                if (snapshots.Count >= 2 &&
                    snapshots[snapshots.Count - 1].SameAs(snapshots[snapshots.Count - 2]))
                    stableTicks++;
                else
                    stableTicks = 0;

                if (stableTicks >= 3) break;
            }

            return snapshots;
        }

        private static void NotifyNeighbors(WorldBase world, Position pos)
        {
            int[] dx = { -1, 1, 0, 0, 0, 0 };
            int[] dy = { 0, 0, -1, 1, 0, 0 };
            int[] dz = { 0, 0, 0, 0, -1, 1 };

            for (int i = 0; i < 6; i++)
            {
                var np = new Position(pos.X + dx[i], pos.Y + dy[i], pos.Z + dz[i]);

                if (world.Lava.ContainsKey(np))
                    world.ScheduleUpdate(np, world.TickRate(np));
            }
        }
    }
}