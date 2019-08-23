namespace ToolUnit
{
    partial class FormDataBaseSync
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_sDBPwd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_sDBUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_sDB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_dDBPwd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_dDBUser = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_dDB = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button_uptTable = new System.Windows.Forms.Button();
            this.listView_table = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_select = new System.Windows.Forms.Button();
            this.listView_TblSel = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_sync = new System.Windows.Forms.Button();
            this.button_TestDBConnect = new System.Windows.Forms.Button();
            this.button_SaveConfig = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_sDBPwd);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_sDBUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox_sDB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "源数据库";
            // 
            // textBox_sDBPwd
            // 
            this.textBox_sDBPwd.Location = new System.Drawing.Point(62, 67);
            this.textBox_sDBPwd.Name = "textBox_sDBPwd";
            this.textBox_sDBPwd.PasswordChar = '*';
            this.textBox_sDBPwd.Size = new System.Drawing.Size(121, 21);
            this.textBox_sDBPwd.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "password";
            // 
            // textBox_sDBUser
            // 
            this.textBox_sDBUser.Location = new System.Drawing.Point(62, 40);
            this.textBox_sDBUser.Name = "textBox_sDBUser";
            this.textBox_sDBUser.Size = new System.Drawing.Size(121, 21);
            this.textBox_sDBUser.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "user";
            // 
            // comboBox_sDB
            // 
            this.comboBox_sDB.FormattingEnabled = true;
            this.comboBox_sDB.Location = new System.Drawing.Point(62, 14);
            this.comboBox_sDB.Name = "comboBox_sDB";
            this.comboBox_sDB.Size = new System.Drawing.Size(121, 20);
            this.comboBox_sDB.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "DBAlias";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_dDBPwd);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBox_dDBUser);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.comboBox_dDB);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(3, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "目标数据库";
            // 
            // textBox_dDBPwd
            // 
            this.textBox_dDBPwd.Location = new System.Drawing.Point(62, 67);
            this.textBox_dDBPwd.Name = "textBox_dDBPwd";
            this.textBox_dDBPwd.PasswordChar = '*';
            this.textBox_dDBPwd.Size = new System.Drawing.Size(121, 21);
            this.textBox_dDBPwd.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "password";
            // 
            // textBox_dDBUser
            // 
            this.textBox_dDBUser.Location = new System.Drawing.Point(62, 40);
            this.textBox_dDBUser.Name = "textBox_dDBUser";
            this.textBox_dDBUser.Size = new System.Drawing.Size(121, 21);
            this.textBox_dDBUser.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "user";
            // 
            // comboBox_dDB
            // 
            this.comboBox_dDB.FormattingEnabled = true;
            this.comboBox_dDB.Location = new System.Drawing.Point(62, 14);
            this.comboBox_dDB.Name = "comboBox_dDB";
            this.comboBox_dDB.Size = new System.Drawing.Size(121, 20);
            this.comboBox_dDB.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "DBAlias";
            // 
            // button_uptTable
            // 
            this.button_uptTable.Location = new System.Drawing.Point(3, 245);
            this.button_uptTable.Name = "button_uptTable";
            this.button_uptTable.Size = new System.Drawing.Size(94, 23);
            this.button_uptTable.TabIndex = 2;
            this.button_uptTable.Text = "更新表";
            this.button_uptTable.UseVisualStyleBackColor = true;
            this.button_uptTable.Click += new System.EventHandler(this.button_uptTable_Click);
            // 
            // listView_table
            // 
            this.listView_table.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_table.FullRowSelect = true;
            this.listView_table.GridLines = true;
            this.listView_table.Location = new System.Drawing.Point(397, 19);
            this.listView_table.Name = "listView_table";
            this.listView_table.Size = new System.Drawing.Size(383, 665);
            this.listView_table.TabIndex = 3;
            this.listView_table.UseCompatibleStateImageBehavior = false;
            this.listView_table.View = System.Windows.Forms.View.Details;
            this.listView_table.DoubleClick += new System.EventHandler(this.listView_table_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "源数据库表名";
            this.columnHeader1.Width = 104;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "目标数据库表名";
            this.columnHeader2.Width = 111;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "表说明";
            this.columnHeader3.Width = 168;
            // 
            // button_select
            // 
            this.button_select.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_select.Location = new System.Drawing.Point(786, 341);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(34, 23);
            this.button_select.TabIndex = 4;
            this.button_select.Text = "→";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // listView_TblSel
            // 
            this.listView_TblSel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.listView_TblSel.FullRowSelect = true;
            this.listView_TblSel.Location = new System.Drawing.Point(826, 19);
            this.listView_TblSel.Name = "listView_TblSel";
            this.listView_TblSel.Size = new System.Drawing.Size(206, 665);
            this.listView_TblSel.TabIndex = 5;
            this.listView_TblSel.UseCompatibleStateImageBehavior = false;
            this.listView_TblSel.View = System.Windows.Forms.View.Details;
            this.listView_TblSel.DoubleClick += new System.EventHandler(this.listView_TblSel_DoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "表名";
            this.columnHeader4.Width = 190;
            // 
            // button_sync
            // 
            this.button_sync.Location = new System.Drawing.Point(103, 245);
            this.button_sync.Name = "button_sync";
            this.button_sync.Size = new System.Drawing.Size(92, 23);
            this.button_sync.TabIndex = 6;
            this.button_sync.Text = "开始同步";
            this.button_sync.UseVisualStyleBackColor = true;
            this.button_sync.Click += new System.EventHandler(this.button_sync_Click);
            // 
            // button_TestDBConnect
            // 
            this.button_TestDBConnect.Location = new System.Drawing.Point(3, 216);
            this.button_TestDBConnect.Name = "button_TestDBConnect";
            this.button_TestDBConnect.Size = new System.Drawing.Size(94, 23);
            this.button_TestDBConnect.TabIndex = 7;
            this.button_TestDBConnect.Text = "测试数据库连接";
            this.button_TestDBConnect.UseVisualStyleBackColor = true;
            this.button_TestDBConnect.Click += new System.EventHandler(this.button_TestDBConnect_Click);
            // 
            // button_SaveConfig
            // 
            this.button_SaveConfig.Location = new System.Drawing.Point(103, 216);
            this.button_SaveConfig.Name = "button_SaveConfig";
            this.button_SaveConfig.Size = new System.Drawing.Size(92, 23);
            this.button_SaveConfig.TabIndex = 8;
            this.button_SaveConfig.Text = "保存配置";
            this.button_SaveConfig.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(397, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "源数据库和目标数据库表详情列表\r\n";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(824, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "已选择的同步表列表";
            // 
            // FormDataBaseSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 690);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_SaveConfig);
            this.Controls.Add(this.button_TestDBConnect);
            this.Controls.Add(this.button_sync);
            this.Controls.Add(this.listView_TblSel);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.listView_table);
            this.Controls.Add(this.button_uptTable);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormDataBaseSync";
            this.Text = "数据库同步";
            this.Load += new System.EventHandler(this.FormDataBaseSync_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_sDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_sDBPwd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_sDBUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_dDBPwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_dDBUser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_dDB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_uptTable;
        private System.Windows.Forms.ListView listView_table;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.ListView listView_TblSel;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button button_sync;
        private System.Windows.Forms.Button button_TestDBConnect;
        private System.Windows.Forms.Button button_SaveConfig;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}