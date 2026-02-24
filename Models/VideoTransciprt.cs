using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lecture2Guide.Models
{
    public class VideoTranscript
    {
        [Key]
        public int Id { get; set; } // Primary key for EF

        public string VideoId { get; set; } = null!;
        public string Language { get; set; } = null!;

        public List<TranscriptItem> Transcript { get; set; } = new();

        // Owned entity type
        public class TranscriptItem
        {
            public int Id { get; set; } // EF requires a key for owned collections
            public string Text { get; set; } = null!;
            public double Start { get; set; }
            public double Duration { get; set; }
        }
    }
}
