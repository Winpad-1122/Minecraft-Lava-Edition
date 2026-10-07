using System;
using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public class WorldVersion_Alpha_1_2_2 : WorldBase
    {
        private int numAdjacentSources = 0;
        private bool[] isOptimalFlowDirection = new bool[4];
        private int[] flowCost = new int[4];

        public bool IsNether { get; set; } = false;

        public WorldVersion_Alpha_1_2_2()
        {
            VersionName = "Alpha 1.2.2 ~ Beta 1.9-pre6";
        }

        public override void UpdateTick(
            Position pos, int var6,
            Dictionary<Position, int> oldLava,
            Dictionary<Position, int> nextLava)
        {
            int var7 = 1;
            if (!IsNether) var7 = 2;

            if (var6 > 0)
            {
                int var8 = -100;
                numAdjacentSources = 0;

                int var11 = GetSmallestFlowDecay(oldLava, pos.X - 1, pos.Y, pos.Z, var8);
                var11 = GetSmallestFlowDecay(oldLava, pos.X + 1, pos.Y, pos.Z, var11);
                var11 = GetSmallestFlowDecay(oldLava, pos.X, pos.Y, pos.Z - 1, var11);
                var11 = GetSmallestFlowDecay(oldLava, pos.X, pos.Y, pos.Z + 1, var11);

                int var9 = var11 + var7;
                if (var9 >= 8 || var11 < 0) var9 = -1;

                int aboveMeta = F(oldLava, pos.X, pos.Y + 1, pos.Z);
                if (aboveMeta >= 0)
                {
                    if (aboveMeta >= 8) var9 = aboveMeta;
                    else var9 = aboveMeta + 8;
                }

                if (var6 < 8 && var9 < 8 && var9 > var6 && Rand.NextInt(4) != 0)
                {
                    var9 = var6;
                }

                if (var9 != var6)
                {
                    var6 = var9;
                    if (var9 < 0) nextLava.Remove(pos);
                    else nextLava[pos] = var9;
                }
            }

            Position below = new Position(pos.X, pos.Y - 1, pos.Z);
            if (LiquidCanDisplaceBlock(oldLava, below))
            {
                int newMeta = var6 >= 8 ? var6 : var6 + 8;
                nextLava[below] = newMeta;
            }
            else if (var6 >= 0 && (var6 == 0 || BlockBlocksFlow(below)))
            {
                bool[] var12 = GetOptimalFlowDirections(oldLava, pos);

                int var9 = var6 + var7;
                if (var6 >= 8) var9 = 1;
                if (var9 >= 8) return;

                int[] dx = { -1, 1, 0, 0 };
                int[] dz = { 0, 0, -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    if (!var12[i]) continue;
                    int nx = pos.X + dx[i];
                    int nz = pos.Z + dz[i];
                    var target = new Position(nx, pos.Y, nz);
                    if (LiquidCanDisplaceBlock(oldLava, target))
                        nextLava[target] = var9;
                }
            }
        }

        private bool[] GetOptimalFlowDirections(
            Dictionary<Position, int> oldLava, Position pos)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dz = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                flowCost[i] = 1000;

                int nx = pos.X + dx[i];
                int nz = pos.Z + dz[i];
                var np = new Position(nx, pos.Y, nz);
                var belowNp = new Position(nx, pos.Y - 1, nz);

                if (!BlockBlocksFlow(np) &&
                    GetMetadataFrom(oldLava, np) != 0)
                {
                    if (!BlockBlocksFlow(belowNp))
                        flowCost[i] = 0;
                    else
                        flowCost[i] = CalculateFlowCost(oldLava, nx, pos.Y, nz, 1, i);
                }
            }

            int minCost = flowCost[0];
            for (int i = 1; i < 4; i++)
                if (flowCost[i] < minCost) minCost = flowCost[i];

            for (int i = 0; i < 4; i++)
                isOptimalFlowDirection[i] = flowCost[i] == minCost;

            return isOptimalFlowDirection;
        }

        private int CalculateFlowCost(
            Dictionary<Position, int> oldLava,
            int x, int y, int z, int depth, int fromDir)
        {
            int best = 1000;
            int[] dx = { -1, 1, 0, 0 };
            int[] dz = { 0, 0, -1, 1 };

            for (int dir = 0; dir < 4; dir++)
            {
                if ((dir == 0 && fromDir == 1) || (dir == 1 && fromDir == 0) ||
                    (dir == 2 && fromDir == 3) || (dir == 3 && fromDir == 2))
                    continue;

                int nx = x + dx[dir];
                int nz = z + dz[dir];
                var np = new Position(nx, y, nz);
                var belowNp = new Position(nx, y - 1, nz);

                if (!BlockBlocksFlow(np) &&
                    (GetMetadataFrom(oldLava, np) != 0))
                {
                    if (!BlockBlocksFlow(belowNp))
                        return depth;
                    if (depth < 4)
                    {
                        int r = CalculateFlowCost(oldLava, nx, y, nz, depth + 1, dir);
                        if (r < best) best = r;
                    }
                }
            }
            return best;
        }

        protected int GetSmallestFlowDecay(
            Dictionary<Position, int> oldLava, int x, int y, int z, int var5)
        {
            int var6 = F(oldLava, x, y, z);
            if (var6 < 0) return var5;
            if (var6 == 0) numAdjacentSources++;
            if (var6 >= 8) var6 = 0;
            return var5 >= 0 && var6 >= var5 ? var5 : var6;
        }

        private bool BlockBlocksFlow(Position p)
        {
            if (IsSolidMaterial(p)) return true;
            if (IsAirLike(p)) return true;
            return false;
        }

        private bool LiquidCanDisplaceBlock(
            Dictionary<Position, int> snap, Position p)
        {
            if (IsLava(p)) return false;
            if (BlockBlocksFlow(p)) return false;
            return true;
        }

        private int F(Dictionary<Position, int> snap, int x, int y, int z)
            => snap.TryGetValue(new Position(x, y, z), out int m) ? m : -1;

        private int GetMetadataFrom(Dictionary<Position, int> snap, Position p)
            => snap.TryGetValue(p, out int m) ? m : -1;

        public override Vec3D GetFlowVector(Position pos, Dictionary<Position, int> snapshot)
        {
            var v = new Vec3D(0, 0, 0);
            int var6 = EffectiveFlowDecay(snapshot, pos);

            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = pos.X + dx[i];
                int nz = pos.Z + dz[i];

                int var10 = EffectiveFlowDecay(snapshot, new Position(nx, pos.Y, nz));
                if (var10 < 0)
                {
                    if (!IsSolidMaterial(new Position(nx, pos.Y, nz)))
                    {
                        var10 = EffectiveFlowDecay(snapshot, new Position(nx, pos.Y - 1, nz));
                        if (var10 >= 0)
                        {
                            var10 -= var6 - 8;
                            v = v.Add(dx[i] * var10, 0, dz[i] * var10);
                        }
                    }
                }
                else
                {
                    var10 -= var6;
                    v = v.Add(dx[i] * var10, 0, dz[i] * var10);
                }
            }

            if (snapshot.TryGetValue(pos, out int meta) && meta >= 8)
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

        private int EffectiveFlowDecay(Dictionary<Position, int> snap, Position p)
        {
            if (!snap.TryGetValue(p, out int m)) return -1;
            if (m >= 8) m = 0;
            return m;
        }
    }
}