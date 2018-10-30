using CatchApp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Tests
{
    public class DefaultTestVisitBuilder
    {
        private readonly CatchAppDbContext _context;

        public DefaultTestVisitBuilder(CatchAppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            var allMembers = _context.Members.ToList();

            var manuel = allMembers.First(x => x.UserName == "Clubber");
            var hans = allMembers.First(x => x.UserName == "Hans");
            var alex = allMembers.First(x => x.UserName == "Alex");

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 0, 0),
                MemberId = manuel.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 30, 0),
                MemberId = alex.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });

            var member = allMembers.Find(x => x.UserName == "Men1");

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 30, 0),
                MemberId = member.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });

            member = allMembers.Find(x => x.UserName == "Men6");

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 30, 0),
                MemberId = member.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });
            member = allMembers.Find(x => x.UserName == "Men7");

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 30, 0),
                MemberId = member.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });

            member = allMembers.Find(x => x.UserName == "Women11");

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 30, 0),
                MemberId = member.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });
            member = allMembers.Find(x => x.UserName == "Women12");

            _context.Visits.Add(new ClubVisit
            {
                ClubId = 1,
                EntryDate = new DateTime(2016, 6, 8, 22, 30, 0),
                MemberId = member.Id,
                LeavingDate = new DateTime(2016, 6, 9, 4, 0, 0),
                HasLeft = false
            });

            _context.SaveChanges();
        }
    }
}
