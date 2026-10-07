using System;
using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public class WorldVersion_1_15 : WorldBase
    {
        public bool IsNether { get; set; } = false;

        public WorldVersion_1_15()
        {
            VersionName = "18w10c(1.13) ~ 22w03a(1.18.2)";
        }

        private int GetDropOff() => IsNether ? 1 : 2;

        private int GetSlopeFindDistance() => IsNether ? 4 : 2;

        private int GetTickDelay() => IsNether ? 10 : 30;

        public override void UpdateTick(
            Position pos, int var6,
            Dictionary<Position, int> oldLava,
            Dictionary<Position, int> nextLava)
        {
            bool isSource = (var6 == 0);

            FluidState115 fluidState;

            if (isSource)
            {
                fluidState = new FluidState115
                {
                    IsEmpty = false,
                    Amount = 8,
                    Falling = false,
                    Metadata = 0,
                };
            }
            else
            {
                fluidState = GetNewLiquid(pos, var6, oldLava);

                if (fluidState.IsEmpty)
                {
                    nextLava.Remove(pos);
                    return;
                }

                if (fluidState.Metadata != var6)
                {
                    nextLava[pos] = fluidState.Metadata;
                }
            }

            Spread(pos, fluidState, oldLava, nextLava);

            if (nextLava.ContainsKey(pos))
                ScheduleUpdate(pos, GetTickDelay());
        }
        private FluidState115 GetNewLiquid(
            Position pos, int currentMeta,
            Dictionary<Position, int> oldLava)
        {
            int highestNeighbor = 0;
            int neighborSources = 0;

            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };

            for (int i = 0; i < 4; i++)
            {
                var np = new Position(pos.X + dx[i], pos.Y, pos.Z + dz[i]);
                int m = GetMeta(oldLava, np);
                if (m < 0) continue;
                if (!CanPassThroughWall(np, pos, oldLava)) continue;

                if (m == 0) neighborSources++;
                highestNeighbor = Math.Max(highestNeighbor, GetAmount(m));
            }

            var above = new Position(pos.X, pos.Y + 1, pos.Z);
            int aboveM = GetMeta(oldLava, above);
            if (aboveM >= 0 && CanPassThroughWall(above, pos, oldLava))
            {
                return new FluidState115
                {
                    IsEmpty = false,
                    Amount = 8,
                    Falling = true,
                    Metadata = 8,
                };
            }

            int newAmount = highestNeighbor - GetDropOff();
            if (newAmount <= 0)
                return new FluidState115 { IsEmpty = true };

            return new FluidState115
            {
                IsEmpty = false,
                Amount = newAmount,
                Falling = false,
                Metadata = 8 - newAmount,
            };
        }
        private void Spread(
            Position pos, FluidState115 fluidState,
            Dictionary<Position, int> oldLava,
            Dictionary<Position, int> nextLava)
        {
            if (fluidState.IsEmpty) return;

            var below = new Position(pos.X, pos.Y - 1, pos.Z);
            if (CanSpreadTo(pos, below, oldLava))
            {
                var newBelow = GetNewLiquid(below, GetMeta(oldLava, below), oldLava);
                if (!newBelow.IsEmpty)
                {
                    nextLava[below] = newBelow.Metadata;
                }

                if (SourceNeighborCount(pos, oldLava) >= 3)
                    SpreadToSides(pos, fluidState, oldLava, nextLava);
                return;
            }

            if (fluidState.Amount == 8 && !fluidState.Falling)
            {
                SpreadToSides(pos, fluidState, oldLava, nextLava);
            }
            else if (!IsWaterHole(pos, below, oldLava))
            {
                SpreadToSides(pos, fluidState, oldLava, nextLava);
            }
        }

        private void SpreadToSides(
            Position pos, FluidState115 fluidState,
            Dictionary<Position, int> oldLava,
            Dictionary<Position, int> nextLava)
        {
            int newAmount = fluidState.Amount - GetDropOff();
            if (fluidState.Falling) newAmount = 7;
            if (newAmount <= 0) return;

            var spread = GetSpread(pos, fluidState, oldLava);

            foreach (var kv in spread)
            {
                var target = kv.Key;
                var targetState = kv.Value;

                if (CanSpreadTo(pos, target, oldLava))
                    nextLava[target] = targetState.Metadata;
            }
        }

        private Dictionary<Position, FluidState115> GetSpread(
            Position pos, FluidState115 fluidState,
            Dictionary<Position, int> oldLava)
        {
            int lowest = 1000;
            var result = new Dictionary<Position, FluidState115>();

            var context = new SpreadContext(this, pos);

            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };

            for (int i = 0; i < 4; i++)
            {
                var np = new Position(pos.X + dx[i], pos.Y, pos.Z + dz[i]);
                if (!CanMaybePassThrough(pos, np, oldLava)) continue;

                var newState = GetNewLiquid(np, GetMeta(oldLava, np), oldLava);
                if (newState.IsEmpty) continue;

                int distance;
                if (context.IsHole(np, oldLava))
                    distance = 0;
                else
                    distance = GetSlopeDistance(np, 1, i, oldLava, context);

                if (distance < lowest)
                    result.Clear();

                if (distance <= lowest)
                {
                    result[np] = newState;
                    lowest = distance;
                }
            }

            return result;
        }

        private int GetSlopeDistance(
            Position pos, int pass, int fromDir,
            Dictionary<Position, int> oldLava,
            SpreadContext context)
        {
            int lowest = 1000;

            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };

            for (int dir = 0; dir < 4; dir++)
            {
                if (dir == fromDir) continue;

                var np = new Position(pos.X + dx[dir], pos.Y, pos.Z + dz[dir]);
                if (!CanPassThrough(pos, np, oldLava)) continue;

                if (context.IsHole(np, oldLava))
                    return pass;

                if (pass < GetSlopeFindDistance())
                {
                    int r = GetSlopeDistance(np, pass + 1, Opp(dir), oldLava, context);
                    if (r < lowest) lowest = r;
                }
            }

            return lowest;
        }

        private static int Opp(int dir) => (dir + 2) % 4;

        private int GetMeta(Dictionary<Position, int> snap, Position p)
            => snap.TryGetValue(p, out int m) ? m : -1;

        private int GetAmount(int metadata)
        {
            if (metadata == 0) return 8;
            if (metadata >= 8) return 8;
            return 8 - metadata;
        }

        private bool CanPassThroughWall(Position a, Position b, Dictionary<Position, int> oldLava)
        {
            if (IsSolidMaterial(a) || IsSolidMaterial(b)) return false;
            return true;
        }

        private bool CanMaybePassThrough(Position from, Position to, Dictionary<Position, int> oldLava)
        {
            if (IsSourceBlock(to, oldLava)) return false;
            if (!CanHoldFluid(to)) return false;
            if (!CanPassThroughWall(from, to, oldLava)) return false;
            return true;
        }

        private bool CanPassThrough(Position from, Position to, Dictionary<Position, int> oldLava)
        {
            if (IsSourceBlock(to, oldLava)) return false;
            if (!CanPassThroughWall(from, to, oldLava)) return false;
            if (!CanHoldFluid(to)) return false;
            return true;
        }

        private bool CanSpreadTo(Position from, Position to, Dictionary<Position, int> oldLava)
        {
            if (IsLava(to)) return false;
            if (IsSolidMaterial(to)) return false;
            if (IsAirLike(to)) return false;
            return true;
        }

        private bool IsSourceBlock(Position p, Dictionary<Position, int> oldLava)
        {
            int m = GetMeta(oldLava, p);
            return m == 0;
        }

        private bool CanHoldFluid(Position p)
        {
            if (IsSolidMaterial(p)) return false;
            if (IsAirLike(p)) return false;
            return true;
        }

        internal bool IsWaterHole(Position top, Position bottom, Dictionary<Position, int> oldLava)
        {
            if (!CanPassThroughWall(top, bottom, oldLava)) return false;
            if (GetMeta(oldLava, bottom) >= 0) return true;
            return CanHoldFluid(bottom);
        }

        private int SourceNeighborCount(Position pos, Dictionary<Position, int> oldLava)
        {
            int count = 0;
            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };
            for (int i = 0; i < 4; i++)
            {
                var np = new Position(pos.X + dx[i], pos.Y, pos.Z + dz[i]);
                if (GetMeta(oldLava, np) == 0) count++;
            }
            return count;
        }

        public override Vec3D GetFlowVector(Position pos, Dictionary<Position, int> snapshot)
        {
            double flowX = 0, flowZ = 0;

            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };

            int currentAmount = GetAmount(GetMeta(snapshot, pos));
            double currentHeight = currentAmount / 9.0;

            for (int i = 0; i < 4; i++)
            {
                var np = new Position(pos.X + dx[i], pos.Y, pos.Z + dz[i]);
                int m = GetMeta(snapshot, np);
                if (m < 0 && !IsSolidMaterial(np) && !IsAirLike(np))
                {
                    var below = new Position(np.X, np.Y - 1, np.Z);
                    int bm = GetMeta(snapshot, below);
                    if (bm >= 0)
                    {
                        double belowHeight = GetAmount(bm) / 9.0;
                        double distance = currentHeight - belowHeight - 0.8888889;
                        if (distance != 0)
                        {
                            flowX += dx[i] * distance;
                            flowZ += dz[i] * distance;
                        }
                    }
                }
                else if (m >= 0)
                {
                    double neighborHeight = GetAmount(m) / 9.0;
                    double distance = currentHeight - neighborHeight;
                    if (distance != 0)
                    {
                        flowX += dx[i] * distance;
                        flowZ += dz[i] * distance;
                    }
                }
            }

            var v = new Vec3D(flowX, 0, flowZ);

            if (GetMeta(snapshot, pos) >= 8)
            {
                bool blocked = false;
                blocked |= IsBlocking(new Position(pos.X, pos.Y, pos.Z - 1));
                blocked |= IsBlocking(new Position(pos.X, pos.Y, pos.Z + 1));
                blocked |= IsBlocking(new Position(pos.X - 1, pos.Y, pos.Z));
                blocked |= IsBlocking(new Position(pos.X + 1, pos.Y, pos.Z));
                blocked |= IsBlocking(new Position(pos.X, pos.Y + 1, pos.Z - 1));
                blocked |= IsBlocking(new Position(pos.X, pos.Y + 1, pos.Z + 1));
                blocked |= IsBlocking(new Position(pos.X - 1, pos.Y + 1, pos.Z));
                blocked |= IsBlocking(new Position(pos.X + 1, pos.Y + 1, pos.Z));

                if (blocked)
                    v = v.Normalize().Add(0, -6, 0);
            }

            return v.Normalize();
        }

        internal class SpreadContext
        {
            private readonly WorldVersion_1_15 owner;
            private readonly Position origin;

            private readonly Dictionary<Position, bool> holeCache = new();

            public SpreadContext(WorldVersion_1_15 owner, Position origin)
            {
                this.owner = owner;
                this.origin = origin;
            }

            public bool IsHole(Position pos, Dictionary<Position, int> oldLava)
            {
                if (holeCache.TryGetValue(pos, out bool h)) return h;

                var below = new Position(pos.X, pos.Y - 1, pos.Z);
                bool result = owner.IsWaterHole(pos, below, oldLava);

                holeCache[pos] = result;
                return result;
            }
        }
    }

    public class FluidState115
    {
        public bool IsEmpty;
        public int Amount;
        public bool Falling;
        public int Metadata;
    }
}