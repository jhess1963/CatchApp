using System.Data.Common;
using Abp.Zero.EntityFramework;
using CatchApp.Authorization.Roles;
using CatchApp.MultiTenancy;
using CatchApp.Users;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CatchApp.EntityFramework
{
    public class CatchAppDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        public virtual IDbSet<Club> Clubs { get; set; }
        public virtual IDbSet<ClubImage> ClubImages { get; set; }
        public virtual IDbSet<ClubVisit> Visits { get; set; }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<Member> Members { get; set; }
        public virtual IDbSet<MemberImage> MemberImages { get; set; }
        public virtual IDbSet<DailyOpenTime> DailyOpenTimes { get; set; }

        public CatchAppDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in CatchAppDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of CatchAppDbContext since ABP automatically handles it.
         */
        public CatchAppDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public CatchAppDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Member>()
                .HasMany(x => x.Friends)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("MemberId");
                    x.MapRightKey("FriendId");
                    x.ToTable("Friendship");
                });

            modelBuilder.Entity<MemberImage>()
                .HasRequired(x => x.Member);

            modelBuilder.Entity<ClubImage>()
                .HasRequired(x => x.Club);

            base.OnModelCreating(modelBuilder);
        }

    }
}
