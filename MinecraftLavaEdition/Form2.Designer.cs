namespace MinecraftLavaEdition
{
    partial class FormInitvar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInitvar));
            groupsource = new GroupBox();
            btnDelSource = new Button();
            btnAddSource = new Button();
            dgvSource = new DataGridView();
            Source_x = new DataGridViewTextBoxColumn();
            Souce_y = new DataGridViewTextBoxColumn();
            Source_z = new DataGridViewTextBoxColumn();
            dgvSimSolid = new DataGridView();
            SimSolid_x = new DataGridViewTextBoxColumn();
            SimSolid_y = new DataGridViewTextBoxColumn();
            SimSolid_z = new DataGridViewTextBoxColumn();
            groupSimAir = new GroupBox();
            btnAddSimAir = new Button();
            btnDelSimAir = new Button();
            dgvSimAir = new DataGridView();
            SimAir_x = new DataGridViewTextBoxColumn();
            SimAir_y = new DataGridViewTextBoxColumn();
            SimAir_z = new DataGridViewTextBoxColumn();
            groupSimSolid = new GroupBox();
            btnDelSimSolid = new Button();
            btnAddSimSolid = new Button();
            btnOK = new Button();
            groupsource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSimSolid).BeginInit();
            groupSimAir.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSimAir).BeginInit();
            groupSimSolid.SuspendLayout();
            SuspendLayout();
            // 
            // groupsource
            // 
            groupsource.Controls.Add(btnDelSource);
            groupsource.Controls.Add(btnAddSource);
            groupsource.Controls.Add(dgvSource);
            groupsource.Location = new Point(12, 13);
            groupsource.Name = "groupsource";
            groupsource.Size = new Size(246, 207);
            groupsource.TabIndex = 3;
            groupsource.TabStop = false;
            groupsource.Text = "熔岩源位置";
            // 
            // btnDelSource
            // 
            btnDelSource.Location = new Point(128, 174);
            btnDelSource.Name = "btnDelSource";
            btnDelSource.Size = new Size(112, 27);
            btnDelSource.TabIndex = 3;
            btnDelSource.Text = "移除";
            btnDelSource.UseVisualStyleBackColor = true;
            // 
            // btnAddSource
            // 
            btnAddSource.Location = new Point(6, 174);
            btnAddSource.Name = "btnAddSource";
            btnAddSource.Size = new Size(112, 27);
            btnAddSource.TabIndex = 2;
            btnAddSource.Text = "添加";
            btnAddSource.UseVisualStyleBackColor = true;
            // 
            // dgvSource
            // 
            dgvSource.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSource.Columns.AddRange(new DataGridViewColumn[] { Source_x, Souce_y, Source_z });
            dgvSource.Location = new Point(6, 22);
            dgvSource.Name = "dgvSource";
            dgvSource.Size = new Size(234, 146);
            dgvSource.TabIndex = 1;
            // 
            // Source_x
            // 
            Source_x.HeaderText = "x";
            Source_x.Name = "Source_x";
            // 
            // Souce_y
            // 
            Souce_y.HeaderText = "y";
            Souce_y.Name = "Souce_y";
            // 
            // Source_z
            // 
            Source_z.HeaderText = "z";
            Source_z.Name = "Source_z";
            // 
            // dgvSimSolid
            // 
            dgvSimSolid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSimSolid.Columns.AddRange(new DataGridViewColumn[] { SimSolid_x, SimSolid_y, SimSolid_z });
            dgvSimSolid.Location = new Point(6, 22);
            dgvSimSolid.Name = "dgvSimSolid";
            dgvSimSolid.Size = new Size(234, 146);
            dgvSimSolid.TabIndex = 1;
            // 
            // SimSolid_x
            // 
            SimSolid_x.HeaderText = "x";
            SimSolid_x.Name = "SimSolid_x";
            // 
            // SimSolid_y
            // 
            SimSolid_y.HeaderText = "y";
            SimSolid_y.Name = "SimSolid_y";
            // 
            // SimSolid_z
            // 
            SimSolid_z.HeaderText = "z";
            SimSolid_z.Name = "SimSolid_z";
            // 
            // groupSimAir
            // 
            groupSimAir.Controls.Add(btnAddSimAir);
            groupSimAir.Controls.Add(btnDelSimAir);
            groupSimAir.Controls.Add(dgvSimAir);
            groupSimAir.Location = new Point(12, 459);
            groupSimAir.Name = "groupSimAir";
            groupSimAir.Size = new Size(246, 207);
            groupSimAir.TabIndex = 5;
            groupSimAir.TabStop = false;
            groupSimAir.Text = "类空气位置";
            // 
            // btnAddSimAir
            // 
            btnAddSimAir.Location = new Point(6, 174);
            btnAddSimAir.Name = "btnAddSimAir";
            btnAddSimAir.Size = new Size(112, 27);
            btnAddSimAir.TabIndex = 5;
            btnAddSimAir.Text = "添加";
            btnAddSimAir.UseVisualStyleBackColor = true;
            // 
            // btnDelSimAir
            // 
            btnDelSimAir.Location = new Point(128, 174);
            btnDelSimAir.Name = "btnDelSimAir";
            btnDelSimAir.Size = new Size(112, 27);
            btnDelSimAir.TabIndex = 6;
            btnDelSimAir.Text = "移除";
            btnDelSimAir.UseVisualStyleBackColor = true;
            // 
            // dgvSimAir
            // 
            dgvSimAir.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSimAir.Columns.AddRange(new DataGridViewColumn[] { SimAir_x, SimAir_y, SimAir_z });
            dgvSimAir.Location = new Point(6, 22);
            dgvSimAir.Name = "dgvSimAir";
            dgvSimAir.Size = new Size(234, 146);
            dgvSimAir.TabIndex = 1;
            // 
            // SimAir_x
            // 
            SimAir_x.HeaderText = "x";
            SimAir_x.Name = "SimAir_x";
            // 
            // SimAir_y
            // 
            SimAir_y.HeaderText = "y";
            SimAir_y.Name = "SimAir_y";
            // 
            // SimAir_z
            // 
            SimAir_z.HeaderText = "z";
            SimAir_z.Name = "SimAir_z";
            // 
            // groupSimSolid
            // 
            groupSimSolid.Controls.Add(btnDelSimSolid);
            groupSimSolid.Controls.Add(btnAddSimSolid);
            groupSimSolid.Controls.Add(dgvSimSolid);
            groupSimSolid.Location = new Point(12, 231);
            groupSimSolid.Name = "groupSimSolid";
            groupSimSolid.Size = new Size(246, 207);
            groupSimSolid.TabIndex = 4;
            groupSimSolid.TabStop = false;
            groupSimSolid.Text = "类固体位置";
            // 
            // btnDelSimSolid
            // 
            btnDelSimSolid.Location = new Point(128, 174);
            btnDelSimSolid.Name = "btnDelSimSolid";
            btnDelSimSolid.Size = new Size(112, 27);
            btnDelSimSolid.TabIndex = 4;
            btnDelSimSolid.Text = "移除";
            btnDelSimSolid.UseVisualStyleBackColor = true;
            // 
            // btnAddSimSolid
            // 
            btnAddSimSolid.Location = new Point(6, 174);
            btnAddSimSolid.Name = "btnAddSimSolid";
            btnAddSimSolid.Size = new Size(112, 27);
            btnAddSimSolid.TabIndex = 4;
            btnAddSimSolid.Text = "添加";
            btnAddSimSolid.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(12, 675);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(246, 23);
            btnOK.TabIndex = 6;
            btnOK.Text = "确认并初始化";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // FormInitvar
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 711);
            Controls.Add(btnOK);
            Controls.Add(groupsource);
            Controls.Add(groupSimAir);
            Controls.Add(groupSimSolid);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(285, 750);
            MinimumSize = new Size(285, 750);
            Name = "FormInitvar";
            Text = "初始参数";
            Load += FormInitvar_Load;
            groupsource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSimSolid).EndInit();
            groupSimAir.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSimAir).EndInit();
            groupSimSolid.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupsource;
        private DataGridView dgvSource;
        private DataGridView dgvSimSolid;
        private GroupBox groupSimAir;
        private DataGridView dgvSimAir;
        private GroupBox groupSimSolid;
        private Button btnDelSource;
        private Button btnAddSource;
        private Button btnAddSimAir;
        private Button btnDelSimAir;
        private Button btnDelSimSolid;
        private Button btnAddSimSolid;
        private Button btnOK;
        private DataGridViewTextBoxColumn Source_x;
        private DataGridViewTextBoxColumn Souce_y;
        private DataGridViewTextBoxColumn Source_z;
        private DataGridViewTextBoxColumn SimSolid_x;
        private DataGridViewTextBoxColumn SimSolid_y;
        private DataGridViewTextBoxColumn SimSolid_z;
        private DataGridViewTextBoxColumn SimAir_x;
        private DataGridViewTextBoxColumn SimAir_y;
        private DataGridViewTextBoxColumn SimAir_z;
    }
}