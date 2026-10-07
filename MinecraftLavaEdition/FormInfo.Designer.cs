namespace MinecraftLavaEdition
{
    partial class FormInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInfo));
            lblCoord = new Label();
            txtInfo = new TextBox();
            SuspendLayout();
            // 
            // lblCoord
            // 
            lblCoord.AutoSize = true;
            lblCoord.Location = new Point(12, 9);
            lblCoord.Name = "lblCoord";
            lblCoord.Size = new Size(43, 17);
            lblCoord.TabIndex = 0;
            lblCoord.Text = "label1";
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(0, 29);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.Size = new Size(1134, 197);
            txtInfo.TabIndex = 2;
            // 
            // FormInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1134, 226);
            Controls.Add(txtInfo);
            Controls.Add(lblCoord);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(1150, 265);
            MinimumSize = new Size(1150, 265);
            Name = "FormInfo";
            Text = "详情信息";
            Load += FormInfo_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCoord;
        private TextBox txtInfo;
    }
}