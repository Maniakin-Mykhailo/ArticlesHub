using ArticlesHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArticlesHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Article>(entity =>
        //    {
        //        entity.HasKey(a => a.Id);
        //        entity.Property(a => a.Title).IsRequired();
        //        entity.Property(a => a.Text).IsRequired();
        //        entity.Property(a => a.Author).IsRequired();

        //        entity.HasMany(a => a.Images)
        //            .WithOne(i => i.Article)
        //            .HasForeignKey(i => i.ArticleId)
        //            .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    modelBuilder.Entity<ArticlesHub.Models.Image>(entity =>
        //    {
        //        entity.HasKey(i => i.Id);
        //        entity.Property(i => i.FileName).IsRequired();
        //    });

        //    base.OnModelCreating(modelBuilder);
        //}


        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticlesHub.Models.Image> Images { get; set; }
    }
}
