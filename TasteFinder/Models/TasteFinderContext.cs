using Microsoft.EntityFrameworkCore; 

namespace TasteFinder.Models
{
    public class TasteFinderContext: DbContext
    {
        public TasteFinderContext(DbContextOptions<TasteFinderContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
                
            }

            
            modelBuilder.Entity<Contribution>()
            .HasOne(p => p.Author)
            .WithMany(p => p.Contributions)
            .HasForeignKey("AuthorEmail").OnDelete(DeleteBehavior.ClientSetNull);
            

        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Keyword> Possessions { get; set; }
        public DbSet<Contribution> Contributions { get; set; }

    }
}

