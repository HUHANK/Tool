using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;

namespace ToolUnit
{
    public partial class FormKnowledgeBase : Form
    {
        private const string sSerializeName = "knowledgetree.msi";
        private const string sRootDir = "./knowledge_database/";
        private const string sTreeDir = "./knowledge_database/tree/";
        private List<SKnowledgeNode> m_Tree;
        public FormKnowledgeBase()
        {
            InitializeComponent();
        }

        private void FormKnowledgeBase_Load(object sender, EventArgs e)
        {
            CTool.CheckPathExistOrCreate(sTreeDir);
            deserializeTree();
            initTreeViewTable(this.treeView.Nodes, m_Tree);
        }

        private void initTreeViewTable(TreeNodeCollection Nodes, List<SKnowledgeNode> snode)
        {
            foreach(SKnowledgeNode knode in snode)
            {
                TreeNode node =  Nodes.Add(knode.name);
                if (knode.childs.Count > 0)
                {
                    initTreeViewTable(node.Nodes, knode.childs);
                }
            }
        }

        private void deserializeTree()
        {
            CSerialize ser = new CSerialize();
            ser.FileName = sRootDir + sSerializeName;
            if (File.Exists(ser.FileName))
                m_Tree = (List<SKnowledgeNode>)ser.DeSerialize();
            else
                m_Tree = new List<SKnowledgeNode>();
        }
        private void serializeTree()
        {
            CSerialize ser = new CSerialize();
            ser.FileName = sRootDir + sSerializeName;
            ser.Serialize(m_Tree);
        }

        private void 根目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inputStr = Interaction.InputBox("请输入根目录名称","根目录","目录名", -1, -1);
            inputStr = inputStr.Trim();
            if (inputStr.Length < 1)
            {
                //MessageBox.Show("您输入的目录名为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach(TreeNode node in treeView.Nodes)
            {
                if (node.Text == inputStr)
                {
                    MessageBox.Show("目录名不能重复!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            this.treeView.Nodes.Add(inputStr);
            SKnowledgeNode rnode = new SKnowledgeNode();
            rnode.name = inputStr;
            rnode.isNode = true;
            rnode.index.Add(m_Tree.Count + 1);
            rnode.filePath = sTreeDir + rnode.name + "/";
            rnode.createFilePath();
            m_Tree.Add(rnode);
            serializeTree();
        }

        private void 子目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selNode = this.treeView.SelectedNode;
            if (selNode == null)
            {
                MessageBox.Show("请选择父目录!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string inputStr = Interaction.InputBox("请输入子目录名称", "子目录", "目录名", -1, -1);
            inputStr = inputStr.Trim();
            if (inputStr.Length < 1)
            {
                //MessageBox.Show("您输入的目录名为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach(TreeNode node in selNode.Nodes)
            {
                if (node.Text == inputStr)
                {
                    MessageBox.Show("目录名不能重复!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            selNode.Nodes.Add(inputStr);
            if ( !selNode.IsExpanded )
            {
                selNode.Expand();
            }
            
            foreach (TreeNode node in selNode.Nodes)
            {
                if (node.Text == inputStr)
                {
                    treeView.SelectedNode = node;
                    return;
                }
            }

            /**/
            List<string> LUALS = new List<string>();
            //node = selNode;
            //while(node != null)
            //{
            //    LUALS.Add(node.Text);
            //    node = node.Parent;
            //}



        }
    }
}
