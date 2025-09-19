using Adminstrator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Administrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParticipantsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Participants/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Participant participant)
        {
            if (participant == null)
                return BadRequest("Participant data is required.");

            // Ensure that User information is included
            if (participant.User == null)
                return BadRequest("User information is required.");

            // Validate username uniqueness
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == participant.User.Username);
            if (existingUser != null)
                return BadRequest("Username already exists.");

            // Add the user to the Users table
            _context.Users.Add(participant.User);
            await _context.SaveChangesAsync();

            // Assign the UserId to the Participant (set the foreign key)
            participant.UserId = participant.User.UserId;

            // Add the Participant to the Participants table
            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            // Return success response
            return Ok(new { message = "Participant registered successfully", participantId = participant.ParticipantId });
        }

        // POST: api/Participants/{participantId}/Enroll/{eventId}
        [HttpPost("{participantId}/Enroll/{eventId}")]
        public async Task<IActionResult> EnrollInEvent(int participantId, int eventId)
        {
            // Find the participant and event in the database
            var participant = await _context.Participants.Include(p => p.Events).FirstOrDefaultAsync(p => p.ParticipantId == participantId);
            var eventEntity = await _context.Events.FindAsync(eventId);

            if (participant == null)
            {
                return NotFound(new { message = "Participant not found." });
            }

            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Check if the participant is already enrolled in the event
            if (!participant.Events.Contains(eventEntity))
            {
                participant.Events.Add(eventEntity);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Enrollment successful" });
        }

        // PUT: api/Participants/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] Participant updatedData)
        {
            if (updatedData == null)
                return BadRequest("Updated participant data is required.");

            var participant = await _context.Participants
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ParticipantId == id);

            if (participant == null)
                return NotFound(new { message = "Participant not found." });

            bool participantUpdated = false;
            bool userUpdated = false;

            // Update participant details
            if (!string.IsNullOrWhiteSpace(updatedData.Name) && updatedData.Name != participant.Name)
            {
                participant.Name = updatedData.Name;
                participantUpdated = true;

                // Update username if it's different from previous
                if (participant.User != null && updatedData.Name != participant.User.Username)
                {
                    participant.User.Username = updatedData.Name; // Optional: only update if needed
                    userUpdated = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(updatedData.Email) && updatedData.Email != participant.Email)
            {
                participant.Email = updatedData.Email;
                participantUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updatedData.Phone) && updatedData.Phone != participant.Phone)
            {
                participant.Phone = updatedData.Phone;
                participantUpdated = true;
            }

            if (participantUpdated || userUpdated)
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Participant and/or User updated successfully" });
            }

            return Ok(new { message = "No changes detected" });
        }

        // POST: api/Participants/Feedback
        [HttpPost("Feedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] Feedback feedback)
        {
            // Validate the feedback data
            if (feedback == null || feedback.EventID <= 0 || feedback.SpeakerID <= 0 || string.IsNullOrWhiteSpace(feedback.feedback_remarks))
            {
                return BadRequest("Invalid feedback data.");
            }

            // Add the feedback to the database
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback submitted successfully" });
        }

        // Optional: Get all feedback (if needed)
        [HttpGet("Feedback")]
        public async Task<IActionResult> GetFeedbacks()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();
            return Ok(feedbacks);
        }
    }
}
