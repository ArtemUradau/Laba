using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace Laba
{
    struct Coordinates
    {
        public uint LatitudeDeegres;
        public uint LatitudeMinutes;
        public float LatitudeSeconds;
        public uint LongitudeDeegres;
        public uint LongitudeMinutes;
        public float LongitudeSeconds;
        public string FileName;
        public Coordinates(uint la_d, uint la_m, float la_s, uint lo_d, uint lo_m, float lo_s, string f_n)
        {
            LatitudeDeegres = la_d;
            LatitudeMinutes = la_m;
            LatitudeSeconds = la_s;
            LongitudeDeegres = lo_d;
            LongitudeMinutes = lo_m;
            LongitudeSeconds = lo_s;
            FileName = f_n;
        }
    }
    partial class GeneralOptions
    {
        static private List<Coordinates> Coords = new List<Coordinates>();
        public static void Func_SortByPlace()
        {
            string PlaceToMove = Program.StartFunc() + "_SortByPlace";
            if (!Directory.Exists(PlaceToMove))
            {
                Directory.CreateDirectory(PlaceToMove);
            }
            foreach (string Name in Pictures)
            {
                Image img = Image.FromFile(Name);

                try
                {
                    PropertyItem pi = img.GetPropertyItem(0x0002);
                    PropertyItem pi2 = img.GetPropertyItem(0x0004);
                    uint degreesNumerator = BitConverter.ToUInt32(pi.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(pi.Value, 4);
                    uint minutesNumerator = BitConverter.ToUInt32(pi.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(pi.Value, 12);
                    float secondsNumerator = BitConverter.ToSingle(pi.Value, 16);
                    float secondsDenominator = BitConverter.ToSingle(pi.Value, 20);
                    uint degreesNumerator2 = BitConverter.ToUInt32(pi2.Value, 0);
                    uint degreesDenominator2 = BitConverter.ToUInt32(pi2.Value, 4);
                    uint minutesNumerator2 = BitConverter.ToUInt32(pi2.Value, 8);
                    uint minutesDenominator2 = BitConverter.ToUInt32(pi2.Value, 12);
                    float secondsNumerator2 = BitConverter.ToSingle(pi2.Value, 16);
                    float secondsDenominator2 = BitConverter.ToSingle(pi2.Value, 20);
                    Coords.Add(new Coordinates(degreesNumerator / degreesDenominator, minutesNumerator / minutesDenominator, secondsNumerator / secondsDenominator,
     degreesNumerator2 / degreesDenominator2, minutesNumerator2 / minutesDenominator2, secondsNumerator2 / secondsDenominator2, Name));
                }
                catch (System.ArgumentException)
                {
                    Console.WriteLine("File" + Name + "doesn't have any GPS tags");
                    continue;
                }
            }
            foreach (Coordinates C in Coords)
            {
                string Place = Request(C);
                if (!Directory.Exists(Path.Combine(PlaceToMove, Place)))
                {
                    Directory.CreateDirectory(Path.Combine(PlaceToMove, Place));
                }
                try
                {
                    File.Copy(C.FileName, Path.Combine(Path.Combine(PlaceToMove, Place), Path.GetFileName(C.FileName)));
                }
                catch (IOException)
                {
                    Console.WriteLine("File" + C.FileName + " already exist");
                    continue;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success");
            Console.ResetColor();
        }
        static private string Request(Coordinates coords)
        {
            string Latitude = coords.LatitudeDeegres + "." + coords.LatitudeMinutes + (int)coords.LatitudeSeconds;
            string Longitude = coords.LongitudeDeegres + "." + coords.LongitudeMinutes + (int)coords.LongitudeSeconds;
            string RequestString = "https://geocode-maps.yandex.ru/1.x/?geocode=" + Longitude + "," + Latitude;
            WebRequest Request = WebRequest.Create(RequestString);
            WebResponse response = Request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("<formatted>"))
                        {
                            response.Close();
                            return FormatRequestAnswer(line);
                        }
                    }
                }
            }
            return "";
        }
        static string FormatRequestAnswer(string FullAdress)
        {
            FullAdress = FullAdress.Substring(0, FullAdress.LastIndexOf("<"));
            if (FullAdress.LastIndexOf(",") != 1)
                FullAdress = FullAdress.Substring(FullAdress.LastIndexOf(",") + 2);
            else FullAdress = FullAdress.Substring(FullAdress.LastIndexOf(">") + 1);
            string FirstChar = FullAdress[0].ToString();
            FirstChar = FirstChar.ToUpper();
            return String.Concat(FirstChar, FullAdress.Substring(1));
        }
    }
}
