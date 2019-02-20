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
        public static void Func_Watermark()
        {
            string PlaceToMove = Program.StartFunc() + "_Watermark";
            if (!Directory.Exists(PlaceToMove))
            {
                Directory.CreateDirectory(PlaceToMove);
            }
            foreach (string Name in Pictures)
            {
                string Date = File.GetCreationTime(Name).ToString();
                Image img = Image.FromFile(Name);
                Graphics newGraphics = Graphics.FromImage(img);
                PointF NewPoint = new PointF();
                NewPoint.X = img.Width - (Date.Length * 10);
                newGraphics.DrawString($"{File.GetCreationTime(Name)}", new System.Drawing.Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel), new SolidBrush(Color.Red), NewPoint, new StringFormat(StringFormatFlags.NoWrap));
                try
                {
                    img.Save(Path.Combine(PlaceToMove, Path.GetFileName(Name)), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception)
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
