namespace DAL
{
    using DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AuctionDB : DbContext
    {
        public AuctionDB()
            : base("name=AuctionDB")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            /*modelBuilder.Entity<DB_Subcategory>()
            .HasMany(s => s.Lots)
            .WithRequired(l => l.Subcategory)
            .WillCascadeOnDelete(false);
            Configuration.LazyLoadingEnabled = false;*/
        }

        public DbSet<DB_Lot> Comments { get; set; }
        public DbSet<DB_User> Users { get; set; }
        public DbSet<DB_Category> Categories { get; set; }
        public DbSet<DB_Subcategory> Posts { get; set; }
    }
    
}