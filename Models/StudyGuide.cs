using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Lecture2Guide.Models
{
    [Flags]
    public enum GuideFormat
    {
        None = 0,
        PDF = 1,
        HTML = 2,
        Markdown = 4
    }

    public class StudyGuide
    {
        [Key]
        public int GuideID { get; set; }

        [ForeignKey("GeneratedByUser")]
        public string GeneratedBy { get; set; } = null!;  // non-null after creation

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        // Store which formats are available
        public GuideFormat Format { get; set; } = GuideFormat.None;

        // Store actual content for each format
        public byte[]? PdfContent { get; set; }      // PDF bytes
        public string? HtmlContent { get; set; }     // HTML text
        public string? MarkdownContent { get; set; } // Markdown text

        // Navigation properties
        public virtual ApplicationUser GeneratedByUser { get; set; } = null!;

        // Helper: Get a list of formats available
        [NotMapped]
        public List<string> AvailableFormats =>
            Enum.GetValues(typeof(GuideFormat))
                .Cast<GuideFormat>()
                .Where(f => f != GuideFormat.None && Format.HasFlag(f))
                .Select(f => f.ToString())
                .ToList();
    }
}
