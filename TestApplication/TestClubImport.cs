using CatchApp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    public class TestClubImport
    {
        private readonly string _sourcePath = @"C:\Users\jhess\Documents\Visual Studio 2015\Projects\CatchApp\CatchApp.EntityFramework\Data";

        public void CreateClubs()
        {
            var inhaltGraz = File.ReadAllLines(Path.Combine(_sourcePath, "ClubsGraz.csv"), Encoding.GetEncoding(1250));
            var inhaltKlagenfurt = File.ReadAllLines(Path.Combine(_sourcePath, "ClubsKlagenfurt.csv"), Encoding.UTF8);

            List<Category> categories = new List<Category>();
            CreateCategories(inhaltGraz, categories);
            CreateCategories(inhaltKlagenfurt, categories);


            List<Club> clubs = new List<Club>();
            CreateClubs(inhaltGraz, clubs, categories);
            CreateClubs(inhaltKlagenfurt, clubs, categories);
        }

        private void CreateClubs(string[] inhalt, List<Club> clubs, List<Category> categories)
        {
            for (int i = 0; i < inhalt.Length; i++)
            {
                var items = inhalt[i].Split(';');
                Club club = new Club
                {
                    Name = items[0].Trim(),
                    Latitude = Convert.ToDouble(items[2].Replace('.', ',')),
                    Longitude = Convert.ToDouble(items[3].Replace('.', ',')),
                };
                clubs.Add(club);
                club.Location = DbGeography.FromText(string.Format("POINT({0} {1})", items[2], items[3]));
                var cat = items[1].Split('/');
                for (int y = 0; y < cat.Length; y++)
                {
                    var category = categories.FirstOrDefault(c => c.Name.ToUpper().Equals(cat[y].Trim().ToUpper()));
                    club.Categories.Add(category);
                }

                var openTimes = new List<DailyOpenTime>();

                WeekDay woTag = WeekDay.Monday;
                for (int z = 4; z < 18; z += 2)
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
            }
        }

        public void CreateCategories(string[] inhalt, List<Category> categories)
        {
            for (int i = 0; i < inhalt.Length; i++)
            {
                var items = inhalt[i].Split(';');
                var cat = items[2].Split('/');
                for (int y = 0; y < cat.Length; y++)
                {
                    var category = categories.FirstOrDefault(c => c.Name.Equals(cat[y].Trim()));
                    if (category == null)
                        categories.Add(new Category { Name = cat[y].Trim() });
                }
            }
        }

        private DateTime? ConvertToTime(string element)
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


    }
}
