using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

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

    }
}