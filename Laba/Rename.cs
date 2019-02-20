using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Laba
{
    partial class GeneralOptions
    {
        static public List<string> Pictures = new List<string>();
        public static void Func_Rename()
        {
            string PlaceToMove = Program.StartFunc() + "_Rename";
            if (!Directory.Exists(PlaceToMove))
            {
                Directory.CreateDirectory(PlaceToMove);
            }
            foreach (string Name in Pictures)
            {
                string NameAfterMove = File.GetCreationTime(Name).ToString().Replace(":", "-")
                    + Name.Substring(Name.LastIndexOf("."));
                int i = 0;
                while (File.Exists(Path.Combine(PlaceToMove, NameAfterMove)))
                {
                    i++;
                    string Format = NameAfterMove.Substring(NameAfterMove.LastIndexOf("."));
                    NameAfterMove = NameAfterMove.Remove(NameAfterMove.LastIndexOf("."));
                    if (i > 1)
                    {
                        NameAfterMove = NameAfterMove.Remove(NameAfterMove.LastIndexOf("("), 3 + (int)Math.Log10(i - 1));
                    }

                    NameAfterMove = String.Concat(NameAfterMove, "(", i, ")", Format);
                }
                File.Copy(Name, Path.Combine(PlaceToMove, NameAfterMove));
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success");
            Console.ResetColor();
        }
    }
}