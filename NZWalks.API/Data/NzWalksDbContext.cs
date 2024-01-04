using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Data
{
    public class NzWalksDbContext : DbContext
    {
        //constructor
        public NzWalksDbContext( DbContextOptions dbContextOptions ) : base( dbContextOptions )
        {

        }

        /*dbSets: propriety of  DB context class
         * that represents a collection of entities
         * in db
        */

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) 
        {
            base.OnModelCreating( modelBuilder );

            //Seed the data for Difficulties (easy, medium, hard)
            var difficulties = new List<Difficulty>() {

                new Difficulty{
                    Id = Guid.Parse( "2d200898-74cc-4553-a752-0c9050f512ed" ),
                    Name ="Easy"
                },

                new Difficulty{
                    Id = Guid.Parse( "2460b5d3-3885-410b-a187-ad6854774b84" ),
                    Name ="Medium"
                },

                new Difficulty{
                    Id = Guid.Parse( "a8c0e6ad-3a95-4f92-b210-00bb1248fc68" ),
                    Name ="Hard"
                }
            };

            //seeding difficulties to the db using Entity framework
            modelBuilder.Entity<Difficulty>().HasData( difficulties );

            var regions = new List<Region>()
            {
                  new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                }
            };

            //seeding regions to the db using Entity framework
            modelBuilder.Entity<Region>().HasData( regions );


        }

        public static implicit operator NzWalksDbContext( SQLWalkRepository v )
        {
            throw new NotImplementedException();
        }
    }
}