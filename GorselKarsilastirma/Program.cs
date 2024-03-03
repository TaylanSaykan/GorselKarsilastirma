using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using ImageMagick;

namespace GorselKarsilastirma
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless"); // Arayüzü göstermeden çalışır.
            Console.WriteLine("Lütfen bekleyin. bu işlem biraz uzun sürebilir");
            using (IWebDriver driver = new ChromeDriver(service, options))
            {
                for (int i = 1; i <= 25; i++) // Örnek olarak sadece 2 görsel alıyoruz
                {
                    string url = $"https://www.taylansaykan.com/{i}";
                    string screenshotFileName = $"ekran_goruntusu_{i}.png";

                    driver.Navigate().GoToUrl(url);
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile(screenshotFileName, ScreenshotImageFormat.Png);
                }
            }

            // Görselleri Karşılaştır
            if (FarkVarMi("ekran_goruntusu_1.png", "ekran_goruntusu_2.png"))//burada ekrangörüntüsü 1 ve 2 arasında anlayamadığım bir fark çıkıyor ekran görüntüsü 1 ve ekran görüntüsü 2 nin kıyaslama kıstaslarını tek görsel olarak yaparsak farkı bulamıyor yani aynı görseller arasında kıyaslamada mutlak sonuca götürüyor
            {
                Console.WriteLine("Görseller arasında fark var.");
            }
            else
            {
                Console.WriteLine("Görseller arasında fark yok.");
            }

            Console.WriteLine("Programı kapatmak için herhangi bir tuşa basın...");
            Console.ReadKey(); // Kullanıcı bir tuşa basana kadar bekler.
        }

        static bool FarkVarMi(string imagePath1, string imagePath2, double threshold = 0.1)
        {
            using (MagickImage image1 = new MagickImage(imagePath1))
            using (MagickImage image2 = new MagickImage(imagePath2))
            {
                using (MagickImage diffImage = new MagickImage())
                {
                    double diff = image1.Compare(image2, ErrorMetric.Absolute, diffImage);

                    return diff > threshold;
                }
            }
        }
    }
}
