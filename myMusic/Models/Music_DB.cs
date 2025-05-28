using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace myMusic.Models
{
    public partial class Music_DB : DbContext
    {
        public Music_DB()
            : base("name=Music_DB")
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<CollectionLibrary> CollectionLibrary { get; set; }
        public virtual DbSet<Collections> Collections { get; set; }
        public virtual DbSet<CollectionSongs> CollectionSongs { get; set; }
        public virtual DbSet<CollectionTypes> CollectionTypes { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<ListeningHistory> ListeningHistory { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }
        public virtual DbSet<Songs> Songs { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Collections)
                .WithRequired(e => e.Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.CollectionLibrary)
                .WithRequired(e => e.Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Likes)
                .WithRequired(e => e.Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.ListeningHistory)
                .WithRequired(e => e.Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Reports)
                .WithRequired(e => e.Accounts)
                .HasForeignKey(e => e.ReporterID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Reports1)
                .WithOptional(e => e.Accounts1)
                .HasForeignKey(e => e.ReportedAccountID);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Accounts1)
                .WithMany(e => e.Accounts2)
                .Map(m => m.ToTable("Follows").MapLeftKey("FollowerID").MapRightKey("FollowingID"));

            modelBuilder.Entity<Collections>()
                .HasMany(e => e.CollectionLibrary)
                .WithRequired(e => e.Collections)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Collections>()
                .HasMany(e => e.CollectionSongs)
                .WithRequired(e => e.Collections)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Collections>()
                .HasMany(e => e.Reports)
                .WithOptional(e => e.Collections)
                .HasForeignKey(e => e.ReportedCollectionID);

            modelBuilder.Entity<CollectionTypes>()
                .HasMany(e => e.Collections)
                .WithRequired(e => e.CollectionTypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Songs>()
                .HasMany(e => e.CollectionSongs)
                .WithRequired(e => e.Songs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Songs>()
                .HasMany(e => e.Likes)
                .WithRequired(e => e.Songs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Songs>()
                .HasMany(e => e.ListeningHistory)
                .WithRequired(e => e.Songs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Songs>()
                .HasMany(e => e.Reports)
                .WithOptional(e => e.Songs)
                .HasForeignKey(e => e.ReportedSongID);
        }
    }
}
