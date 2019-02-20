#undef  TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Laba
{
    class Program
    {
        public static string StartFunc()
        {
            string SelectedPath = "";
#if TEST
            SelectedPath += @"d:\Artem\Pictures";
#endif
            while (SelectedPath == "")
            {
                Console.WriteLine("Specify the path to the folder: ");
                SelectedPath = Console.ReadLine();

                if (!Directory.Exists(SelectedPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The folder " + SelectedPath + " doesn't exist");
                    Console.ResetColor();
                    SelectedPath = "";
                }
            }

            string PlaceToMove = Path.GetDirectoryName(SelectedPath);
            string FolderName = Path.GetFileName(SelectedPath);
            PlaceToMove = Path.Combine(PlaceToMove, FolderName);

            GeneralOptions.Pictures.AddRange(Directory.GetFiles(SelectedPath, "*.jpg"));
            GeneralOptions.Pictures.AddRange(Directory.GetFiles(SelectedPath, "*.png"));

            return PlaceToMove;
        }
        static void Main(string[] args)
        {
            int Option;
            Console.WriteLine("Choose the operation:");
            Console.WriteLine("1.Rename image \n2.Add watermark \n3.Sort by year \n4.Sort by place");
            Option = Int32.Parse(Console.ReadLine());

            switch (Option)
            {
                case 1:
                    GeneralOptions.Func_Rename();
                    break;
                case 2:
                    GeneralOptions.Func_Watermark();
                    break;
                case 3:
                    GeneralOptions.Func_SortByYear();
                    break;
                case 4:
                    GeneralOptions.Func_SortByPlace();
                    break;
                default:
                    Console.WriteLine("");
                    break;
            }
            Console.ReadLine();
        }
    }
}
