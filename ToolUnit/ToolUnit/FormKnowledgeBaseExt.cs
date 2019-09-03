using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolUnit
{
    /****************************STRUCT******************************/
    [Serializable]
    class SKnowledgeNode
    {
        public string guid;
        public string name;
        public List<int> index;
        public bool isNode; /*是否是结点*/
        public bool isLeaf; /*是否是叶结点*/
        public string filePath; /*对应的路径*/

        public SKnowledgeNode parent;
        public List<SKnowledgeNode> childs;

        public SKnowledgeNode()
        {
            name = "";
            index = new List<int>();
            isNode = false;
            isLeaf = false;
            filePath = "";
            parent = null;
            childs = new List<SKnowledgeNode>();
            guid = Guid.NewGuid().ToString();
        }

        public void createFilePath()
        {
            if (filePath.Length < 1) return;
            CTool.CheckPathExistOrCreate(filePath);
            if (isNode) return;
        }
    }
    /****************************STRUCT******************************/
}
