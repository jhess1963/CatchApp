using Abp;
using Abp.Domain.Repositories;
using CatchApp;
using CatchApp.Clubs;
using CatchApp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //var test = new TestClubImport();
            //test.CreateClubs();
            //ShrinkImage();
            TestWebApi();
            //SearchClubs();
        }

        private static void TestWebApi()
        {
            var testClient = new TestWebApiClient();

            //testClient.SearchClubsExt();
            //testClient.InsertClubImage();
            testClient.InsertMemberImage();
            Console.WriteLine("Fertig!");
            Console.ReadLine();
        }

        private static void SearchClubs()
        {
            AbpBootstrapper bootstrapper = new AbpBootstrapper();
            bootstrapper.Initialize();

            IClubAppService clubService = bootstrapper.IocManager.Resolve<IClubAppService>();
            //var result = clubService.SearchClubs(new CatchApp.Geodata.GeoPoint(46.599550, 14.270328));

        }

        private static void ShrinkImage()
        {
            Image bigImage = Image.FromFile(@"D:\Photos\Hansi und Astrid\2016\04 April\IMG_1451.JPG");
            Byte[] img = ImageHelper.ImageToByteArray(bigImage, ImageFormat.Jpeg);
            Byte[] smallImg = ImageHelper.ResizeImage(img, 150, 150);
            Image smallImage = ImageHelper.ByteArrayToImage(smallImg);
            smallImage.Save(@"c:\temp\kleinesBild1.jpeg", ImageFormat.Png);
        }

        #region Test Clubs Ascii
        private void TestClubs()
        {
            string _sourcePath = @"C:\Users\jhess\Documents\Visual Studio 2015\Projects\CatchApp\CatchApp.EntityFramework\Data";
            var inhalt = File.ReadAllLines(Path.Combine(_sourcePath, "Clubs.csv"), Encoding.GetEncoding(1250));

            CatchAppDbContext _context = new CatchAppDbContext();

            var dbCategories = _context.Categories.ToList();

            for (int i = 0; i < inhalt.Length; i++)
            {
                var items = inhalt[i].Split(';');
                Club club = new Club
                {
                    Name = items[1].Trim(),
                    Latitude = Convert.ToDouble(items[3].Replace('.', ',')),
                    Longitude = Convert.ToDouble(items[4].Replace('.', ','))
                };
                var cat = items[2].Split('/');
                for (int y = 0; y < cat.Length; y++)
                {
                    var category = dbCategories.FirstOrDefault(c => c.Name.Equals(cat[y].Trim()));
                    club.Categories.Add(category);
                }

                var openTimes = new List<DailyOpenTime>();

                WeekDay woTag = WeekDay.Monday;
                for (int z = 5; z < 19; z += 2)
                {
                    var beginn = ConvertToTime(items[z]);
                    var ende = ConvertToTime(items[z + 1]);
                    if (beginn != null && ende != null)
                    {
                        club.OpenTimes.Add(new DailyOpenTime
                        {
                            OpenTime = beginn.Value,
                            CloseTime = ende.Value,
                            DayOfWeek = woTag
                        });
                    }
                    woTag++;
                }

                _context.Clubs.Add(club);
            }
            //_context.SaveChanges();
        }

        private static DateTime? ConvertToTime(string element)
        {
            var zeit = element.Split(':');
            if (zeit.Length == 2)
            {
                int std = Convert.ToInt32(zeit[0]);
                int min = Convert.ToInt32(zeit[1]);
                return new DateTime(2016, 1, 1, (std == 24) ? 0 : std, min, 0);
            }
            else
                return null;
        }

        #endregion
    }
}



