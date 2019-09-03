namespace ToolUnit
{
    partial class FormKnowledgeBase
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
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_treeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加结点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加知识ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.子目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.根目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_treeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.contextMenuStrip_treeView;
            this.treeView.Location = new System.Drawing.Point(25, 22);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(173, 339);
            this.treeView.TabIndex = 0;
            // 
            // contextMenuStrip_treeView
            // 
            this.contextMenuStrip_treeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加结点ToolStripMenuItem,
            this.添加知识ToolStripMenuItem});
            this.contextMenuStrip_treeView.Name = "contextMenuStrip_treeView";
            this.contextMenuStrip_treeView.Size = new System.Drawing.Size(153, 70);
            // 
            // 添加结点ToolStripMenuItem
            // 
            this.添加结点ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.子目录ToolStripMenuItem,
            this.根目录ToolStripMenuItem,
            this.修改目录ToolStripMenuItem,
            this.删除目录ToolStripMenuItem});
            this.添加结点ToolStripMenuItem.Name = "添加结点ToolStripMenuItem";
            this.添加结点ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加结点ToolStripMenuItem.Text = "目录";
            // 
            // 添加知识ToolStripMenuItem
            // 
            this.添加知识ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.添加知识ToolStripMenuItem.Name = "添加知识ToolStripMenuItem";
            this.添加知识ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加知识ToolStripMenuItem.Text = "知识";
            // 
            // 子目录ToolStripMenuItem
            // 
            this.子目录ToolStripMenuItem.Name = "子目录ToolStripMenuItem";
            this.子目录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.子目录ToolStripMenuItem.Text = "添加子目录";
            this.子目录ToolStripMenuItem.Click += new System.EventHandler(this.子目录ToolStripMenuItem_Click);
            // 
            // 根目录ToolStripMenuItem
            // 
            this.根目录ToolStripMenuItem.Name = "根目录ToolStripMenuItem";
            this.根目录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.根目录ToolStripMenuItem.Text = "添加根目录";
            this.根目录ToolStripMenuItem.Click += new System.EventHandler(this.根目录ToolStripMenuItem_Click);
            // 
            // 修改目录ToolStripMenuItem
            // 
            this.修改目录ToolStripMenuItem.Name = "修改目录ToolStripMenuItem";
            this.修改目录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.修改目录ToolStripMenuItem.Text = "修改目录";
            // 
            // 删除目录ToolStripMenuItem
            // 
            this.删除目录ToolStripMenuItem.Name = "删除目录ToolStripMenuItem";
            this.删除目录ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除目录ToolStripMenuItem.Text = "删除目录";
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // FormKnowledgeBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 405);
            this.Controls.Add(this.treeView);
            this.Name = "FormKnowledgeBase";
            this.Text = "知识库";
            this.Load += new System.EventHandler(this.FormKnowledgeBase_Load);
            this.contextMenuStrip_treeView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_treeView;
        private System.Windows.Forms.ToolStripMenuItem 添加结点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加知识ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 子目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 根目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
    }
}