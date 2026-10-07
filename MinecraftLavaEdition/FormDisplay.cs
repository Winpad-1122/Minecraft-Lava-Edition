using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftLavaEdition
{
    public partial class FormDisplay : Form
    {
        private WorldBase? world;
        private Dictionary<Position, int>? lava;

        private const int CellSize = 40;

        public FormDisplay()
        {
            InitializeComponent();

            numY.ValueChanged += numY_ValueChanged;
            picMap.Paint += picMap_Paint;
        }

        public void SetWorld(WorldBase w) { world = w; }

        public void SetLava(Dictionary<Position, int> lava)
        {
            this.lava = lava;
            picMap.Invalidate();
        }

        private void numY_ValueChanged(object? sender, EventArgs e)
        {
            picMap.Invalidate();
        }

        private void picMap_Paint(object? sender, PaintEventArgs e)
        {
            if (world == null || lava == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int y = (int)numY.Value;

            double cx = 0, cz = 0;
            int count = 0;
            foreach (var p in lava.Keys)
            {
                cx += p.X;
                cz += p.Z;
                count++;
            }
            if (count == 0) return;
            cx /= count;
            cz /= count;

            int originX = picMap.Width / 2;
            int originY = picMap.Height / 2;

            var solidSet = new HashSet<Position>();
            var airLikeSet = new HashSet<Position>();
            var airSet = new HashSet<Position>();

            foreach (var p in lava.Keys)
            {
                if (p.Y != y) continue;

                AddNeighborIfBlocking(world, solidSet, airLikeSet, airSet,
                    new Position(p.X - 1, p.Y, p.Z));
                AddNeighborIfBlocking(world, solidSet, airLikeSet, airSet,
                    new Position(p.X + 1, p.Y, p.Z));
                AddNeighborIfBlocking(world, solidSet, airLikeSet, airSet,
                    new Position(p.X, p.Y, p.Z - 1));
                AddNeighborIfBlocking(world, solidSet, airLikeSet, airSet,
                    new Position(p.X, p.Y, p.Z + 1));
            }

            using (var pen = new Pen(Color.Black, 2))
                foreach (var p in solidSet) DrawCell(g, p, originX, originY, pen);

            using (var pen = new Pen(Color.Gray, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                foreach (var p in airLikeSet) DrawCell(g, p, originX, originY, pen);
            }

            using (var pen = new Pen(Color.LightGray, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                foreach (var p in airSet) DrawCell(g, p, originX, originY, pen);
            }

            using (var pen = new Pen(Color.Orange, 2))
            using (var brush = new SolidBrush(Color.Orange))
            using (var font = new Font("Consolas", 7f))
            using (var textBrush = new SolidBrush(Color.DarkOrange))
            {
                foreach (var kv in lava)
                {
                    var p = kv.Key;
                    if (p.Y != y) continue;

                    DrawCell(g, p, originX, originY, pen);

                    var info = LavaRenderInfoBuilder.Build(world, p, lava);
                    DrawArrow(g, p, originX, originY, info.FlowX, info.FlowZ, brush);

                    DrawCoord(g, p, originX, originY, font, textBrush);
                }
            }
        }

        private void AddNeighborIfBlocking(
            WorldBase world,
            HashSet<Position> solidSet,
            HashSet<Position> airLikeSet,
            HashSet<Position> airSet,
            Position p)
        {
            if (world.IsSolidMaterial(p)) solidSet.Add(p);
            else if (world.IsAirLike(p)) airLikeSet.Add(p);
            else if (!world.IsLava(p)) airSet.Add(p);
        }

        private void DrawCell(Graphics g, Position p, int originX, int originY, Pen pen)
        {
            int sx = originX + p.X * CellSize - CellSize / 2;
            int sy = originY + p.Z * CellSize - CellSize / 2;
            g.DrawRectangle(pen, sx, sy, CellSize, CellSize);
        }

        private void DrawArrow(
            Graphics g, Position p, int originX, int originY,
            float fx, float fz, Brush brush)
        {
            int cx = originX + p.X * CellSize;
            int cy = originY + p.Z * CellSize;
            int arrowCy = cy - 6;

            int half = CellSize / 2 - 12;
            float len = (float)Math.Sqrt(fx * fx + fz * fz);

            if (len < 1e-3)
            {
                g.FillEllipse(brush, cx - 2, arrowCy - 2, 4, 4);
                return;
            }

            float dx = fx / len;
            float dy = fz / len;

            int ex = cx + (int)(dx * half);
            int ey = arrowCy + (int)(dy * half);

            using (var pen = new Pen(brush, 2))
            {
                g.DrawLine(pen, cx, arrowCy, ex, ey);

                float arrowSize = 5;
                float angle = (float)Math.Atan2(dy, dx);
                float a1 = angle + (float)Math.PI * 0.75f;
                float a2 = angle - (float)Math.PI * 0.75f;

                g.DrawLine(pen, ex, ey,
                    ex + (float)Math.Cos(a1) * arrowSize,
                    ey + (float)Math.Sin(a1) * arrowSize);
                g.DrawLine(pen, ex, ey,
                    ex + (float)Math.Cos(a2) * arrowSize,
                    ey + (float)Math.Sin(a2) * arrowSize);
            }
        }

        private void DrawCoord(
            Graphics g, Position p, int originX, int originY,
            Font font, Brush brush)
        {
            int cx = originX + p.X * CellSize;
            int cy = originY + p.Z * CellSize;

            string text = $"{p.X},{p.Z}";
            var size = g.MeasureString(text, font);

            float tx = cx - size.Width / 2f;
            float ty = cy + CellSize / 2f - size.Height - 2;

            g.DrawString(text, font, brush, tx, ty);
        }

        private void FormDisplay_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
        }
    }
}