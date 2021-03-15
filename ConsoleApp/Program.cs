using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Text.Json;



namespace ConsoleApp
{

    class Program
    {
        public static string way=null;
        static void Main(string[] args)
        {

            var size = (int.Parse(ConfigurationManager.AppSettings["numOfSto"]));//изначальное значение размера страницы в app.config
            Console.SetWindowSize(int.Parse(ConfigurationManager.AppSettings["ConsoleSizeX"]), int.Parse(ConfigurationManager.AppSettings["ConsoleSizeY"]));
            Console.SetBufferSize(int.Parse(ConfigurationManager.AppSettings["ConsoleSizeX"]), int.Parse(ConfigurationManager.AppSettings["ConsoleSizeY"]));


            List<string> storyCommand = new List<string>();//лист истории комманд

            var wayFile = new wayFile();
            var NodeFile = new NodeFile();
            var OutFile = new OutFile();
            var command = new Command();


           
            var settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oldCommand.json");
            if (File.Exists(settingsPath))
            {

                var jsonControl = File.ReadAllText(settingsPath);
                if(jsonControl.ToString()=="")
                    File.WriteAllText(settingsPath, "\"\" ");

                var json = JsonSerializer.Deserialize<string>(File.ReadAllText(settingsPath));
        
                    if (json != null)
                    {
                        way = json;

                    }
                    else
                        way = null;

                    if (way == null)
                    {
                        Console.WriteLine("Введите путь");
                        way = Console.ReadLine();
                        Console.Clear();
                    }
            }
            else
            {
                        File.Create(settingsPath);
                        Console.WriteLine("Введите путь");
                        way = Console.ReadLine();
                        Console.Clear();
            }


    wayFile.wayF(way, NodeFile);//формирование что-то вроде дерева 

            OutFile.clearList();
            OutFile.FirstListNode(NodeFile);//запись в лист корня каталога
            OutFile.outFile(NodeFile);//запись в лист всех элементов каталога
            //первичный вывод листа по заданому количеству элементов на странице
            OutFile.ConsoleFirstOut((int.Parse(ConfigurationManager.AppSettings["numOfSto"])));


            
            string str = null;//строка команды и пути
            string strbuf = null;//буфер пути для записи при выходе из программы
            int strPosition = 0;//счеткик позиции для вывода стороки
            int numStoryUp = 0;//индекс истории вверх
            int numStoryDown = 0;//индекс истории вниз
         
          
            while (true)
            {
                //вывод информации о файле
                Console.SetCursorPosition(0, 28);
                Console.Write($"name-{ NodeFile.Name}, type-{ NodeFile.type}, size-{ NodeFile.size} byte, IsReadOnly-{ NodeFile.IsRead}");

                Console.SetCursorPosition(strPosition, 29);
                var key = Console.ReadKey();

                //вывод списка постранично
                if (key.Key == ConsoleKey.RightArrow|| key.Key == ConsoleKey.LeftArrow)
                {
                  
                    OutFile.ConsoleSizeOut(size, key.Key);
                    Console.SetCursorPosition(0, 29);
                    Console.Write(str);
               
                }
                //удаление символов в сторке
                if (key.Key == ConsoleKey.Backspace)
                {
                    
                    if (strPosition > 0)
                    {
                        strPosition--;
                        str = str.Remove(str.Length - 1);
                        Console.Write(" ");
                    }
                }
                //добавление символов в стоку
                else 
                {
                    if (key.Key != ConsoleKey.Enter&& key.Key != ConsoleKey.LeftArrow && key.Key != ConsoleKey.RightArrow && key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.DownArrow)
                    {
                        str = str + key.KeyChar;
                        strPosition++;
                    }
                }
             
                //выполнение команды
                if (key.Key == ConsoleKey.Enter && str!=null)
                {

                    string wayCommand=command.СommandInput(str, NodeFile);
                    NodeFile = new NodeFile();
                    wayFile.wayF(wayCommand, NodeFile);

                    Console.Clear();
                    OutFile.clearList();
                    OutFile.FirstListNode(NodeFile);//запись в лист корня каталога
                    OutFile.outFile(NodeFile);//запись в лист всех элементов каталога
                    OutFile.ConsoleFirstOut((int.Parse(ConfigurationManager.AppSettings["numOfSto"])));

                    storyCommand.Add(str);//запись истории ввода
                    strbuf = wayCommand;
                    str = null;
                    strPosition = 0;
                
                }

                //вывод из истории по стрелки вверх
                if (key.Key == ConsoleKey.UpArrow && numStoryUp<storyCommand.Count)
                {
                    strPosition = 0;
                    if (str != null)
                        //для очиски строки команды
                        for (int i = str.Length; i >= 0; i--)
                        {
                            Console.SetCursorPosition(i, 29);
                            Console.Write(" ");
                        }
                    Console.SetCursorPosition(strPosition, 29);
                    Console.Write(storyCommand[numStoryUp]);
                    strPosition = storyCommand[numStoryUp].Length;
                    str = storyCommand[numStoryUp];
                    numStoryUp++;
                }
                //вывод из истории по стрелки вниз
                if (key.Key == ConsoleKey.DownArrow && numStoryDown>=0)
                {
                    strPosition = 0;
                    numStoryDown =numStoryUp-2;
                    if (numStoryDown < 0)
                        numStoryDown = 0;
                    if (str != null)
                    
                        for (int i = str.Length; i >= 0; i--)
                        {
                            Console.SetCursorPosition(i, 29);
                            Console.Write(" ");
                        }
                    if (storyCommand.Count != 0)
                    {
                        Console.SetCursorPosition(strPosition, 29);
                        Console.Write(storyCommand[numStoryDown]);
                        strPosition = storyCommand[numStoryDown].Length;
                        str = storyCommand[numStoryDown];
                        numStoryUp = numStoryDown + 1;
                    }
                }


                //выход
                if (key.Key == ConsoleKey.Escape)
                {
                    //сохранение последнего успешного пути
                    if (strbuf != null)
                        str = strbuf;              
                    else
                        str = way;
                 

                    if (!Directory.Exists(str))
                        str = null;

                    var inJson=JsonSerializer.Serialize(str);
                    File.WriteAllText("oldCommand.json", inJson);
                   
                    break;
                }

             

            }
  
        }
    }
}

