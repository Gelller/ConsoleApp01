using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApp
{
    class OutFile
    {
        private int i; //для вывода листа в консоль

        private List<string> listOut = new List<string>();
   

        public void clearList()
        {
            listOut.Clear();
        }


        //для записи в лист корня всего дерева
        public void FirstListNode(NodeFile first)
        {
            listOut.Add(first.fileWay);
          
        }

        //добавления в лист элементы дерева каталога
        public void outFile(NodeFile oldFile)
        {

            if (oldFile.Next != null)
            {

                for (int i = 0; i < oldFile.Next.Count(); i++)
                {
                    var nextFile = oldFile.Next[i];                  
                    listOut.Add(nextFile.NodeFile.fileWay);
                    outFile(nextFile.NodeFile);
                }


            }

            
        }

        //первичный вывод в консоль списка файлов по заданому количеству элементов
        public void ConsoleFirstOut(int size)
        {
            for (i=0;i< listOut.Count && i < size;i++)
            {
                Console.WriteLine(listOut[i]);

            }
        }


        //вывод в консоль следующий страницы по заданому количеству элементов
        public void ConsoleSizeOut(int size, ConsoleKey key)
        {
            int buffetSize = 0;
            buffetSize = i;
            if (key == ConsoleKey.RightArrow)
            {
                if (i != listOut.Count)
                {
                    Console.Clear();
                    for (i = buffetSize; i < listOut.Count && i < buffetSize + size; i++)
                    {
                        Console.WriteLine(listOut[i]);
                    }
                    buffetSize = buffetSize + size;
                }
            }
           
            if (key == ConsoleKey.LeftArrow)
            {
                buffetSize = buffetSize - size - size;
                //когда надо перейти с последний старицы на предпоследнию
                if(i== listOut.Count)
                {
                    int ost = 0;
                    if (i % (int.Parse(ConfigurationManager.AppSettings["numOfSto"])) != 0)
                        ost = i % (int.Parse(ConfigurationManager.AppSettings["numOfSto"]));
                    else
                        ost = size;

                    buffetSize = buffetSize - ost + size;

                }

                if (buffetSize >= 0)
                {
                    Console.Clear();
                    for (i = buffetSize; i < listOut.Count && i < buffetSize + size; i++)
                    {
                        Console.WriteLine(listOut[i]);
                    }
                    buffetSize = buffetSize + size;
                }
            }
        }

        public void Print(NodeFile NodeFile)
        {
            
            var OutFile = new OutFile();
            Console.Clear();
            OutFile.clearList();
            OutFile.FirstListNode(NodeFile);//запись в лист корня каталога
            OutFile.outFile(NodeFile);//запись в лист всех элементов каталога
            //первичный вывод листа по заданому количеству элементов на странице
            OutFile.ConsoleFirstOut((int.Parse(ConfigurationManager.AppSettings["numOfSto"])));

        }
    }
}
