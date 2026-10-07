using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftLavaEdition
{
    public partial class FormMain : Form
    {
        private FormInitvar? formInitvar;
        private FormResult? formResult;
        private FormDisplay? formDisplay;
        private Form3D? form3D;
        private FormInfo? formInfo;

        public WorldBase? CurrentVersion { get; private set; }

        private WorldBase? world;
        private List<Snapshot>? snapshots;
        private int currentIndex = 0;

        private List<Position> lastSource = new();
        private List<Position> lastSolid = new();
        private List<Position> lastAir = new();

        private System.Windows.Forms.Timer playTimer;
        private bool isPlaying = false;

#pragma warning disable CS8618
        public FormMain()
#pragma warning restore CS8618
        {
            InitializeComponent();

            btnBack.Click += btnBack_Click;
            btnForward.Click += btnForward_Click;

            playTimer = new System.Windows.Forms.Timer();
            playTimer.Interval = 300;
            playTimer.Tick += playTimer_Tick;

            UpdateButtons();
        }

        private void FormMain_Load(object? sender, EventArgs e)
        {
            cmbVersion.Items.Clear();
            foreach (var name in WorldVersions.GetNames())
                cmbVersion.Items.Add(name);
            cmbVersion.SelectedIndex = 0;
            cmbVersion.SelectedIndexChanged += cmbVersion_SelectedIndexChanged;

            txtSeed.Leave += (s, ev) =>
            {
                if (lastSource.Count == 0 && lastSolid.Count == 0 && lastAir.Count == 0)
                    return;
                RunSimulation(lastSource, lastSolid, lastAir);
            };

            chkNether.CheckedChanged += (s, ev) =>
            {
                if (lastSource.Count == 0 && lastSolid.Count == 0 && lastAir.Count == 0)
                    return;
                RunSimulation(lastSource, lastSolid, lastAir);
            };

            UpdateVersionControls();

            formInitvar = new FormInitvar(this);
            formInitvar.Show(this);
            formInitvar.StartPosition = FormStartPosition.Manual;
            formInitvar.Location = new Point(this.Left, this.Bottom - 5);

            formResult = new FormResult();
            formResult.Show(this);
            formResult.StartPosition = FormStartPosition.Manual;
            formResult.Location = new Point(this.Right - 285, this.Bottom - 5);

            formDisplay = new FormDisplay();
            formDisplay.Show(this);
            formDisplay.StartPosition = FormStartPosition.Manual;
            formDisplay.Location = new Point(this.Right - 765, this.Bottom - 5);

            form3D = new Form3D();
            form3D.Show(this);
            form3D.StartPosition = FormStartPosition.Manual;
            form3D.Location = new Point(this.Right - 1425, this.Bottom - 5);

            formInfo = new FormInfo();
            formInfo.Show(this);
            formInfo.StartPosition = FormStartPosition.Manual;
            formInfo.Location = new Point(this.Right - 1425, this.Bottom + 480);

            formResult.OnInfoRequested += info =>
            {
                if (formInfo != null && !formInfo.IsDisposed)
                    formInfo.SetInfo(info);
            };
        }

        private void UpdateVersionControls()
        {
            string name = cmbVersion.SelectedItem?.ToString() ?? "";

            bool useSeed = name.Contains("20100617")
                        || name.Contains("Alpha")
                        || name.Contains("1.0.0");

            txtSeed.Enabled = useSeed;
            lblSeed.Enabled = useSeed;

            bool allowNether = name.Contains("Alpha") || name.Contains("1.0.0") || name.Contains("1.8") || name.Contains("1.13") || name.Contains("Mod");
            chkNether.Enabled = allowNether;
        }

        private void cmbVersion_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateVersionControls();

            if (lastSource.Count == 0 && lastSolid.Count == 0 && lastAir.Count == 0)
                return;

            RunSimulation(lastSource, lastSolid, lastAir);
        }

        public void RunSimulation(
            List<Position> source, List<Position> solid, List<Position> air)
        {
            lastSource = source;
            lastSolid = solid;
            lastAir = air;

            var allVersions = WorldVersions.CreateAll();
            int idx = cmbVersion.SelectedIndex;
            if (idx < 0) idx = 0;
            CurrentVersion = allVersions[idx];

            long seed = 12345L;
            long.TryParse(txtSeed.Text, out seed);
            NextTickListEntry.ResetId();
            CurrentVersion.Seed = seed;
            CurrentVersion.ResetRandom();

            if (CurrentVersion is WorldVersion_Alpha_1_2_2 alpha)
                alpha.IsNether = chkNether.Checked;
            else if (CurrentVersion is WorldVersion_1_0_0_rc1 rc1)
                rc1.IsNether = chkNether.Checked;
            else if (CurrentVersion is WorldVersion_1_8 v18)
                v18.IsNether = chkNether.Checked;
            else if (CurrentVersion is WorldVersion_1_15 v115)
                v115.IsNether = chkNether.Checked;
            else if (CurrentVersion is WorldVersion_Modern vModern)
                vModern.IsNether = chkNether.Checked;

            foreach (var p in source) CurrentVersion.AddLavaSource(p);
            foreach (var p in solid) CurrentVersion.AddSolid(p);
            foreach (var p in air) CurrentVersion.AddAirLike(p);

            List<Snapshot> snapshots = Simulation.Run(CurrentVersion);
            LoadSnapshots(CurrentVersion, snapshots);
        }

        public void LoadSnapshots(WorldBase w, List<Snapshot> result)
        {
            world = w;
            snapshots = result;
            currentIndex = 0;

            if (formResult != null && !formResult.IsDisposed) formResult.SetWorld(w);
            if (formDisplay != null && !formDisplay.IsDisposed) formDisplay.SetWorld(w);
            if (form3D != null && !form3D.IsDisposed) form3D.SetWorld(w);

            ShowCurrent();
            UpdateButtons();
        }

        private void ShowCurrent()
        {
            if (snapshots == null || snapshots.Count == 0) return;

            var snap = snapshots[currentIndex];

            lblTick.Text = $"第 {snap.Tick} / {snapshots[snapshots.Count - 1].Tick} tick";

            if (formResult != null && !formResult.IsDisposed) formResult.SetLava(snap.Lava);
            if (formDisplay != null && !formDisplay.IsDisposed) formDisplay.SetLava(snap.Lava);
            if (form3D != null && !form3D.IsDisposed) form3D.SetLava(snap.Lava);
        }

        private void btnBack_Click(object? sender, EventArgs e)
        {
            if (snapshots == null) return;
            if (currentIndex > 0)
            {
                currentIndex--;
                ShowCurrent();
                UpdateButtons();
            }
        }

        private void btnForward_Click(object? sender, EventArgs e)
        {
            if (snapshots == null) return;
            if (currentIndex < snapshots.Count - 1)
            {
                currentIndex++;
                ShowCurrent();
                UpdateButtons();
            }
        }

        private void btnPlay_Click(object? sender, EventArgs e)
        {
            if (isPlaying) StopPlay();
            else StartPlay();
            
        }

        private void StartPlay()
        {
            if (snapshots == null) return;
            if (currentIndex >= snapshots.Count - 1) return;
            btnPlay.BackgroundImage = Properties.Resources.Stop;
            isPlaying = true;
            playTimer.Start();
        }

        private void StopPlay()
        {
            btnPlay.BackgroundImage = Properties.Resources.Play;
            isPlaying = false;
            playTimer.Stop();
            UpdateButtons();
        }

        private void playTimer_Tick(object? sender, EventArgs e)
        {
            if (snapshots == null) { StopPlay(); return; }

            if (currentIndex < snapshots.Count - 1)
            {
                currentIndex++;
                ShowCurrent();
                UpdateButtons();
            }
            else StopPlay();
        }

        private void UpdateButtons()
        {
            bool hasData = snapshots != null && snapshots.Count > 0;

            btnBack.Enabled = hasData && currentIndex > 0;
            btnForward.Enabled = hasData && currentIndex < snapshots.Count - 1;
            btnPlay.Enabled = hasData;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            playTimer?.Stop();
            base.OnFormClosed(e);
        }

        private void 初始参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formInitvar != null && !formInitvar.IsDisposed)
            {
                formInitvar.Activate();
                formInitvar.BringToFront();
                return;
            }
            formInitvar = new FormInitvar(this);
            formInitvar.StartPosition = FormStartPosition.Manual;
            formInitvar.Location = new Point(this.Left, this.Bottom - 5);
            formInitvar.Show(this);
        }

        private void 输出结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formInitvar != null && !formInitvar.IsDisposed)
            {
                formInitvar.Activate();
                formInitvar.BringToFront();
                return;
            }
            formResult = new FormResult();
            formResult.Show(this);
            formResult.StartPosition = FormStartPosition.Manual;
            formResult.Location = new Point(this.Right - 285, this.Bottom - 5);
        }

        private void 详情信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formInitvar != null && !formInitvar.IsDisposed)
            {
                formInitvar.Activate();
                formInitvar.BringToFront();
                return;
            }
            formInfo = new FormInfo();
            formInfo.Show(this);
            formInfo.StartPosition = FormStartPosition.Manual;
            formInfo.Location = new Point(this.Right - 1425, this.Bottom + 480);
        }

        private void 平面视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formInitvar != null && !formInitvar.IsDisposed)
            {
                formInitvar.Activate();
                formInitvar.BringToFront();
                return;
            }
            formDisplay = new FormDisplay();
            formDisplay.Show(this);
            formDisplay.StartPosition = FormStartPosition.Manual;
            formDisplay.Location = new Point(this.Right - 765, this.Bottom - 5);
        }

        private void 三维视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formInitvar != null && !formInitvar.IsDisposed)
            {
                formInitvar.Activate();
                formInitvar.BringToFront();
                return;
                form3D = new Form3D();
                form3D.Show(this);
                form3D.StartPosition = FormStartPosition.Manual;
                form3D.Location = new Point(this.Right - 1425, this.Bottom - 5);
            }

        }

    }
}