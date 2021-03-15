using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace ConsoleApp
{
    class Command
    {
        //буфер ссылок
        private NodeFile FFile = new NodeFile();
        private NodeFile FFile2 = new NodeFile();
        public string СommandInput(string command, NodeFile filename)
        {
            var NodeFile = new NodeFile();
            string com = null;
            string way = null;
            int b1 = 0;

            //обработка команды
            for (int i = 0; i < command.Length; i++)
            {
                if (command[i] != ' ')
                {
                    com = com + command[i].ToString();

                }
                else
                {
                    b1 = i + 1;
                    break;
                }

            }

            //обработка пути
            if (com == "cd")
            {

                for (int i = b1; i < command.Length; i++)
                {

                    way = way + command[i].ToString();

                }
                return way;
            }

            if (com == "copy")
            {

                for (int i = b1; i < command.Length; i++)
                {

                    way = way + command[i].ToString();

                }
                copy(filename, way);
                way = filename.fileWay;
                FFile2 = filename;
            }

            if (com == "in")
            {

                for (int i = b1; i < command.Length; i++)
                {

                    way = way + command[i].ToString();

                }
                In(filename, way);
            }
            if (com == "delete")
            {

                for (int i = b1; i < command.Length; i++)
                {

                    way = way + command[i].ToString();

                }
                way=delete(filename, way);
            }
            if (way != null)
                return way;
            else
                return filename.fileWay;
        }

    

        //куда скопировать
        public void In(NodeFile oldFile, string way)
        {
         
            var wayFile = new wayFile();

            if (FFile.type == "Directory")
            {
                //возвращает ссылку на полный список папок и подпапок
                var filedir = wayFile.wayDir(FFile.fileWay, oldFile);
                //первичное создание папки 
                try
                {
                    var copyfileway=wayFile.FileFind(way, FFile2);
                    copyfileway = wayFile.wayDir(way, copyfileway);
                    string oldname = FFile.Name;
                    bool nametrue = false;
                    //если одинаковые имена
                    for (int i = 0; i < copyfileway.Next.Count; i++)
                    {
                        if (copyfileway.Next[i].NodeFile.Name == FFile.Name)
                        {

                            for (int g = 0; g < FFile.Name.Length; g++)
                            {
                                if (FFile.Name[g].ToString() == "-")
                                {
                                    nametrue = true;
                                    break;
                                }

                            }
                            if (!nametrue)
                            {
                                FFile.Name = oldname + "-копия 1";
                                i = 0;
                            }
                            else

                            {
                                char num = oldname[oldname.Length - 5];
                                oldname = oldname.Remove(oldname.Length - 5);
                                num++;
                                FFile.Name = oldname + num + FFile.Exstension;
                                oldname = FFile.Name;
                                nametrue = false;
                                i = 0;
                            }
                        }
                    }

                    new DirectoryInfo(way).CreateSubdirectory(FFile.Name);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The process failed: {e.ToString()}");
                }
                way = way + @"/" + FFile.Name;
                //для копирования папок и всего содержимого
                wayFile.CopyDir(way, filedir);

            }
            else
            {
                try
                {
                    var copyfileway = wayFile.FileFind(way, FFile2);
                    copyfileway = wayFile.wayDir(way, copyfileway);
                    string oldname = FFile.Name;
                    bool nametrue = false;
                    //если одинаковые имена
                    for (int i = 0; i < copyfileway.Next.Count; i++)
                    {
                        if (copyfileway.Next[i].NodeFile.Name == FFile.Name)
                        {

                            for (int g = 0; g < FFile.Name.Length; g++)
                            {
                                if (FFile.Name[g].ToString() == "-")
                                {
                                    nametrue = true;
                                    break;
                                }

                            }
                            if (!nametrue)
                            {
                                FFile.Name = oldname + "-копия 1";
                                i = 0;
                            }
                            else

                            {
                                char num = oldname[oldname.Length - 1];
                                oldname = oldname.Remove(oldname.Length - 1);
                                num++;
                                FFile.Name = oldname + num;
                                oldname = FFile.Name;
                                nametrue = false;
                                i = 0;
                            }
                        }
                    }
                    new FileInfo(FFile.fileWay).CopyTo(way + @"/" + FFile.Name);
  
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The process failed: {e.ToString()}");
                    Console.WriteLine("Для продолжения нажмите любую клавишу");
                    Console.ReadLine();
                    
                }

            }
          
        }
        //что скопировать
        public void copy(NodeFile oldFile, string way)
        {
            var wayFile = new wayFile();
            //возвращяет ссылку на искомый файл или папку
            FFile = wayFile.FileFind(way, oldFile);
            

        }
        //удалить выбраный файл
        public string delete(NodeFile oldFile, string way)
        {
            try
            {
                if (File.Exists(way))
                {
                    var wayFile = new wayFile();
                    FileInfo fileInf = new FileInfo(way);
                    //для удаления файлов
                    fileInf.Delete();
                }
                if (Directory.Exists(way))
                {
                 //для удаления папок
                  Directory.Delete(way, true);

                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"The process failed: {e.ToString()}");
                Console.WriteLine("Для продолжения нажмите любую клавишу");
                Console.ReadLine();

            }
            return oldFile.fileWay;
        }

       
    }
}
