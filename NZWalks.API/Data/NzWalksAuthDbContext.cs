using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using System.Data.Entity;

namespace NZWalks.API.Data
{
    public class NzWalksAuthDbContext: IdentityDbContext
    {
        //constructor
        public NzWalksAuthDbContext( DbContextOptions <NzWalksAuthDbContext> dbContextOptions ) : base( dbContextOptions )
        {

        }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            base.OnModelCreating( builder );
            var readerRoleId = "fb92122b-0cc7-4b7f-ad95-4d2c2541c64d";
            var writerRoleId = "359f9b93-2584-47a1-83ef-0997b7bf6cdf";

            var roles = new List<IdentityRole>
            {

                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()

                },

                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()

                }
            };

            //seeding roles in db
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
