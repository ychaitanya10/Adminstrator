using Adminstrator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Adminstrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParticipantController(AppDbContext context)
        {
            _context = context;
        }

        //// 1. Register a new participant
        //[HttpPost("register")]
        //public async Task<IActionResult> Register(Participant participant)
        //{
        //    _context.Participants.Add(participant);
        //    await _context.SaveChangesAsync();
        //    return Ok(participant);
        //}

        // 2. Modify participant details
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateParticipant(int id, [FromBody] Participant updated)
        {
            var participant = await _context.Participants
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ParticipantId == id);

            if (participant == null) return NotFound("Participant not found.");

            // Update only if values are provided
            if (!string.IsNullOrWhiteSpace(updated.Name)&&updated.Name!="string")
                participant.Name = updated.Name;

            if (!string.IsNullOrWhiteSpace(updated.Email)&&updated.Email != "string")
                participant.Email = updated.Email;

            if (!string.IsNullOrWhiteSpace(updated.Phone)&&updated.Phone != "string")
                participant.Phone = updated.Phone;

            // Optional: update User fields if provided
            if (updated.User != null)
            {
                if (!string.IsNullOrWhiteSpace(updated.User.Username))
                    participant.User.Username = updated.User.Username;

                if (!string.IsNullOrWhiteSpace(updated.User.Password))
                    participant.User.Password = updated.User.Password; // Consider hashing
            }

            await _context.SaveChangesAsync();
            return Ok(participant);
        }

        // 3. Enroll in an event
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll(int participantId, int eventId)
        {
            var participant = await _context.Participants
                .Include(p => p.Events)
                .FirstOrDefaultAsync(p => p.ParticipantId == participantId);

            var evnt = await _context.Events.FindAsync(eventId);

            if (participant == null || evnt == null)
                return NotFound("Participant or Event not found.");

            participant.Events.Add(evnt);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Enrollment successful." });
        }

        // 4. Submit feedback
        [HttpPost("submit-feedback")]
        public async Task<IActionResult> SubmitFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return Ok(feedback);
        }

        // 5. Apply a promotion code (simplified logic)
        //[HttpPost("apply-promocode")]
        //public IActionResult ApplyPromoCode(string promoCode)
        //{
        //    // Simulate promo code validation
        //    var validCodes = await _context.PromotionCodes.FindAsync(promoCode);

        //    if (!validCodes.Contains(promoCode.ToUpper()))
        //        return BadRequest("Invalid promo code.");

        //    return Ok(new { Message = "Promo code applied successfully!", Discount = "10-50%" });
        //}

        [HttpGet]
        public async Task<IActionResult> getParticipant()
        {
            return Ok(_context.Participants.ToListAsync());
        }
    }
}