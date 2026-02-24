using Lecture2Guide.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lecture2Guide.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
            : base(options) { }

        public DbSet<StudyGuide> StudyGuides { get; set; }
        public DbSet<VideoTranscript> VideoTranscripts { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating(modelBuilder);

            // StudyGuide -> ApplicationUser
            modelBuilder.Entity<StudyGuide>()
                .HasOne(sg => sg.GeneratedByUser)
                .WithMany(u => u.GeneratedStudyGuides)
                .HasForeignKey(sg => sg.GeneratedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // VideoTranscript -> TranscriptItems
            modelBuilder.Entity<VideoTranscript>()
                .OwnsMany(vt => vt.Transcript, ti =>
                {
                    ti.WithOwner().HasForeignKey("VideoTranscriptId"); // FK in table
                    ti.HasKey("Id"); // Shadow PK
                    ti.Property<int>("Id").ValueGeneratedOnAdd(); // Auto-increment
                    ti.Property(t => t.Text).IsRequired();
                    ti.Property(t => t.Start);
                    ti.Property(t => t.Duration);
                });
        }
    }
}
