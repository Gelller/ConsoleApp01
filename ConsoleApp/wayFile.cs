using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;




namespace ConsoleApp
{
    class wayFile
    {
        
      

        private int i=0;//cчетчик глубины
        private NodeFile FFile = new NodeFile();
       
        public void wayF(string way, NodeFile filename)
        {
            
            try
            {
                DirectoryInfo dir = new DirectoryInfo(way);
                FileInfo fileInf = new FileInfo(way);
                //условие глубины просмотра 1-каталоги первого уровня, 2-каталоги второго уровня итд
                if (i < (int.Parse(ConfigurationManager.AppSettings["ViewingDepth"])))
                {
                    var startFile = filename;
                    if (Directory.Exists(way))
                    {
                        //заполение класса корневого элемента         
                        startFile.Name = dir.Name;
                        startFile.type = fileInf.Attributes.ToString();
                        startFile.fileWay = way;
                        startFile.IsRead = fileInf.IsReadOnly;
                        startFile.DirIn = dir.GetDirectories().Length;
                        startFile.FileIn = dir.GetFiles().Length;
                        startFile.size = size(dir);

                   
                        string[] directories = Directory.GetDirectories(way);
                        string[] files = Directory.GetFiles(way);
                        if (dir.GetDirectories() != null)
                        {
                            int j = 0;
                            startFile.Next = new List<Edge>();
                            foreach (var item in dir.GetDirectories())
                            {
                               
                                fileInf = new FileInfo(item.FullName);  
                                //добавление элементов в лист (смысл в том что начальный элемент содержит ссылки на входящии в него элементы)
                                startFile.Next.Add(new Edge() { NodeFile = new NodeFile { fileWay = item.FullName, Name = item.Name, type = fileInf.Attributes.ToString(), } });
                                i++;
                                wayF(item.FullName, startFile.Next[j].NodeFile);
                                i--;
                                j++;
                            }

                           

                        }

                        //если в папке были файлы они тоже добавляются в лист папки в которой они находятся
                        if (files.Length != 0)
                        {
                            if (startFile.Next == null)
                                startFile.Next = new List<Edge>();
                            for (int j = 0; j < files.Length; j++)
                            {

                                fileInf = new FileInfo(files[j]);
                                startFile.Next.Add(new Edge() { NodeFile = new NodeFile { fileWay = files[j], type = fileInf.Attributes.ToString(), Name = fileInf.Name, size = fileInf.Length, IsRead = fileInf.IsReadOnly, Exstension = fileInf.Extension } }); 
                            }

                        }

                    }
                    else
                    {
                        Console.SetCursorPosition(0, 29);
                        Console.Write("Не существует, введите путь");
                        Console.WriteLine();
                        Program.way= Console.ReadLine();
                        Console.Clear();
                        wayF(Program.way, filename);
                    }
                    
                }
               
                    
                
            }
            
            catch (UnauthorizedAccessException)
            {
               ;
                    
            }
            catch (Exception)
            {
                ;
            }

        }

         



        //подсчет размера папки
        private long size(DirectoryInfo dir)
        {

            long Size = 0;
            FileInfo[] fis = dir.GetFiles();
            foreach (FileInfo fi in fis)
            {
                try
                {
                    Size += fi.Length;
                }
                //Данное исключение делается для пропуска папок к которым нет доступа
                catch (UnauthorizedAccessException)
                {
                    ;
                }
            }
            DirectoryInfo[] dis = dir.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                try
                {
                    Size += size(di);
                }
                //Данное исключение делается для пропуска папок к которым нет доступа
                catch (UnauthorizedAccessException)
                {
                    ;
                }
            }
            return (Size);
        }



        //поиск файла или папки в списке
        public NodeFile FileFind(string way, NodeFile filename)
        {
            
            try
            { 
                var startFile = filename;
            
                if (i < (int.Parse(ConfigurationManager.AppSettings["ViewingDepth"])))
                {
               
                    var findWay = way;
                    var Startway = startFile.fileWay;

                    if (Directory.Exists(Startway))
                    {
                        string[] directories = Directory.GetDirectories(Startway);
                        string[] files = Directory.GetFiles(Startway);
                        if (directories.Length != 0)
                        {
                       
                            for (int j = 0; j < directories.Length; j++)
                            {
                                if (startFile.Next[j].NodeFile.fileWay == findWay)
                                {
                                    FFile = startFile.Next[j].NodeFile;
                                    break;
                                }   
                                i++;
                                FileFind(findWay, startFile.Next[j].NodeFile);

                            }


                        }

                        if (files.Length != 0)
                        {

                            for (int j = 0; j < directories.Length + files.Length; j++)
                            {
                                if (startFile.Next[j].NodeFile.fileWay == findWay)
                                {
                                    FFile = startFile.Next[j].NodeFile;
                                    return FFile;
                                }
                          
                            }
                        }
                  
                    }
             
              
                 
                
                }
                
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("The file or directory cannot be found.");
                Console.WriteLine("Для продолжения нажмите любую клавишу");
                Console.ReadLine();
            }
            return FFile;
        }

        //рекурсивное копирование для папок
        public NodeFile CopyDir(string way, NodeFile filename)
        {
            try
            {
                var startFile = filename;



                var findWay = way;
                var Startway = startFile.fileWay;

                if (Directory.Exists(Startway))
                {
                    string[] directories = Directory.GetDirectories(Startway);
                    string[] files = Directory.GetFiles(Startway);
                    if (directories.Length != 0)
                    {


                        for (int j = 0; j < directories.Length; j++)
                        {
                            new DirectoryInfo(way).CreateSubdirectory(startFile.Next[j].NodeFile.Name);
                            CopyDir(way + @"/" + startFile.Next[j].NodeFile.Name, startFile.Next[j].NodeFile);

                        }


                    }

                    if (files.Length != 0)
                    {
                        for (int j = 0; j < files.Length; j++)
                        {
                            var fileInf = new FileInfo(files[j]);
                            new FileInfo(fileInf.FullName).CopyTo(way + @"\" + fileInf.Name);
                        }
                    }

                }

            }
            catch (FileNotFoundException)
            {
                ;
            }
            catch (UnauthorizedAccessException)
            {
                ;
            }
            return FFile;

            

        }

        //функция для создание списка node по пути, почти повторяет функцию wayF за исключением того что нет контроля глубины рекурсии, это необходими для того чтобы скопировались все катологи и подкатологи
        public NodeFile wayDir(string way, NodeFile filename)
        {
            try
            {
                var startFile = filename;

                //заполение класса корневого элемента
                FileInfo fileInf = new FileInfo(way);
                DirectoryInfo dir = new DirectoryInfo(way);
                startFile.Name = fileInf.Name;
                startFile.type = fileInf.Attributes.ToString();
                startFile.fileWay = way;

                if (Directory.Exists(way))
                {
                    string[] directories = Directory.GetDirectories(way);
                    string[] files = Directory.GetFiles(way);

                       
                        if (dir.GetDirectories() != null)
                        {
                            int j = 0;
                            startFile.Next = new List<Edge>();
                            foreach (var item in dir.GetDirectories())
                            {
                                fileInf = new FileInfo(item.FullName);
                                //добавление элементов в лист (смысл в том что начальный элемент содержит ссылки на входящии в него элементы)
                                startFile.Next.Add(new Edge() { NodeFile = new NodeFile { fileWay = item.FullName, Name = item.Name } });
                                i++;
                                 wayDir(item.FullName, startFile.Next[j].NodeFile);
                                i--;
                                j++;
                            }

                        }



                        if (files.Length != 0)
                        {
                            if (startFile.Next == null)
                                startFile.Next = new List<Edge>();
                            for (int j = 0; j < files.Length; j++)
                            {

                                fileInf = new FileInfo(files[j]);
                                startFile.Next.Add(new Edge() { NodeFile = new NodeFile { fileWay = files[j], type = fileInf.Attributes.ToString(), Name = fileInf.Name, Exstension = fileInf.Extension } }); ;
                            }
                        }

                }

            }
            catch (FileNotFoundException)
            {
                ;
            }
            catch (UnauthorizedAccessException)
            {
                ;
            }
           return filename;

        }
            
      

    }
}