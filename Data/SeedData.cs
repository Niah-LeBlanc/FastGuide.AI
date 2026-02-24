using Lecture2Guide.Data;
using Lecture2Guide.Models;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize( ApplicationDbContext context )
    {
        context.Database.Migrate();

        // Seed users
        if (!context.Users.Any())
        {
            var user1 = new ApplicationUser
            {
                UserName = "testuser1",
                NormalizedUserName = "TESTUSER1",
                Email = "test1@example.com",
                NormalizedEmail = "TEST1@EXAMPLE.COM",
                EmailConfirmed = true,
                IsAdmin = false,
                LastLogin = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var admin = new ApplicationUser
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                IsAdmin = true,
                LastLogin = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            context.Users.AddRange(user1, admin);
            context.SaveChanges(); // Flush so IDs are generated
        }

        // Seed study guides
        if (!context.StudyGuides.Any())
        {
            // Retrieve the actual users with their generated IDs
            var user1Id = context.Users.First(u => u.UserName == "testuser1").Id;
            var adminId = context.Users.First(u => u.UserName == "admin").Id;

            var guides = new List<StudyGuide>
            {
                new StudyGuide
                {
                    Title = "C# Basics",
                    GeneratedBy = user1Id,
                    Format = GuideFormat.PDF | GuideFormat.Markdown,
                    MarkdownContent = "# C# Basics\nThis is sample markdown content."
                },
                new StudyGuide
                {
                    Title = "ASP.NET Core Guide",
                    GeneratedBy = adminId,
                    Format = GuideFormat.HTML,
                    HtmlContent = "<h1>ASP.NET Core</h1><p>Sample HTML content</p>"
                }
            };

            context.StudyGuides.AddRange(guides);
            context.SaveChanges();
        }
    }
}
