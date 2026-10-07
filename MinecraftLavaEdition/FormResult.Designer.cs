namespace MinecraftLavaEdition
{
    partial class FormResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResult));
            dgvResult = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvResult).BeginInit();
            SuspendLayout();
            // 
            // dgvResult
            // 
            dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResult.Dock = DockStyle.Fill;
            dgvResult.Location = new Point(0, 0);
            dgvResult.Name = "dgvResult";
            dgvResult.RowHeadersWidth = 51;
            dgvResult.RowTemplate.Height = 29;
            dgvResult.Size = new Size(269, 711);
            dgvResult.TabIndex = 0;
            // 
            // FormResult
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 711);
            Controls.Add(dgvResult);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(285, 750);
            MinimumSize = new Size(285, 750);
            Name = "FormResult";
            Text = "输出结果";
            Load += FormResult_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResult).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvResult;
    }
}