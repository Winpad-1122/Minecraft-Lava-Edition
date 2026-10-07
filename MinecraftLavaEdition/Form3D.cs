using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftLavaEdition
{
    public partial class Form3D : Form
    {
        private WorldBase? world;
        private Dictionary<Position, int>? lava;

        private float yaw = 45f;
        private float pitch = 30f;
        private float zoom = 24f;
        private float panX = 0f;
        private float panY = 0f;

        private bool isDragging = false;
        private Point lastMouse;

        private class Face
        {
            public float[][] Points = Array.Empty<float[]>();
            public float Depth;
            public int SubOrder;
            public Color Fill;
            public Color Border;
            public bool IsArrow;
        }

        public Form3D()
        {
            InitializeComponent();

            pic3D.Paint += pic3D_Paint;
            pic3D.MouseDown += pic3D_MouseDown;
            pic3D.MouseMove += pic3D_MouseMove;
            pic3D.MouseUp += pic3D_MouseUp;
            pic3D.MouseWheel += pic3D_MouseWheel;

            trkRange.ValueChanged += (s, e) => pic3D.Invalidate();
        }

        public void SetWorld(WorldBase w) { world = w; }

        public void SetLava(Dictionary<Position, int> lava)
        {
            this.lava = lava;
            pic3D.Invalidate();
        }

        private void pic3D_Paint(object? sender, PaintEventArgs e)
        {
            if (world == null || lava == null || lava.Count == 0) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float scale = zoom;
            int range = trkRange.Value;

            int lMinX = int.MaxValue, lMaxX = int.MinValue;
            int lMinZ = int.MaxValue, lMaxZ = int.MinValue;

            foreach (var p in lava.Keys)
            {
                if (p.X < lMinX) lMinX = p.X;
                if (p.X > lMaxX) lMaxX = p.X;
                if (p.Z < lMinZ) lMinZ = p.Z;
                if (p.Z > lMaxZ) lMaxZ = p.Z;
            }

            int centerX = (lMinX + lMaxX) / 2;
            int centerZ = (lMinZ + lMaxZ) / 2;

            int finalMaxY = int.MinValue;
            foreach (var p in world.Lava.Keys)
                if (p.Y > finalMaxY) finalMaxY = p.Y;
            if (finalMaxY == int.MinValue) finalMaxY = 0;

            int centerY = (0 + finalMaxY) / 2;

            int halfX = Math.Max((lMaxX - lMinX) / 2 + 2, range);
            int halfY = Math.Max((finalMaxY - 0) / 2 + 2, range);
            int halfZ = Math.Max((lMaxZ - lMinZ) / 2 + 2, range);

            int minX = centerX - halfX, maxX = centerX + halfX;
            int minY = centerY - halfY, maxY = centerY + halfY;
            int minZ = centerZ - halfZ, maxZ = centerZ + halfZ;

            int originX = pic3D.Width / 2;
            int originY = pic3D.Height / 2;

            var faces = new List<Face>();

            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        var p = new Position(x, y, z);
                        if (lava.ContainsKey(p)) CollectLavaFaces(faces, p);
                        else if (world.IsSolidMaterial(p)) CollectSolidFaces(faces, p, 1);
                        else if (world.IsAirLike(p)) CollectSolidFaces(faces, p, 2);
                    }
            foreach (var kv in lava)
            {
                var p = kv.Key;
                var info = LavaRenderInfoBuilder.Build(world, p, lava);
                if (info.TopCorners[0] == null) continue;

                float cx = 0, cy = 0, cz = 0;
                foreach (var tc in info.TopCorners)
                {
                    cx += tc[0]; cy += tc[1]; cz += tc[2];
                }
                cx /= 4; cy /= 4; cz /= 4;

                float fx = info.FlowX;
                float fz = info.FlowZ;
                float flen = (float)Math.Sqrt(fx * fx + fz * fz);
                if (flen < 1e-3) continue;

                fx /= flen;
                fz /= flen;

                float arrowWorldLen = 0.45f;
                float startX = cx, startY = cy, startZ = cz;
                float endX = cx + fx * arrowWorldLen;
                float endY = cy;
                float endZ = cz + fz * arrowWorldLen;
                float yawRad = yaw * (float)Math.PI / 180f;
                float cosYaw = (float)Math.Cos(yawRad);
                float sinYaw = (float)Math.Sin(yawRad);

                float x1 = cx * cosYaw - cz * sinYaw;
                float z1 = cx * sinYaw + cz * cosYaw;

                float pitchRad = pitch * (float)Math.PI / 180f;
                float cosPitch = (float)Math.Cos(pitchRad);
                float sinPitch = (float)Math.Sin(pitchRad);

                float z2 = cy * sinPitch + z1 * cosPitch;

                faces.Add(new Face
                {
                    Points = new[]
                    {
                        new[] { startX, startY, startZ },
                        new[] { endX, endY, endZ }
                    },
                    Depth = z2,
                    SubOrder = 100,
                    IsArrow = true,
                });
            }
            faces.Sort((a, b) =>
            {
                if (a.Depth != b.Depth) return a.Depth.CompareTo(b.Depth);
                return a.SubOrder.CompareTo(b.SubOrder);
            });
            foreach (var f in faces)
            {
                if (f.IsArrow)
                {
                    var p0 = Project(f.Points[0][0], f.Points[0][1], f.Points[0][2],
                        centerX, centerY, centerZ, originX, originY, scale);
                    var p1 = Project(f.Points[1][0], f.Points[1][1], f.Points[1][2],
                        centerX, centerY, centerZ, originX, originY, scale);

                    DrawArrowLine(g, p0, p1, scale);
                    continue;
                }

                PointF[] pts = new PointF[f.Points.Length];
                for (int i = 0; i < pts.Length; i++)
                {
                    var v = f.Points[i];
                    pts[i] = Project(v[0], v[1], v[2],
                        centerX, centerY, centerZ, originX, originY, scale);
                }

                using (var b = new SolidBrush(f.Fill))
                using (var pen = new Pen(f.Border, 1))
                {
                    g.FillPolygon(b, pts);
                    g.DrawPolygon(pen, pts);
                }
            }

            DrawAxes(g);
        }
        private void DrawAxes(Graphics g)
        {
            float ox = pic3D.Width - 80;
            float oy = 80;
            float axisLen = 50f;

            var dirX = ProjectAxis(1, 0, 0, axisLen);
            var dirY = ProjectAxis(0, 1, 0, axisLen);
            var dirZ = ProjectAxis(0, 0, 1, axisLen);

            using (var penX = new Pen(Color.Red, 2))
            using (var penY = new Pen(Color.Green, 2))
            using (var penZ = new Pen(Color.Blue, 2))
            using (var font = new Font("Consolas", 8f))
            using (var brushX = new SolidBrush(Color.Red))
            using (var brushY = new SolidBrush(Color.Green))
            using (var brushZ = new SolidBrush(Color.Blue))
            {
                g.DrawLine(penX, ox, oy, ox + dirX.X, oy + dirX.Y);
                g.DrawString("X", font, brushX, ox + dirX.X + 2, oy + dirX.Y + 2);

                g.DrawLine(penY, ox, oy, ox + dirY.X, oy + dirY.Y);
                g.DrawString("Y", font, brushY, ox + dirY.X + 2, oy + dirY.Y + 2);

                g.DrawLine(penZ, ox, oy, ox + dirZ.X, oy + dirZ.Y);
                g.DrawString("Z", font, brushZ, ox + dirZ.X + 2, oy + dirZ.Y + 2);

                using (var brushO = new SolidBrush(Color.Black))
                    g.FillEllipse(brushO, ox - 2, oy - 2, 4, 4);
            }
        }

        private PointF ProjectAxis(float x, float y, float z, float scale)
        {
            float yawRad = yaw * (float)Math.PI / 180f;
            float cosYaw = (float)Math.Cos(yawRad);
            float sinYaw = (float)Math.Sin(yawRad);

            float x1 = x * cosYaw - z * sinYaw;
            float z1 = x * sinYaw + z * cosYaw;

            float pitchRad = pitch * (float)Math.PI / 180f;
            float cosPitch = (float)Math.Cos(pitchRad);
            float sinPitch = (float)Math.Sin(pitchRad);

            float y1 = y * cosPitch - z1 * sinPitch;

            float sx = x1 * scale;
            float sy = -y1 * scale;

            return new PointF(sx, sy);
        }
        private PointF Project(
            float x, float y, float z,
            int centerX, int centerY, int centerZ,
            int originX, int originY, float scale)
        {
            float rx = x - centerX;
            float ry = y - centerY;
            float rz = z - centerZ;

            float yawRad = yaw * (float)Math.PI / 180f;
            float cosYaw = (float)Math.Cos(yawRad);
            float sinYaw = (float)Math.Sin(yawRad);

            float x1 = rx * cosYaw - rz * sinYaw;
            float z1 = rx * sinYaw + rz * cosYaw;

            float pitchRad = pitch * (float)Math.PI / 180f;
            float cosPitch = (float)Math.Cos(pitchRad);
            float sinPitch = (float)Math.Sin(pitchRad);

            float y1 = ry * cosPitch - z1 * sinPitch;
            float z2 = ry * sinPitch + z1 * cosPitch;

            float sx = x1 * scale + panX;
            float sy = -y1 * scale + panY;

            return new PointF(originX + sx, originY + sy);
        }

        private void pic3D_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                isDragging = true;
                lastMouse = e.Location;
            }
        }

        private void pic3D_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            int dx = e.X - lastMouse.X;
            int dy = e.Y - lastMouse.Y;
            lastMouse = e.Location;

            if (e.Button == MouseButtons.Left)
            {
                panX += dx;
                panY += dy;
            }
            else if (e.Button == MouseButtons.Right)
            {
                yaw += dx * 0.5f;
                pitch += dy * 0.5f;
                if (pitch < -89f) pitch = -89f;
                if (pitch > 89f) pitch = 89f;
            }

            pic3D.Invalidate();
        }

        private void pic3D_MouseUp(object? sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void pic3D_MouseWheel(object? sender, MouseEventArgs e)
        {
            float factor = e.Delta > 0 ? 1.1f : 0.9f;
            zoom *= factor;

            if (zoom < 4f) zoom = 4f;
            if (zoom > 120f) zoom = 120f;

            pic3D.Invalidate();
        }

        private void CollectSolidFaces(List<Face> faces, Position p, int kind)
        {
            Color top, bottom, left, right, front, back, border;
            if (kind == 1)
            {
                top = Color.FromArgb(150, 150, 150);
                bottom = Color.FromArgb(100, 100, 100);
                left = Color.FromArgb(110, 110, 110);
                right = Color.FromArgb(80, 80, 80);
                front = Color.FromArgb(95, 95, 95);
                back = Color.FromArgb(125, 125, 125);
                border = Color.Black;
            }
            else
            {
                top = Color.FromArgb(220, 220, 220);
                bottom = Color.FromArgb(170, 170, 170);
                left = Color.FromArgb(200, 200, 200);
                right = Color.FromArgb(180, 180, 180);
                front = Color.FromArgb(190, 190, 190);
                back = Color.FromArgb(210, 210, 210);
                border = Color.Gray;
            }

            float x = p.X, y = p.Y, z = p.Z;

            AddFace(faces, left, border, 0,
                new[] { new[] { x, y, z }, new[] { x, y, z + 1 }, new[] { x, y + 1, z + 1 }, new[] { x, y + 1, z } });

            AddFace(faces, right, border, 1,
                new[] { new[] { x + 1, y, z + 1 }, new[] { x + 1, y, z }, new[] { x + 1, y + 1, z }, new[] { x + 1, y + 1, z + 1 } });

            AddFace(faces, back, border, 2,
                new[] { new[] { x + 1, y, z }, new[] { x, y, z }, new[] { x, y + 1, z }, new[] { x + 1, y + 1, z } });

            AddFace(faces, front, border, 3,
                new[] { new[] { x, y, z + 1 }, new[] { x + 1, y, z + 1 }, new[] { x + 1, y + 1, z + 1 }, new[] { x, y + 1, z + 1 } });

            AddFace(faces, bottom, border, 4,
                new[] { new[] { x, y, z + 1 }, new[] { x, y, z }, new[] { x + 1, y, z }, new[] { x + 1, y, z + 1 } });

            AddFace(faces, top, border, 5,
                new[] { new[] { x, y + 1, z }, new[] { x, y + 1, z + 1 }, new[] { x + 1, y + 1, z + 1 }, new[] { x + 1, y + 1, z } });
        }

        private void CollectLavaFaces(List<Face> faces, Position p)
        {
            var info = LavaRenderInfoBuilder.Build(world!, p, lava!);
            if (info.TopCorners[0] == null) return;

            var NW = info.TopCorners[0];
            var SW = info.TopCorners[1];
            var SE = info.TopCorners[2];
            var NE = info.TopCorners[3];

            float x = p.X, y = p.Y, z = p.Z;

            Color topC = Color.FromArgb(255, 140, 0);
            Color bottomC = Color.FromArgb(180, 80, 0);
            Color leftC = Color.FromArgb(220, 100, 0);
            Color rightC = Color.FromArgb(170, 70, 0);
            Color frontC = Color.FromArgb(220, 100, 0);
            Color backC = Color.FromArgb(170, 70, 0);
            Color border = Color.DarkRed;

            AddFace(faces, leftC, border, 0,
                new[] { NW, SW, new[] { x, y, z + 1 }, new[] { x, y, z } });

            AddFace(faces, rightC, border, 1,
                new[] { NE, SE, new[] { x + 1, y, z + 1 }, new[] { x + 1, y, z } });

            AddFace(faces, backC, border, 2,
                new[] { NW, NE, new[] { x + 1, y, z }, new[] { x, y, z } });

            AddFace(faces, frontC, border, 3,
                new[] { SW, SE, new[] { x + 1, y, z + 1 }, new[] { x, y, z + 1 } });

            AddFace(faces, bottomC, border, 4,
                new[] { new[] { x, y, z + 1 }, new[] { x, y, z }, new[] { x + 1, y, z }, new[] { x + 1, y, z + 1 } });

            AddFace(faces, topC, border, 5,
                new[] { NW, SW, SE, NE });
        }

        private void AddFace(List<Face> faces, Color fill, Color border, int subOrder, float[][] pts)
        {
            float cx = 0, cy = 0, cz = 0;
            foreach (var pt in pts)
            {
                cx += pt[0]; cy += pt[1]; cz += pt[2];
            }
            cx /= pts.Length; cy /= pts.Length; cz /= pts.Length;
            float yawRad = yaw * (float)Math.PI / 180f;
            float cosYaw = (float)Math.Cos(yawRad);
            float sinYaw = (float)Math.Sin(yawRad);

            float x1 = cx * cosYaw - cz * sinYaw;
            float z1 = cx * sinYaw + cz * cosYaw;

            float pitchRad = pitch * (float)Math.PI / 180f;
            float cosPitch = (float)Math.Cos(pitchRad);
            float sinPitch = (float)Math.Sin(pitchRad);

            float z2 = cy * sinPitch + z1 * cosPitch;

            faces.Add(new Face
            {
                Points = pts,
                Depth = z2,
                SubOrder = subOrder,
                Fill = fill,
                Border = border,
                IsArrow = false
            });
        }
        private void DrawArrowLine(Graphics g, PointF p0, PointF p1, float scale)
        {
            float dx = p1.X - p0.X;
            float dy = p1.Y - p0.Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            if (len < 1e-3) return;

            using (var pen = new Pen(Color.Black, 2))
            {
                g.DrawLine(pen, p0, p1);

                float angle = (float)Math.Atan2(dy, dx);
                float arrowSize = scale * 0.15f;

                float a1 = angle + (float)Math.PI * 0.75f;
                float a2 = angle - (float)Math.PI * 0.75f;

                g.DrawLine(pen, p1.X, p1.Y,
                    p1.X + (float)Math.Cos(a1) * arrowSize,
                    p1.Y + (float)Math.Sin(a1) * arrowSize);
                g.DrawLine(pen, p1.X, p1.Y,
                    p1.X + (float)Math.Cos(a2) * arrowSize,
                    p1.Y + (float)Math.Sin(a2) * arrowSize);
            }
        }
    }
}