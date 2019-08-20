namespace ToolUnit
{
    partial class FormSearchFiles
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.button_setPath = new System.Windows.Forms.Button();
            this.button_search = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_searchTxt = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox_UL = new System.Windows.Forms.CheckBox();
            this.checkBox_WM = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_cancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "检索目录";
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Location = new System.Drawing.Point(78, 9);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.Size = new System.Drawing.Size(455, 21);
            this.textBox_filePath.TabIndex = 1;
            this.textBox_filePath.Text = "D:\\";
            // 
            // button_setPath
            // 
            this.button_setPath.Location = new System.Drawing.Point(548, 8);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(44, 23);
            this.button_setPath.TabIndex = 2;
            this.button_setPath.Text = "...";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(436, 64);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(75, 23);
            this.button_search.TabIndex = 3;
            this.button_search.Text = "开始检索";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "检索文件字符";
            // 
            // textBox_searchTxt
            // 
            this.textBox_searchTxt.Location = new System.Drawing.Point(104, 65);
            this.textBox_searchTxt.Name = "textBox_searchTxt";
            this.textBox_searchTxt.Size = new System.Drawing.Size(326, 21);
            this.textBox_searchTxt.TabIndex = 5;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(15, 94);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(577, 400);
            this.listBox1.TabIndex = 6;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // checkBox_UL
            // 
            this.checkBox_UL.AutoSize = true;
            this.checkBox_UL.Location = new System.Drawing.Point(15, 40);
            this.checkBox_UL.Name = "checkBox_UL";
            this.checkBox_UL.Size = new System.Drawing.Size(84, 16);
            this.checkBox_UL.TabIndex = 7;
            this.checkBox_UL.Text = "匹配大小写";
            this.checkBox_UL.UseVisualStyleBackColor = true;
            // 
            // checkBox_WM
            // 
            this.checkBox_WM.AutoSize = true;
            this.checkBox_WM.Checked = true;
            this.checkBox_WM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_WM.Location = new System.Drawing.Point(106, 40);
            this.checkBox_WM.Name = "checkBox_WM";
            this.checkBox_WM.Size = new System.Drawing.Size(84, 16);
            this.checkBox_WM.TabIndex = 8;
            this.checkBox_WM.Text = "启用匹配符";
            this.checkBox_WM.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(462, 36);
            this.progressBar1.MarqueeAnimationSpeed = 50;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(130, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // button_cancle
            // 
            this.button_cancle.Location = new System.Drawing.Point(517, 64);
            this.button_cancle.Name = "button_cancle";
            this.button_cancle.Size = new System.Drawing.Size(75, 23);
            this.button_cancle.TabIndex = 10;
            this.button_cancle.Text = "取消";
            this.button_cancle.UseVisualStyleBackColor = true;
            this.button_cancle.Click += new System.EventHandler(this.button_cancle_Click);
            // 
            // FormSearchFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 497);
            this.Controls.Add(this.button_cancle);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkBox_WM);
            this.Controls.Add(this.checkBox_UL);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox_searchTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.button_setPath);
            this.Controls.Add(this.textBox_filePath);
            this.Controls.Add(this.label1);
            this.Name = "FormSearchFiles";
            this.Text = "文件检索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSearchFiles_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_filePath;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_searchTxt;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox_UL;
        private System.Windows.Forms.CheckBox checkBox_WM;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button_cancle;
    }
}