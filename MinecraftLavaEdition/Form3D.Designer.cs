namespace MinecraftLavaEdition
{
    partial class Form3D
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3D));
            pic3D = new PictureBox();
            trkRange = new TrackBar();
            labsize = new Label();
            pictureBoxL = new PictureBox();
            labelL = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pic3D).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkRange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pic3D
            // 
            pic3D.Dock = DockStyle.Bottom;
            pic3D.Location = new Point(0, 40);
            pic3D.Name = "pic3D";
            pic3D.Size = new Size(654, 411);
            pic3D.TabIndex = 0;
            pic3D.TabStop = false;
            // 
            // trkRange
            // 
            trkRange.Cursor = Cursors.Hand;
            trkRange.Location = new Point(67, 9);
            trkRange.Maximum = 80;
            trkRange.Minimum = 5;
            trkRange.Name = "trkRange";
            trkRange.Size = new Size(104, 45);
            trkRange.TabIndex = 2;
            trkRange.Value = 20;
            // 
            // labsize
            // 
            labsize.AutoSize = true;
            labsize.Location = new Point(7, 12);
            labsize.Name = "labsize";
            labsize.Size = new Size(56, 17);
            labsize.TabIndex = 4;
            labsize.Text = "空间尺寸";
            // 
            // pictureBoxL
            // 
            pictureBoxL.Image = Properties.Resources.Left;
            pictureBoxL.Location = new Point(492, 12);
            pictureBoxL.Name = "pictureBoxL";
            pictureBoxL.Size = new Size(18, 20);
            pictureBoxL.TabIndex = 5;
            pictureBoxL.TabStop = false;
            // 
            // labelL
            // 
            labelL.AutoSize = true;
            labelL.Location = new Point(510, 14);
            labelL.Name = "labelL";
            labelL.Size = new Size(32, 17);
            labelL.TabIndex = 6;
            labelL.Text = "平移";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(563, 14);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 8;
            label1.Text = "缩放";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Middle;
            pictureBox1.Location = new Point(545, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(18, 20);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(616, 15);
            label2.Name = "label2";
            label2.Size = new Size(32, 17);
            label2.TabIndex = 10;
            label2.Text = "旋转";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Right;
            pictureBox2.Location = new Point(598, 13);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(18, 20);
            pictureBox2.TabIndex = 9;
            pictureBox2.TabStop = false;
            // 
            // Form3D
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(654, 451);
            Controls.Add(label2);
            Controls.Add(pictureBox2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(labelL);
            Controls.Add(pictureBoxL);
            Controls.Add(labsize);
            Controls.Add(pic3D);
            Controls.Add(trkRange);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(670, 490);
            MinimumSize = new Size(670, 490);
            Name = "Form3D";
            Text = "三维视图";
            ((System.ComponentModel.ISupportInitialize)pic3D).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkRange).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxL).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pic3D;
        private TrackBar trkRange;
        private Label labsize;
        private PictureBox pictureBoxL;
        private Label labelL;
        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private PictureBox pictureBox2;
    }
}