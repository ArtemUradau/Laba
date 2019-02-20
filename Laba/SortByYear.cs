using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;

namespace Laba
{
    partial class GeneralOptions
    {
        public static void Func_SortByYear()
        {
            string PlaceToMove = Program.StartFunc() + "_SortByYear";
            if (!Directory.Exists(PlaceToMove))
            {
                Directory.CreateDirectory(PlaceToMove);
            }
            foreach (string Name in Pictures)
            {
                string year = File.GetCreationTime(Name).ToString().Substring(6, 4);
                if (!Directory.Exists(Path.Combine(PlaceToMove, year)))
                {
                    Directory.CreateDirectory(Path.Combine(PlaceToMove, year));
                }
                try
                {
                    File.Copy(Name, Path.Combine(Path.Combine(PlaceToMove, year), Path.GetFileName(Name)));
                }
                catch (IOException)
                {
                    Console.WriteLine("File" + Name + " already exist");
                    continue;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success");
            Console.ResetColor();
        }
    }
}
