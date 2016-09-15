namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.tbThresholdMin = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbThresholdMax = new System.Windows.Forms.TextBox();
            this.tbCannyThreshold = new System.Windows.Forms.TextBox();
            this.tbCannyThresholdLinking = new System.Windows.Forms.TextBox();
            this.labelx = new System.Windows.Forms.Label();
            this.labely = new System.Windows.Forms.Label();
            this.tbMinOuterRectArea = new System.Windows.Forms.TextBox();
            this.tbMaxMarkArea = new System.Windows.Forms.TextBox();
            this.labelz = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(726, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbThresholdMin
            // 
            this.tbThresholdMin.Location = new System.Drawing.Point(90, 12);
            this.tbThresholdMin.Name = "tbThresholdMin";
            this.tbThresholdMin.Size = new System.Drawing.Size(96, 20);
            this.tbThresholdMin.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 129);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 339);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(418, 129);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 339);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "threshold min";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "threshold max";
            // 
            // tbThresholdMax
            // 
            this.tbThresholdMax.Location = new System.Drawing.Point(90, 38);
            this.tbThresholdMax.Name = "tbThresholdMax";
            this.tbThresholdMax.Size = new System.Drawing.Size(96, 20);
            this.tbThresholdMax.TabIndex = 2;
            // 
            // tbCannyThreshold
            // 
            this.tbCannyThreshold.Location = new System.Drawing.Point(316, 9);
            this.tbCannyThreshold.Name = "tbCannyThreshold";
            this.tbCannyThreshold.Size = new System.Drawing.Size(96, 20);
            this.tbCannyThreshold.TabIndex = 2;
            // 
            // tbCannyThresholdLinking
            // 
            this.tbCannyThresholdLinking.Location = new System.Drawing.Point(316, 35);
            this.tbCannyThresholdLinking.Name = "tbCannyThresholdLinking";
            this.tbCannyThresholdLinking.Size = new System.Drawing.Size(96, 20);
            this.tbCannyThresholdLinking.TabIndex = 2;
            // 
            // labelx
            // 
            this.labelx.AutoSize = true;
            this.labelx.Location = new System.Drawing.Point(193, 12);
            this.labelx.Name = "labelx";
            this.labelx.Size = new System.Drawing.Size(83, 13);
            this.labelx.TabIndex = 6;
            this.labelx.Text = "cannyThreshold";
            this.labelx.Click += new System.EventHandler(this.label1_Click);
            // 
            // labely
            // 
            this.labely.AutoSize = true;
            this.labely.Location = new System.Drawing.Point(193, 41);
            this.labely.Name = "labely";
            this.labely.Size = new System.Drawing.Size(117, 13);
            this.labely.TabIndex = 7;
            this.labely.Text = "cannyThresholdLinking";
            // 
            // tbMinOuterRectArea
            // 
            this.tbMinOuterRectArea.Location = new System.Drawing.Point(541, 6);
            this.tbMinOuterRectArea.Name = "tbMinOuterRectArea";
            this.tbMinOuterRectArea.Size = new System.Drawing.Size(96, 20);
            this.tbMinOuterRectArea.TabIndex = 2;
            // 
            // tbMaxMarkArea
            // 
            this.tbMaxMarkArea.Location = new System.Drawing.Point(541, 32);
            this.tbMaxMarkArea.Name = "tbMaxMarkArea";
            this.tbMaxMarkArea.Size = new System.Drawing.Size(96, 20);
            this.tbMaxMarkArea.TabIndex = 2;
            // 
            // labelz
            // 
            this.labelz.AutoSize = true;
            this.labelz.Location = new System.Drawing.Point(418, 9);
            this.labelz.Name = "labelz";
            this.labelz.Size = new System.Drawing.Size(94, 13);
            this.labelz.TabIndex = 6;
            this.labelz.Text = "minOuterRectArea";
            this.labelz.Click += new System.EventHandler(this.label1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(418, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "maxMarkArea";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 480);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labely);
            this.Controls.Add(this.labelz);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.tbMaxMarkArea);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbMinOuterRectArea);
            this.Controls.Add(this.tbCannyThresholdLinking);
            this.Controls.Add(this.tbCannyThreshold);
            this.Controls.Add(this.tbThresholdMax);
            this.Controls.Add(this.tbThresholdMin);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbThresholdMin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbThresholdMax;
        private System.Windows.Forms.TextBox tbCannyThreshold;
        private System.Windows.Forms.TextBox tbCannyThresholdLinking;
        private System.Windows.Forms.Label labelx;
        private System.Windows.Forms.Label labely;
        private System.Windows.Forms.TextBox tbMinOuterRectArea;
        private System.Windows.Forms.TextBox tbMaxMarkArea;
        private System.Windows.Forms.Label labelz;
        private System.Windows.Forms.Label label4;
    }
}

