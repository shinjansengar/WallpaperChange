using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WallpaperChange
{
    class Program
    {
        static void Main(string[] args)
        {
            GetImageAsync(args[1]);
            
            string photo = @"C:\Users\shinj\source\repos\WallpaperChange\WallpaperChange\bin\Debug\net5.0\output.jpg";
            DisplayPicture(photo);
        }

        public static async Task GetImageAsync(string imageAddress)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(imageAddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    var bitmap = await response.Content.ReadAsByteArrayAsync();
               
                    using (Image image = Image.FromStream(new MemoryStream(bitmap)))
                    {
                        image.Save("output.jpg", ImageFormat.Jpeg);  
                    }
                }

            }


        }


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);

        private const uint SPI_SETDESKWALLPAPER = 0x14;
        private const uint SPIF_UPDATEINIFILE = 0x1;
        private const uint SPIF_SENDWININICHANGE = 0x2;

        private static void DisplayPicture(string file_name)
        {
            uint flags = 0;
            if (!SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0, file_name, flags))
            {
                Console.WriteLine("Error");
            }
        }
    }
}
