using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Lecture2Guide.Models
{
    // MUST inherit from IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // Custom properties
        public bool IsAdmin { get; set; }

        // Store profile picture directly as bytes
        public byte[]? ProfilePicture { get; set; }

        public DateTime LastLogin { get; set; }
        public string? SessionToken { get; set; }

        // Navigation properties
        public virtual ICollection<StudyGuide> GeneratedStudyGuides { get; set; }

        public ApplicationUser()
        {
            GeneratedStudyGuides = new HashSet<StudyGuide>();
            LastLogin = DateTime.UtcNow;
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.UtcNow;
        }
    }
}
