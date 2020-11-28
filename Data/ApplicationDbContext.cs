using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KrkPoll.Data.Models;

namespace KrkPoll.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<DiscussionPost> DiscussionPosts { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<VotingHistory> VotingHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(2000);

            });

            modelBuilder.Entity<DiscussionPost>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.Post).HasMaxLength(4000);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.Content).HasMaxLength(100);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
                entity.Property(e => e.Content).HasMaxLength(2000);
            });

            modelBuilder.Entity<VotingHistory>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
                entity.HasIndex(e => new {e.ApplicationUserId, e.QuestionId, e.AnswerId}).IsUnique();

                entity.HasOne(v => v.Question)
                    .WithMany(q => q.Voting)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(v => v.Answer)
                    .WithMany(a => a.Voting)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(v => v.ApplicationUser)
                    .WithMany(u => u.Voting)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
