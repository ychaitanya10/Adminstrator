using Adminstrator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Adminstrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeakerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpeakerController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Speaker/register
        // This endpoint creates both User and Speaker, fixing the foreign key issue
        [HttpPost("register")]
        public IActionResult RegisterSpeaker([FromBody] SpeakerRegistrationDto registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create User first
            var user = new User
            {
                Username = registration.Username,
                Password = registration.Password,
                Role = "Speaker"
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Create Speaker, referencing UserId
            var speaker = new Speaker
            {
                Name = registration.Name,
                Email = registration.Email,
                Phone = registration.Phone,
                Address = registration.Address,
                KeySkills = string.Join(",", registration.KeySkills),

                UserId = user.UserId
            };
            _context.Speakers.Add(speaker);
            _context.SaveChanges();

            return Ok(speaker);
        }

        // GET: api/Speaker/{speakerId}/events
        [HttpGet("{speakerId}/events")]
        public IActionResult GetAvailableEvents(int speakerId)
        {
            // Example logic: get all events not associated with this speaker
            var events = _context.Events
                .Where(e => e.SpeakerId != speakerId)
                .ToList();

            return Ok(events);
        }

        // POST: api/Speaker/{speakerId}/enroll/{eventId}
        [HttpPost("{speakerId}/enroll/{eventId}")]
        public IActionResult EnrollInEvent(int speakerId, int eventId)
        {
            var @event = _context.Events.Find(eventId);
            if (@event == null)
                return NotFound("Event not found.");

            @event.SpeakerId = speakerId;
            _context.SaveChanges();
            return Ok("Enrolled successfully.");
        }

        // GET: api/Speaker/{speakerId}/feedback
        [HttpGet("{speakerId}/feedback")]
        public IActionResult GetSpeakerFeedback(int speakerId)
        {
            var feedbacks = _context.Feedbacks
                .Where(f => f.SpeakerID == speakerId)
                .ToList();
            return Ok(feedbacks);
        }
    }

    // DTO for registration
    public class SpeakerRegistrationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string[] KeySkills { get; set; }
    }
}