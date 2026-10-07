using System;
using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public class CornerHeights
    {
        public float NW;
        public float NE;
        public float SE;
        public float SW;

        public override string ToString()
            => $"NW={NW:F3}, NE={NE:F3}, SE={SE:F3}, SW={SW:F3}";
    }

    public class LavaRenderInfo
    {
        public Position Position;
        public int Metadata;
        public bool IsFalling;

        public float SurfaceHeight;
        public CornerHeights Corners = new CornerHeights();

        public float[][] TopCorners = new float[4][];

        public float FlowX;
        public float FlowY;
        public float FlowZ;

        public float FlowAngle;
        public string FlowDirection = "静止";

        public string TopFaceUV = "";
        public string[] SideFaceUV = new string[4];

        public float[][] SideFaceUVValues = new float[4][];

        public List<string> Notes = new List<string>();
        public string[] CornerDetails = new string[4];

        public bool UseRC1Offset = false;
        public bool UseModernBottomOffset = false;
    }

    public static class LavaRenderInfoBuilder
    {
        public static LavaRenderInfo Build(WorldBase world, Position pos)
            => Build(world, pos, world.Lava);

        public static LavaRenderInfo Build(
            WorldBase world, Position pos,
            Dictionary<Position, int> lavaSnapshot)
        {
            var info = new LavaRenderInfo
            {
                Position = pos,
                Metadata = lavaSnapshot.TryGetValue(pos, out int m) ? m : -1,
            };

            if (info.Metadata < 0) return info;

            info.IsFalling = info.Metadata >= 8;

            if (world is WorldVersion_1_0_0_rc1)
                info.UseRC1Offset = true;

            if (world is WorldVersion_Modern)
                info.UseModernBottomOffset = true;

            if (world is WorldVersion_Modern)
            {
                GetCornersModern(world, pos, lavaSnapshot,
                    out float nw, out float ne, out float se, out float sw,
                    out string detail);

                info.Corners.NW = nw;
                info.Corners.NE = ne;
                info.Corners.SE = se;
                info.Corners.SW = sw;

                info.CornerDetails[0] = "NW: " + detail;
                info.CornerDetails[1] = "NE: " + detail;
                info.CornerDetails[2] = "SE: " + detail;
                info.CornerDetails[3] = "SW: " + detail;
            }
            else
            {
                info.Corners.NW = GetFluidHeight(world, pos.X, pos.Y, pos.Z, lavaSnapshot, out var dNW);
                info.Corners.NE = GetFluidHeight(world, pos.X + 1, pos.Y, pos.Z, lavaSnapshot, out var dNE);
                info.Corners.SE = GetFluidHeight(world, pos.X + 1, pos.Y, pos.Z + 1, lavaSnapshot, out var dSE);
                info.Corners.SW = GetFluidHeight(world, pos.X, pos.Y, pos.Z + 1, lavaSnapshot, out var dSW);

                info.CornerDetails[0] = "NW: " + dNW;
                info.CornerDetails[1] = "NE: " + dNE;
                info.CornerDetails[2] = "SE: " + dSE;
                info.CornerDetails[3] = "SW: " + dSW;
            }

            info.SurfaceHeight = (info.Corners.NW + info.Corners.NE
                                + info.Corners.SE + info.Corners.SW) / 4f;

            float[][] corners =
            {
                new[] { (float)pos.X,     pos.Y + info.Corners.NW, (float)pos.Z     },
                new[] { (float)pos.X,     pos.Y + info.Corners.SW, (float)pos.Z + 1 },
                new[] { (float)pos.X + 1, pos.Y + info.Corners.SE, (float)pos.Z + 1 },
                new[] { (float)pos.X + 1, pos.Y + info.Corners.NE, (float)pos.Z     },
            };

            if (info.UseRC1Offset)
            {
                float cx = pos.X + 0.5f;
                float cz = pos.Z + 0.5f;
                float off = 0.001f;

                for (int i = 0; i < 4; i++)
                {
                    float dx = corners[i][0] - cx;
                    float dz = corners[i][2] - cz;
                    float len = (float)Math.Sqrt(dx * dx + dz * dz);
                    if (len > 1e-6f)
                    {
                        float scale = (len - off) / len;
                        corners[i][0] = cx + dx * scale;
                        corners[i][2] = cz + dz * scale;
                    }
                }
            }

            info.TopCorners[0] = corners[0];
            info.TopCorners[1] = corners[1];
            info.TopCorners[2] = corners[2];
            info.TopCorners[3] = corners[3];

            var flowVec = world.GetFlowVector(pos, lavaSnapshot);
            info.FlowX = (float)flowVec.X;
            info.FlowY = (float)flowVec.Y;
            info.FlowZ = (float)flowVec.Z;
            info.FlowAngle = world.GetFlowDirection(pos, lavaSnapshot);
            info.FlowDirection = DescribeFlow(flowVec);

            info.TopFaceUV = GetTopUV(info.FlowAngle, world);
            BuildSideUVs(info, world);

            if (info.IsFalling) info.Notes.Add("下落状态（metadata >= 8）。");
            if (world.IsSolidMaterial(new Position(pos.X, pos.Y + 1, pos.Z)))
                info.Notes.Add("上方是石头，顶面不渲染。");
            if (world.IsAirLike(new Position(pos.X, pos.Y + 1, pos.Z)))
                info.Notes.Add("上方是类空气，阻挡流体但计入高度。");

            return info;
        }
        private static void GetCornersModern(
            WorldBase world, Position pos,
            Dictionary<Position, int> snapshot,
            out float nw, out float ne, out float se, out float sw,
            out string detail)
        {
            float heightSelf = GetHeightModern(world, pos, snapshot);
            if (heightSelf >= 1.0f)
            {
                nw = ne = se = sw = 1.0f;
                detail = "现代：当前格高度 1.0，四角直接 1.0";
                return;
            }

            float heightNorth = GetHeightModern(world, new Position(pos.X, pos.Y, pos.Z - 1), snapshot);
            float heightSouth = GetHeightModern(world, new Position(pos.X, pos.Y, pos.Z + 1), snapshot);
            float heightEast = GetHeightModern(world, new Position(pos.X + 1, pos.Y, pos.Z), snapshot);
            float heightWest = GetHeightModern(world, new Position(pos.X - 1, pos.Y, pos.Z), snapshot);

            float cornerNW = GetHeightModern(world, new Position(pos.X - 1, pos.Y, pos.Z - 1), snapshot);
            float cornerNE = GetHeightModern(world, new Position(pos.X + 1, pos.Y, pos.Z - 1), snapshot);
            float cornerSE = GetHeightModern(world, new Position(pos.X + 1, pos.Y, pos.Z + 1), snapshot);
            float cornerSW = GetHeightModern(world, new Position(pos.X - 1, pos.Y, pos.Z + 1), snapshot);

            nw = CalculateAverageHeight(heightSelf, heightNorth, heightWest, cornerNW);
            ne = CalculateAverageHeight(heightSelf, heightNorth, heightEast, cornerNE);
            se = CalculateAverageHeight(heightSelf, heightSouth, heightEast, cornerSE);
            sw = CalculateAverageHeight(heightSelf, heightSouth, heightWest, cornerSW);

            detail = $"现代：self={heightSelf:F3}, N={heightNorth:F3}, S={heightSouth:F3}, E={heightEast:F3}, W={heightWest:F3}";
        }

        private static float CalculateAverageHeight(
            float heightSelf, float height1, float height2, float heightCorner)
        {
            if (height1 >= 1.0f || height2 >= 1.0f)
                return 1.0f;

            float total = 0f;
            float weight = 0f;
            if (height1 > 0f || height2 > 0f)
            {
                if (heightCorner >= 1.0f)
                    return 1.0f;

                AddWeightedHeight(ref total, ref weight, heightCorner);
            }

            AddWeightedHeight(ref total, ref weight, heightSelf);
            AddWeightedHeight(ref total, ref weight, height1);
            AddWeightedHeight(ref total, ref weight, height2);

            if (weight == 0f) return 1.0f;
            return total / weight;
        }

        private static void AddWeightedHeight(ref float total, ref float weight, float h)
        {
            if (h >= 0.8f)
            {
                total += h * 10f;
                weight += 10f;
            }
            else if (h >= 0f)
            {
                total += h;
                weight += 1f;
            }
        }

        private static float GetHeightModern(
            WorldBase world, Position pos,
            Dictionary<Position, int> snapshot)
        {
            int m = snapshot.TryGetValue(pos, out int mv) ? mv : -1;

            if (m >= 0)
            {
                var above = new Position(pos.X, pos.Y + 1, pos.Z);
                if (snapshot.ContainsKey(above)) return 1.0f;

                int amount = m == 0 ? 8 : (m >= 8 ? 8 : 8 - m);
                return amount / 9.0f;
            }

            if (world.IsSolidMaterial(pos)) return -1.0f;
            return 0.0f;
        }
        private static float GetFluidHeight(
            WorldBase world, int x, int y, int z,
            Dictionary<Position, int> snapshot,
            out string detail)
        {
            if (world is WorldVersion_1_8)
                return GetFluidHeight18(world, x, y, z, snapshot, out detail);

            if (world is WorldVersion_1_15)
                return GetFluidHeight115(world, x, y, z, snapshot, out detail);

            return GetFluidHeightOld(world, x, y, z, snapshot, out detail);
        }

        private static float GetFluidHeightOld(
            WorldBase world, int x, int y, int z,
            Dictionary<Position, int> snapshot,
            out string detail)
        {
            int count = 0;
            float total = 0f;

            int solidCount = 0;
            int airLikeCount = 0;
            int fluidCount = 0;
            int airCount = 0;

            for (int i = 0; i < 4; i++)
            {
                int nx = x - (i & 1);
                int nz = z - ((i >> 1) & 1);

                Position above = new Position(nx, y + 1, nz);
                if (snapshot.ContainsKey(above))
                {
                    detail = "上方同种流体，高度 1.0";
                    return 1.0f;
                }

                Position np = new Position(nx, y, nz);
                int m = snapshot.TryGetValue(np, out int mv) ? mv : -1;

                if (m < 0)
                {
                    if (world.IsSolidMaterial(np)) solidCount++;
                    else if (world.IsAirLike(np))
                    {
                        total += 1f; count += 1; airLikeCount++;
                    }
                    else
                    {
                        total += 1f; count += 1; airCount++;
                    }
                }
                else
                {
                    if (m >= 8 || m == 0)
                    {
                        total += GetDecayHeight(m) * 10f;
                        count += 10;
                    }
                    total += GetDecayHeight(m);
                    count += 1;
                    fluidCount++;
                }
            }

            int seed = x * 3111 + z * 31827;
            float jitter = (float)(((seed * seed * 31287123 + seed * 318127) & 7)) / 7.0f / 9.0f;

            float height = count == 0 ? 1.0f : 1.0f - total / count + jitter;

            detail = $"旧版：石头={solidCount}, 类空气={airLikeCount}, 空气={airCount}, 流体={fluidCount}, 高度={height:F3}";
            return height;
        }

        private static float GetFluidHeight18(
            WorldBase world, int x, int y, int z,
            Dictionary<Position, int> snapshot,
            out string detail)
        {
            int count = 0;
            float total = 0f;

            int solidCount = 0;
            int airLikeCount = 0;
            int fluidCount = 0;
            int airCount = 0;

            for (int i = 0; i < 4; i++)
            {
                int nx = x - (i & 1);
                int nz = z - ((i >> 1) & 1);

                Position above = new Position(nx, y + 1, nz);
                if (snapshot.ContainsKey(above))
                {
                    detail = "上方同种流体，高度 1.0";
                    return 1.0f;
                }

                Position np = new Position(nx, y, nz);
                int m = snapshot.TryGetValue(np, out int mv) ? mv : -1;

                if (m < 0)
                {
                    if (world.IsSolidMaterial(np)) solidCount++;
                    else if (world.IsAirLike(np))
                    {
                        total += 1f; count += 1; airLikeCount++;
                    }
                    else
                    {
                        total += 1f; count += 1; airCount++;
                    }
                }
                else
                {
                    if (m >= 8 || m == 0)
                    {
                        total += GetDecayHeight(m) * 10f;
                        count += 10;
                    }
                    total += GetDecayHeight(m);
                    count += 1;
                    fluidCount++;
                }
            }

            float height = count == 0 ? 1.0f : 1.0f - total / count;

            detail = $"1.8：石头={solidCount}, 类空气={airLikeCount}, 空气={airCount}, 流体={fluidCount}, 高度={height:F3}";
            return height;
        }

        private static float GetFluidHeight115(
            WorldBase world, int x, int y, int z,
            Dictionary<Position, int> snapshot,
            out string detail)
        {
            int count = 0;
            float total = 0f;

            int fluidCount = 0;
            int airCount = 0;

            for (int i = 0; i < 4; i++)
            {
                int nx = x - (i & 1);
                int nz = z - ((i >> 1) & 1);

                Position above = new Position(nx, y + 1, nz);
                if (snapshot.ContainsKey(above))
                {
                    detail = "上方同种流体，高度 1.0";
                    return 1.0f;
                }

                Position np = new Position(nx, y, nz);
                int m = snapshot.TryGetValue(np, out int mv) ? mv : -1;

                if (m >= 0)
                {
                    int amount = m == 0 ? 8 : (m >= 8 ? 8 : 8 - m);
                    float h = amount / 9.0f;

                    if (h >= 0.8f)
                    {
                        total += h * 10f;
                        count += 10;
                    }
                    else
                    {
                        total += h;
                        count += 1;
                    }
                    fluidCount++;
                }
                else if (!world.IsSolidMaterial(np) && !world.IsAirLike(np))
                {
                    count += 1;
                    airCount++;
                }
            }

            if (count == 0)
            {
                detail = "无邻居，高度 1.0";
                return 1.0f;
            }

            float height = total / count;
            detail = $"1.15：流体={fluidCount}, 空气={airCount}, total={total:F3}, count={count}, 高度={height:F3}";
            return height;
        }

        private static float GetDecayHeight(int metadata)
        {
            if (metadata >= 8) metadata = 0;
            return (metadata + 1) / 9f;
        }

        private static void BuildSideUVs(LavaRenderInfo info, WorldBase world)
        {
            float[] topHeight = new float[4];
            float[] bottomHeight = new float[4];

            topHeight[0] = info.Corners.NW;
            bottomHeight[0] = info.Corners.NE;

            topHeight[1] = info.Corners.SW;
            bottomHeight[1] = info.Corners.SE;

            topHeight[2] = info.Corners.NW;
            bottomHeight[2] = info.Corners.SW;

            topHeight[3] = info.Corners.NE;
            bottomHeight[3] = info.Corners.SE;

            bool is115 = (world is WorldVersion_1_15) || (world is WorldVersion_Modern);

            for (int i = 0; i < 4; i++)
            {
                float[] uv = new float[8];

                if (is115)
                {
                    float u0 = 0f;
                    float u1 = 8f / 16f;
                    float v0 = (1f - topHeight[i]) * 16f * 0.5f / 16f;
                    float v1 = (1f - bottomHeight[i]) * 16f * 0.5f / 16f;
                    float v2 = 8f / 16f;

                    uv[0] = u0; uv[1] = v0;
                    uv[2] = u1; uv[3] = v1;
                    uv[4] = u1; uv[5] = v2;
                    uv[6] = u0; uv[7] = v2;
                }
                else
                {
                    float u0 = 0f;
                    float u1 = 1f - 0.01f / 16f;
                    float v0 = (1f - topHeight[i]) / 16f;
                    float v1 = (1f - bottomHeight[i]) / 16f;
                    float v2 = 1f - 0.01f / 16f;

                    uv[0] = u0; uv[1] = v0;
                    uv[2] = u1; uv[3] = v1;
                    uv[4] = u1; uv[5] = v2;
                    uv[6] = u0; uv[7] = v2;
                }

                info.SideFaceUVValues[i] = uv;
            }

            info.SideFaceUV[0] = "北面 UV：u 沿 +x，v 沿 -y";
            info.SideFaceUV[1] = "南面 UV：u 沿 -x，v 沿 -y";
            info.SideFaceUV[2] = "西面 UV：u 沿 +z，v 沿 -y";
            info.SideFaceUV[3] = "东面 UV：u 沿 -z，v 沿 -y";
        }
        private static string DescribeFlow(Vec3D v)
        {
            if (Math.Abs(v.X) < 1e-3 && Math.Abs(v.Z) < 1e-3)
                return "静止";

            string dir = "";
            if (v.Z < -0.3) dir += "北";
            else if (v.Z > 0.3) dir += "南";
            if (v.X < -0.3) dir += "西";
            else if (v.X > 0.3) dir += "东";

            return string.IsNullOrEmpty(dir) ? "静止" : dir;
        }
        private static string GetTopUV(float angle, WorldBase world)
        {
            if (angle < -999f) return "无流向，UV 不旋转";

            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            float scale;
            string ver;

            if (world is WorldVersion_1_15 || world is WorldVersion_Modern)
            {
                scale = 0.25f;
                ver = (world is WorldVersion_Modern) ? "现代" : "1.15";
            }
            else
            {
                scale = 8f / 256f;
                ver = (world is WorldVersion_1_8) ? "1.8" : "旧版";
            }

            float var73 = sin * scale;
            float var36 = cos * scale;

            double centerU = 8.0 / 256.0;
            double centerV = 8.0 / 256.0;

            double u0 = centerU - var36 - var73;
            double v0 = centerV - var36 + var73;
            double u1 = centerU - var36 + var73;
            double v1 = centerV + var36 + var73;
            double u2 = centerU + var36 + var73;
            double v2 = centerV + var36 - var73;
            double u3 = centerU + var36 - var73;
            double v3 = centerV - var36 - var73;

            return $"{ver} 旋转角={angle * 180 / Math.PI:F1}°\r\n"
                 + $"  顶点0 (u,v) = ({u0:F4}, {v0:F4})\r\n"
                 + $"  顶点1 (u,v) = ({u1:F4}, {v1:F4})\r\n"
                 + $"  顶点2 (u,v) = ({u2:F4}, {v2:F4})\r\n"
                 + $"  顶点3 (u,v) = ({u3:F4}, {v3:F4})";
        }
    }
}