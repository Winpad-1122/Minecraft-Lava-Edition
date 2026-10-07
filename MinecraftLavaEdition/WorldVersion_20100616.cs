using System;
using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public class WorldVersion_20100616 : WorldBase
    {
        private const int FlowStep = 2;

        private int aC = 0;
        private bool[] aD = new bool[4];
        private int[] aE = new int[4];

        public WorldVersion_20100616()
        {
            VersionName = "Infdev 20100616-1808";
        }

        public override void UpdateTick(
            Position pos, int metadata,
            Dictionary<Position, int> oldLava,
            Dictionary<Position, int> nextLava)
        {
            int var14 = metadata;

            if (var14 > 0)
            {
                aC = 0;

                int var6 = H(oldLava, pos.X - 1, pos.Y, pos.Z, -100);
                var6 = H(oldLava, pos.X + 1, pos.Y, pos.Z, var6);
                var6 = H(oldLava, pos.X, pos.Y, pos.Z - 1, var6);
                var6 = H(oldLava, pos.X, pos.Y, pos.Z + 1, var6);

                int var7 = var6 + FlowStep;
                if (var7 >= 8 || var6 < 0) var7 = -1;

                int aboveMeta = F(oldLava, pos.X, pos.Y + 1, pos.Z);
                if (aboveMeta >= 0)
                {
                    var6 = aboveMeta;
                    var7 = var6 >= 8 ? var6 : var6 + 8;
                }

                if (aC >= 2) var7 = 0;

                if (var14 < 8 && var7 < 8 && var7 > var14)
                    var7 = var14;

                if (var7 != var14)
                {
                    var14 = var7;
                    if (var7 < 0) nextLava.Remove(pos);
                    else nextLava[pos] = var7;
                }
                else H(oldLava, pos.X, pos.Y, pos.Z, -100);
            }
            else H(oldLava, pos.X, pos.Y, pos.Z, -100);

            Position below = new Position(pos.X, pos.Y - 1, pos.Z);
            if (CanFlowInto(below))
            {
                int newMeta = var14 >= 8 ? var14 : var14 + 8;
                nextLava[below] = newMeta;
            }
            else
            {
                if (var14 >= 0 && LiquidCanDisplaceBlock(below)
                    && GetMetadataFrom(oldLava, below) != 0)
                {
                    aE = new int[4] { 1000, 1000, 1000, 1000 };
                    int[] dx = { -1, 1, 0, 0 };
                    int[] dz = { 0, 0, -1, 1 };

                    for (int i = 0; i < 4; i++)
                    {
                        int nx = pos.X + dx[i];
                        int nz = pos.Z + dz[i];
                        var np = new Position(nx, pos.Y, nz);
                        var belowNp = new Position(nx, pos.Y - 1, nz);

                        if (!LiquidCanDisplaceBlock(np) &&
                            GetMetadataFrom(oldLava, np) != 0)
                        {
                            if (!LiquidCanDisplaceBlock(belowNp))
                                aE[i] = 0;
                            else
                                aE[i] = GetSmallestFlowDecay(nx, pos.Y, nz, 1, oldLava);
                        }
                    }

                    int minDecay = aE[0];
                    for (int i = 1; i < 4; i++)
                        if (aE[i] < minDecay) minDecay = aE[i];

                    for (int i = 0; i < 4; i++)
                        aD[i] = aE[i] == minDecay;

                    int newDecay = var14 + FlowStep;
                    if (var14 >= 8) newDecay = 1;
                    if (newDecay >= 8) return;

                    for (int i = 0; i < 4; i++)
                    {
                        if (!aD[i]) continue;
                        int nx = pos.X + dx[i];
                        int nz = pos.Z + dz[i];
                        var target = new Position(nx, pos.Y, nz);
                        if (CanFlowInto(target))
                            nextLava[target] = newDecay;
                    }
                }
            }
        }

        private int H(Dictionary<Position, int> snap, int x, int y, int z, int var5)
        {
            int var6 = F(snap, x, y, z);
            if (var6 < 0) return var5;
            if (var6 == 0) aC++;
            if (var6 >= 8) var6 = 0;
            return var5 >= 0 && var6 >= var5 ? var5 : var6;
        }

        private int F(Dictionary<Position, int> snap, int x, int y, int z)
            => snap.TryGetValue(new Position(x, y, z), out int m) ? m : -1;

        private int GetMetadataFrom(Dictionary<Position, int> snap, Position p)
            => snap.TryGetValue(p, out int m) ? m : -1;

        private bool LiquidCanDisplaceBlock(Position p)
        {
            if (!IsSolidMaterial(p) && !IsAirLike(p) && !IsLava(p))
                return false;
            return IsSolidMaterial(p) || IsAirLike(p);
        }

        private bool CanFlowInto(Position p)
        {
            if (p.Y == -1) return false;
            if (IsBlocking(p)) return false;
            if (IsLava(p)) return false;
            return true;
        }

        private int GetSmallestFlowDecay(
            int x, int y, int z, int var5,
            Dictionary<Position, int> oldLava)
        {
            int var6 = 1000;
            int[] dx = { -1, 1, 0, 0 };
            int[] dz = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int nz = z + dz[i];
                var np = new Position(nx, y, nz);
                var belowNp = new Position(nx, y - 1, nz);

                if (!LiquidCanDisplaceBlock(np) &&
                    GetMetadataFrom(oldLava, np) != 0)
                {
                    if (!LiquidCanDisplaceBlock(belowNp))
                        return var5;
                    if (var5 < 4)
                    {
                        int r = GetSmallestFlowDecay(nx, y, nz, var5 + 1, oldLava);
                        if (r < var6) var6 = r;
                    }
                }
            }
            return var6;
        }

        public override Vec3D GetFlowVector(Position pos, Dictionary<Position, int> snapshot)
        {
            var v = new Vec3D(0, 0, 0);
            int current = H(snapshot, pos.X, pos.Y, pos.Z, -100);

            int[] dx = { -1, 0, 1, 0 };
            int[] dz = { 0, -1, 0, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = pos.X + dx[i];
                int nz = pos.Z + dz[i];
                int m = H(snapshot, nx, pos.Y, nz, -100);
                if (m < 0 && !IsBlocking(new Position(nx, pos.Y, nz)))
                    m = 7;
                if (m >= 0)
                {
                    m -= current;
                    v = v.Add(dx[i] * m, 0, dz[i] * m);
                }
            }

            if (snapshot.TryGetValue(pos, out int meta) && meta >= 8)
                v = v.Add(0, -1, 0);

            return v.Normalize();
        }
    }
}