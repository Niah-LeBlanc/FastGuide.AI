using Lecture2Guide.Data;
using Lecture2Guide.Models;
using Lecture2Guide.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lecture2Guide.Controllers
{
    public class GuideController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TranscriptService _transcriptService;

        public GuideController( ApplicationDbContext context, TranscriptService transcriptService )
        {
            _context = context;
            _transcriptService = transcriptService;
        }

        [HttpGet]
        public IActionResult GuideIndex()
        {
            return View("GuideIndex"); // points to Views/Guide/GuideIndex.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> GenerateGuide( [FromBody] GuideRequest request )
        {
            if (string.IsNullOrEmpty(request.YoutubeUrl))
                return BadRequest("URL is required.");

            // Example: extract video ID from URL (simple placeholder)
            string videoId = ExtractVideoId(request.YoutubeUrl);

            // Prepare a list to hold transcript items (will never be null)
            List<VideoTranscript.TranscriptItem> transcriptItems = new List<VideoTranscript.TranscriptItem>();

            // 1️⃣ Fetch transcript items (TranscriptService returns VideoTranscript?)
            try
            {
                var videoTranscript = await _transcriptService.GetTranscriptAsync(request.YoutubeUrl);

                // videoTranscript may be null; videoTranscript.Transcript may also be null.
                // Use null-coalescing to ensure transcriptItems is not null to satisfy nullability.
                transcriptItems = videoTranscript?.Transcript ?? new List<VideoTranscript.TranscriptItem>();

                // Print to console for debugging
                Console.WriteLine("------ TRANSCRIPT START ------");
                foreach (var item in transcriptItems)
                    Console.WriteLine(item.Text);
                Console.WriteLine("------- TRANSCRIPT END -------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching transcript: {ex.Message}");
                transcriptItems = new List<VideoTranscript.TranscriptItem>();
            }

            // 2️⃣ Store transcript in DB
            var videoTranscriptEntity = new VideoTranscript
            {
                VideoId = videoId,
                Language = "en",
                Transcript = transcriptItems
            };

            _context.VideoTranscripts.Add(videoTranscriptEntity);
            await _context.SaveChangesAsync();

            // 3️⃣ Generate guide HTML (placeholder for now)
            var guideHtml = await GenerateGuideFromTranscriptAsync(transcriptItems);

            return Json(new
            {
                html = guideHtml,
                videoTitle = "Sample Video",
                generationTime = "Just now"
            });
        }

        private Task<string> GenerateGuideFromTranscriptAsync( List<VideoTranscript.TranscriptItem> transcript )
        {
            // Placeholder: combine transcript lines into simple HTML
            string html = "<p>";
            foreach (var item in transcript)
                html += item.Text + "<br/>";
            html += "</p>";
            return Task.FromResult(html);
        }

        private string ExtractVideoId( string youtubeUrl )
        {
            // Simplified extraction (assumes ?v=ID)
            var uri = new Uri(youtubeUrl);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query.Get("v") ?? Guid.NewGuid().ToString(); // fallback to GUID
        }

        public class GuideRequest
        {
            public string YoutubeUrl { get; set; }
        }
    }
}
