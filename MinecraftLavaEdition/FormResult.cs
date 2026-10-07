using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MinecraftLavaEdition
{
    public partial class FormResult : Form
    {
        private WorldBase? world;

        public Action<LavaRenderInfo>? OnInfoRequested;

        public FormResult()
        {
            InitializeComponent();

            if (dgvResult.Columns.Count == 0)
            {
                dgvResult.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colCoord",
                    HeaderText = "坐标"
                });
                dgvResult.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colMeta",
                    HeaderText = "metadata"
                });
            }

            dgvResult.AllowUserToAddRows = false;
            dgvResult.ReadOnly = true;
            dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvResult.CellDoubleClick += dgvResult_CellDoubleClick;
        }

        public void SetWorld(WorldBase w) { world = w; }

        public void SetLava(Dictionary<Position, int> lava)
        {
            dgvResult.Rows.Clear();
            if (lava == null) return;

            var ordered = lava
                .OrderBy(p => p.Key.Y)
                .ThenBy(p => p.Key.Z)
                .ThenBy(p => p.Key.X);

            foreach (var kv in ordered)
            {
                int idx = dgvResult.Rows.Add(kv.Key.ToString(), kv.Value);
                dgvResult.Rows[idx].Tag = kv.Key;
            }
        }

        private void dgvResult_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (world == null) return;

            var row = dgvResult.Rows[e.RowIndex];
            if (row.Tag is Position pos)
            {
                var info = LavaRenderInfoBuilder.Build(world, pos);
                OnInfoRequested?.Invoke(info);
            }
        }

        private void FormResult_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
        }
    }
}