namespace MinecraftLavaEdition
{
    partial class FormDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplay));
            picMap = new PictureBox();
            numY = new NumericUpDown();
            lblY = new Label();
            ((System.ComponentModel.ISupportInitialize)picMap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numY).BeginInit();
            SuspendLayout();
            // 
            // picMap
            // 
            picMap.Dock = DockStyle.Fill;
            picMap.Location = new Point(0, 0);
            picMap.Name = "picMap";
            picMap.Size = new Size(474, 451);
            picMap.SizeMode = PictureBoxSizeMode.CenterImage;
            picMap.TabIndex = 0;
            picMap.TabStop = false;
            // 
            // numY
            // 
            numY.Location = new Point(27, 4);
            numY.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            numY.Name = "numY";
            numY.Size = new Size(120, 23);
            numY.TabIndex = 1;
            // 
            // lblY
            // 
            lblY.AutoSize = true;
            lblY.Location = new Point(3, 6);
            lblY.Name = "lblY";
            lblY.Size = new Size(23, 17);
            lblY.TabIndex = 2;
            lblY.Text = "y=";
            // 
            // FormDisplay
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(474, 451);
            Controls.Add(lblY);
            Controls.Add(numY);
            Controls.Add(picMap);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(490, 490);
            Name = "FormDisplay";
            Text = "平面视图";
            Load += FormDisplay_Load;
            ((System.ComponentModel.ISupportInitialize)picMap).EndInit();
            ((System.ComponentModel.ISupportInitialize)numY).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picMap;
        private NumericUpDown numY;
        private Label lblY;
    }
}