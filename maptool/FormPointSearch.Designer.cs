
namespace maptool
{
    partial class FormPointSearch
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
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textZX = new System.Windows.Forms.TextBox();
            this.textZY = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.textRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(300, 295);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 29);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "X座標";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y座標";
            // 
            // textZX
            // 
            this.textZX.Location = new System.Drawing.Point(147, 47);
            this.textZX.Name = "textZX";
            this.textZX.Size = new System.Drawing.Size(190, 23);
            this.textZX.TabIndex = 3;
            // 
            // textZY
            // 
            this.textZY.Location = new System.Drawing.Point(147, 98);
            this.textZY.Name = "textZY";
            this.textZY.Size = new System.Drawing.Size(190, 23);
            this.textZY.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(12, 295);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 29);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "検索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(231, 199);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(106, 29);
            this.btnPaste.TabIndex = 6;
            this.btnPaste.Text = "地点貼り付け";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // textRate
            // 
            this.textRate.Location = new System.Drawing.Point(246, 144);
            this.textRate.Name = "textRate";
            this.textRate.Size = new System.Drawing.Size(91, 23);
            this.textRate.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "倍率";
            // 
            // FormPointSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 337);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textRate);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.textZY);
            this.Controls.Add(this.textZX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormPointSearch";
            this.Text = "座標検索";
            this.Load += new System.EventHandler(this.FormPointSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textZX;
        private System.Windows.Forms.TextBox textZY;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.TextBox textRate;
        private System.Windows.Forms.Label label3;
    }
}