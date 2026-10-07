namespace MinecraftLavaEdition
{
    public partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            lblTick = new Label();
            btnBack = new Button();
            btnPlay = new Button();
            btnForward = new Button();
            cmbVersion = new ComboBox();
            lblSeed = new Label();
            txtSeed = new TextBox();
            chkNether = new CheckBox();
            menuStrip1 = new MenuStrip();
            视图ToolStripMenuItem = new ToolStripMenuItem();
            初始参数ToolStripMenuItem = new ToolStripMenuItem();
            输出结果ToolStripMenuItem = new ToolStripMenuItem();
            详情信息ToolStripMenuItem = new ToolStripMenuItem();
            平面视图ToolStripMenuItem = new ToolStripMenuItem();
            三维视图ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTick
            // 
            lblTick.AutoSize = true;
            lblTick.Location = new Point(141, 4);
            lblTick.Name = "lblTick";
            lblTick.Size = new Size(67, 17);
            lblTick.TabIndex = 0;
            lblTick.Text = "第 0/0 tick";
            // 
            // btnBack
            // 
            btnBack.BackgroundImage = Properties.Resources.Pre;
            btnBack.BackgroundImageLayout = ImageLayout.Stretch;
            btnBack.Location = new Point(56, 1);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(25, 25);
            btnBack.TabIndex = 1;
            btnBack.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            btnPlay.BackgroundImage = Properties.Resources.Play;
            btnPlay.BackgroundImageLayout = ImageLayout.Stretch;
            btnPlay.Location = new Point(84, 1);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(25, 25);
            btnPlay.TabIndex = 2;
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnForward
            // 
            btnForward.BackgroundImage = Properties.Resources.Next;
            btnForward.BackgroundImageLayout = ImageLayout.Stretch;
            btnForward.Location = new Point(112, 1);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(25, 25);
            btnForward.TabIndex = 3;
            btnForward.UseVisualStyleBackColor = true;
            // 
            // cmbVersion
            // 
            cmbVersion.FormattingEnabled = true;
            cmbVersion.Location = new Point(283, 2);
            cmbVersion.Name = "cmbVersion";
            cmbVersion.Size = new Size(235, 25);
            cmbVersion.TabIndex = 4;
            // 
            // lblSeed
            // 
            lblSeed.AutoSize = true;
            lblSeed.Location = new Point(526, 4);
            lblSeed.Name = "lblSeed";
            lblSeed.Size = new Size(44, 17);
            lblSeed.TabIndex = 5;
            lblSeed.Text = "种子：";
            // 
            // txtSeed
            // 
            txtSeed.Location = new Point(563, 3);
            txtSeed.Name = "txtSeed";
            txtSeed.Size = new Size(133, 23);
            txtSeed.TabIndex = 6;
            // 
            // chkNether
            // 
            chkNether.AutoSize = true;
            chkNether.Location = new Point(713, 4);
            chkNether.Name = "chkNether";
            chkNether.Size = new Size(51, 21);
            chkNether.TabIndex = 7;
            chkNether.Text = "下界";
            chkNether.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 视图ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1684, 25);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // 视图ToolStripMenuItem
            // 
            视图ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 初始参数ToolStripMenuItem, 输出结果ToolStripMenuItem, 详情信息ToolStripMenuItem, 平面视图ToolStripMenuItem, 三维视图ToolStripMenuItem });
            视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            视图ToolStripMenuItem.Size = new Size(44, 21);
            视图ToolStripMenuItem.Text = "视图";
            // 
            // 初始参数ToolStripMenuItem
            // 
            初始参数ToolStripMenuItem.Name = "初始参数ToolStripMenuItem";
            初始参数ToolStripMenuItem.Size = new Size(124, 22);
            初始参数ToolStripMenuItem.Text = "初始参数";
            初始参数ToolStripMenuItem.Click += 初始参数ToolStripMenuItem_Click;
            // 
            // 输出结果ToolStripMenuItem
            // 
            输出结果ToolStripMenuItem.Name = "输出结果ToolStripMenuItem";
            输出结果ToolStripMenuItem.Size = new Size(124, 22);
            输出结果ToolStripMenuItem.Text = "输出结果";
            输出结果ToolStripMenuItem.Click += 输出结果ToolStripMenuItem_Click;
            // 
            // 详情信息ToolStripMenuItem
            // 
            详情信息ToolStripMenuItem.Name = "详情信息ToolStripMenuItem";
            详情信息ToolStripMenuItem.Size = new Size(124, 22);
            详情信息ToolStripMenuItem.Text = "详情信息";
            详情信息ToolStripMenuItem.Click += 详情信息ToolStripMenuItem_Click;
            // 
            // 平面视图ToolStripMenuItem
            // 
            平面视图ToolStripMenuItem.Name = "平面视图ToolStripMenuItem";
            平面视图ToolStripMenuItem.Size = new Size(124, 22);
            平面视图ToolStripMenuItem.Text = "平面视图";
            平面视图ToolStripMenuItem.Click += 平面视图ToolStripMenuItem_Click;
            // 
            // 三维视图ToolStripMenuItem
            // 
            三维视图ToolStripMenuItem.Name = "三维视图ToolStripMenuItem";
            三维视图ToolStripMenuItem.Size = new Size(124, 22);
            三维视图ToolStripMenuItem.Text = "三维视图";
            三维视图ToolStripMenuItem.Click += 三维视图ToolStripMenuItem_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1684, 31);
            Controls.Add(chkNether);
            Controls.Add(txtSeed);
            Controls.Add(lblSeed);
            Controls.Add(cmbVersion);
            Controls.Add(btnForward);
            Controls.Add(btnPlay);
            Controls.Add(btnBack);
            Controls.Add(lblTick);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximumSize = new Size(1700, 70);
            MinimumSize = new Size(1700, 70);
            Name = "FormMain";
            Text = "Minecraft: Lava Edition";
            Load += FormMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTick;
        private Button btnBack;
        private Button btnPlay;
        private Button btnForward;
        private ComboBox cmbVersion;
        private Label lblSeed;
        private TextBox txtSeed;
        private CheckBox chkNether;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 视图ToolStripMenuItem;
        private ToolStripMenuItem 初始参数ToolStripMenuItem;
        private ToolStripMenuItem 输出结果ToolStripMenuItem;
        private ToolStripMenuItem 详情信息ToolStripMenuItem;
        private ToolStripMenuItem 平面视图ToolStripMenuItem;
        private ToolStripMenuItem 三维视图ToolStripMenuItem;
    }
}
