using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ToolUnit
{
    class FullTextSearchExt
    {
    }

    class CFileSearchDetail
    {
        /*文件名*/
        private string m_FileName;
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }
        /*行号+行的内容*/
        private Dictionary<int, string> m_SearchResults;
        /*正则匹配的内容*/
        private string m_Pattern; 
        public string Pattern
        {
            get { return m_Pattern; }
            set { m_Pattern = value; }
        }
        /*是否匹配到内容*/
        private bool m_isMatched;
        public bool IsMatched
        {
            get { return m_isMatched; }
        }

        public CFileSearchDetail()
        {

        }

    }
}
