using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{

    public class NodeFile
    {
        public string Name;
        public string type;
        public string fileWay;
        public long size;
        public bool IsRead;
        public int DirIn;
        public int FileIn;
        public string Exstension;
        public List<Edge> Next;


    }
    public class Edge
    {

        public NodeFile NodeFile;

    }

    



}
