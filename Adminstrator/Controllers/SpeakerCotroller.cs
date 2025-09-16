using Adminstrator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Adminstrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeakerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _materialsFolder;

        public SpeakerController(AppDbContext context)
        {
            _context = context;
            _materialsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedMaterials");
            if (!Directory.Exists(_materialsFolder))
                Directory.CreateDirectory(_materialsFolder);
        }

        // POST: api/speaker/upload-materials
        [HttpPost("upload-materials")]
        public async Task<IActionResult> UploadMaterials([FromForm] UploadMaterialFormDto dto)
        {
            if (dto.MaterialFile == null || dto.MaterialFile.Length == 0)
                return BadRequest("No file uploaded.");

            var evnt = await _context.Events.FindAsync(dto.EventId);
            if (evnt == null)
                return NotFound("Event not found.");

            var fileName = $"{dto.EventId}_{Guid.NewGuid()}_{dto.MaterialFile.FileName}";
            var filePath = Path.Combine(_materialsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.MaterialFile.CopyToAsync(stream);
            }

            // TODO: Save metadata to a Material table (if you have one)
            var materialUrl = Url.Action(nameof(DownloadMaterial), new { fileName });

            return Ok(new
            {
                Message = "Materials uploaded successfully!",
                MaterialUrl = materialUrl
            });
        }

        // GET: api/speaker/download-material/{fileName}
        [HttpGet("download-material/{fileName}")]
        public async Task<IActionResult> DownloadMaterial(string fileName)
        {
            var filePath = Path.Combine(_materialsFolder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Requested material not found.");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = GetContentType(filePath);
            return File(memory, contentType, fileName);
        }

        // GET: api/speaker/roster/{eventId}
        [HttpGet("roster/{eventId}")]
        public async Task<IActionResult> GetRoster(int eventId)
        {
            //var evnt = await _context.Events
            //    .Include(e => e.Participants)
            //    .FirstOrDefaultAsync(e => e.EventId == eventId);
            var evnt = await _context.Events.FindAsync(eventId);

            if (evnt == null)
                return NotFound("Event not found.");

            var roster = evnt.Participants
                .Select(p => new { p.ParticipantId, p.Name, p.Email })
                .ToList();

            return Ok(roster);
        }

        // GET: api/speaker/feedback/{eventId}
        [HttpGet("feedback/{eventId}")]
        public async Task<IActionResult> ViewFeedback(int eventId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.EventID == eventId)
                .Select(f => new
                {
                    f.FeedBackID,
                    f.feedback_remarks,
                    f.SpeakerID
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        // POST: api/speaker/express-interest
        [HttpPost("express-interest")]
        public async Task<IActionResult> ExpressInterest([FromBody] CourseInterestDto dto)
        {
            var speaker = await _context.Speakers
                .FirstOrDefaultAsync(s => s.SpeakerId == dto.SpeakerId);

            var topic = await _context.Topics
                .FirstOrDefaultAsync(t => t.TopicId == dto.TopicId);

            if (speaker == null || topic == null)
                return NotFound("Speaker or topic not found.");

            // TODO: Save interest to a SpeakerCourseInterest table (not in your current models)
            // For now, just return OK
            return Ok(new { Message = $"Interest expressed for topic '{topic.TopicName}' successfully!" });
        }

        // GET: api/speaker/search-events?topicId=2
        [HttpGet("search-events")]
        public async Task<IActionResult> SearchEvents([FromQuery] int topicId)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic == null)
                return NotFound("Topic not found.");

            var events = await _context.Events
                .Where(e => e.TopicId == topicId)
                .Select(e => new
                {
                    e.EventId,
                    e.CourseTitle,
                    e.StartDate,
                    e.EndDate,
                    e.ClassSize
                })
                .ToListAsync();

            return Ok(events);
        }

        // POST: api/speaker/enroll
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollForEvent([FromBody] SpeakerEnrollmentDto dto)
        {
            var speaker = await _context.Speakers.FindAsync(dto.SpeakerId);
            var evnt = await _context.Events.FindAsync(dto.EventId);

            if (speaker == null || evnt == null)
                return NotFound("Speaker or Event not found.");

            if (evnt.SpeakerId != 0 && evnt.SpeakerId != null)
                return BadRequest("Event already has a speaker.");

            evnt.SpeakerId = speaker.SpeakerId;
            await _context.SaveChangesAsync();

            // TODO: Notify admin
            return Ok(new { Message = $"Speaker {dto.SpeakerId} enrolled for event {dto.EventId} successfully!" });
        }

        // Helper: Basic content type detection
        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".doc" or ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".ppt" or ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }
    }

    // DTOs

    public class UploadMaterialFormDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public IFormFile MaterialFile { get; set; }
    }

    public class CourseInterestDto
    {
        public int SpeakerId { get; set; }
        public int TopicId { get; set; }
    }

    public class SpeakerEnrollmentDto
    {
        public int SpeakerId { get; set; }
        public int EventId { get; set; }
    }
}