using CatchApp.EntityFramework;
using CatchApp.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Migrations.SeedData
{
    public class DefaultMemberBuilder
    {
        private readonly CatchAppDbContext _context;

        public DefaultMemberBuilder(CatchAppDbContext context)
        {
            _context = context;
        }

        public void BuildTestMembers()
        {
            var maleMembers = new List<Member>();

            for (int i = 1; i < 11; i++)
            {
                maleMembers.Add(new Member
                {
                    UserName = string.Format("Men{0}", i),
                    IsMale = true,
                    IsSingle = (i < 5) ? true : false
                });
            }

            var femaleMembers = new List<Member>();

            for (int i = 11; i < 20; i++)
            {
                femaleMembers.Add(new Member
                {
                    UserName = string.Format("Women{0}", i),
                    IsMale = false,
                    IsSingle = (i < 12) ? true : false,
                });
            }

            maleMembers[0].Friends.Add(maleMembers[5]);
            maleMembers[0].Friends.Add(maleMembers[6]);
            maleMembers[0].Friends.Add(maleMembers[7]);
            maleMembers[0].Friends.Add(maleMembers[8]);
            maleMembers[0].Friends.Add(maleMembers[9]);
            maleMembers[0].Friends.Add(femaleMembers[0]);
            maleMembers[0].Friends.Add(femaleMembers[1]);
            maleMembers[0].Friends.Add(femaleMembers[2]);
            maleMembers[0].Friends.Add(femaleMembers[3]);

            femaleMembers[0].Friends.Add(maleMembers[0]);
            femaleMembers[0].Friends.Add(maleMembers[1]);
            femaleMembers[0].Friends.Add(femaleMembers[2]);
            femaleMembers[0].Friends.Add(femaleMembers[3]);

            foreach (var member in maleMembers)
            {
                _context.Members.Add(member);
            };

            foreach (var member in femaleMembers)
            {
                _context.Members.Add(member);
            };

            _context.SaveChanges();
        }

        public void Build()
        {
            if (_context.Users.Count(x => x.UserName == "Clubber") == 0)
            {
                var userManuel = _context.Users.Add(new User
                {
                    TenantId = 1,
                    UserName = "Clubber",
                    Name = "Hess",
                    Surname = "Manuel",
                    Password = new PasswordHasher().HashPassword("manuel"),
                    EmailAddress = "hess1201@gmail.com",
                    IsActive = true
                });

                var userAlex = _context.Users.Add(new User
                {
                    TenantId = 1,
                    UserName = "Alex",
                    Name = "Maestrini",
                    Surname = "Alex",
                    Password = new PasswordHasher().HashPassword("alex"),
                    EmailAddress = "aaa@pamis.at",
                    IsActive = true
                });

                var userHans = _context.Users.Add(new User
                {
                    TenantId = 1,
                    UserName = "Hans",
                    Name = "Hess",
                    Surname = "Johann",
                    Password = new PasswordHasher().HashPassword("hans"),
                    EmailAddress = "jhess@pamis.at",
                    IsActive = true
                });

                _context.SaveChanges();

                var manuel = _context.Members.Add(new Member
                {
                    UserName = "Clubber",
                    Name = "Hess",
                    Surname = "Manuel",
                    IsMale = true,
                    IsSingle = true,
                });

                var alex = _context.Members.Add(new Member
                {
                    UserName = "Alex",
                    Name = "Maestrini",
                    Surname = "Alexander",
                    IsMale = true,
                    IsSingle = false,
                });

                var hans = _context.Members.Add(new Member
                {
                    UserName = "Hans",
                    Name = "Hess",
                    Surname = "Johann",
                    IsMale = true,
                    IsSingle = false,
                });

                manuel.Friends.Add(alex);
                manuel.Friends.Add(hans);

                alex.Friends.Add(manuel);
                hans.Friends.Add(manuel);

                _context.SaveChanges();
            }
        }
    }
}
