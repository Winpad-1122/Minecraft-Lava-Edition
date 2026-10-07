using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MinecraftLavaEdition
{
    public partial class FormInitvar : Form
    {
        private readonly FormMain mainForm;

        public FormInitvar(FormMain main)
        {
            InitializeComponent();

            mainForm = main;

            InitGrid(dgvSource);
            InitGrid(dgvSimSolid);
            InitGrid(dgvSimAir);

            btnAddSource.Click += btnAddSource_Click;
            btnAddSimSolid.Click += btnAddSimSolid_Click;
            btnAddSimAir.Click += btnAddSimAir_Click;

            btnDelSource.Click += btnDelSource_Click;
            btnDelSimSolid.Click += btnDelSimSolid_Click;
            btnDelSimAir.Click += btnDelSimAir_Click;

            btnOK.Click += btnOK_Click;
        }

        private void InitGrid(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = true;
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgv.Columns.Count == 0)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colX", HeaderText = "x" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colY", HeaderText = "y" });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colZ", HeaderText = "z" });
            }
        }

        private void btnAddSource_Click(object? sender, EventArgs e) => AddRow(dgvSource);
        private void btnAddSimSolid_Click(object? sender, EventArgs e) => AddRow(dgvSimSolid);
        private void btnAddSimAir_Click(object? sender, EventArgs e) => AddRow(dgvSimAir);

        private void AddRow(DataGridView dgv)
        {
            int index = dgv.Rows.Add();
            dgv.ClearSelection();
            dgv.Rows[index].Selected = true;
            dgv.CurrentCell = dgv.Rows[index].Cells[0];
            dgv.BeginEdit(true);
        }

        private void btnDelSource_Click(object? sender, EventArgs e) => DeleteSelectedRows(dgvSource);
        private void btnDelSimSolid_Click(object? sender, EventArgs e) => DeleteSelectedRows(dgvSimSolid);
        private void btnDelSimAir_Click(object? sender, EventArgs e) => DeleteSelectedRows(dgvSimAir);

        private void DeleteSelectedRows(DataGridView dgv)
        {
            var indexes = dgv.SelectedRows
                             .Cast<DataGridViewRow>()
                             .Where(r => !r.IsNewRow)
                             .Select(r => r.Index)
                             .Distinct()
                             .OrderByDescending(i => i)
                             .ToList();

            if (indexes.Count == 0)
            {
                MessageBox.Show("未选中对象。", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (int i in indexes)
                dgv.Rows.RemoveAt(i);
        }

        private void btnOK_Click(object? sender, EventArgs e)
        {
            if (!TryGetPositions(dgvSource, "Source", out var source)) return;
            if (!TryGetPositions(dgvSimSolid, "SimSolid", out var solid)) return;
            if (!TryGetPositions(dgvSimAir, "SimAir", out var air)) return;

            mainForm.RunSimulation(source, solid, air);
        }

        private bool TryGetPositions(DataGridView dgv, string name, out List<Position> result)
        {
            result = new List<Position>();

            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                var row = dgv.Rows[r];
                if (row.IsNewRow) continue;

                string sx = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                string sy = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                string sz = row.Cells[2].Value?.ToString()?.Trim() ?? "";

                if (sx.Length == 0 && sy.Length == 0 && sz.Length == 0)
                    continue;

                if (!int.TryParse(sx, out int x) ||
                    !int.TryParse(sy, out int y) ||
                    !int.TryParse(sz, out int z))
                {
                    MessageBox.Show(
                        $"“{name}”第 {r + 1} 行格式错误，必须是三个整数。",
                        "数据错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    dgv.ClearSelection();
                    dgv.Rows[r].Selected = true;
                    dgv.CurrentCell = dgv.Rows[r].Cells[0];
                    return false;
                }

                result.Add(new Position(x, y, z));
            }

            return true;
        }

        private void FormInitvar_Load(object? sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
        }
    }
}