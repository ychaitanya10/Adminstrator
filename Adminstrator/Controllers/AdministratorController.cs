using Adminstrator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Alias to resolve conflict between namespace and model class
using AdminModel = Adminstrator.Models.Administrator;

namespace Adminstrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdministratorController(AppDbContext context)
        {
            _context = context;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Actions On Promotion Codes
        [HttpPost("PromoCode/add")]
        public async Task<ActionResult> AddPromoCode(PromotionCode code_obj)
        {
            if (code_obj != null)
            {
                await _context.PromotionCodes.AddAsync(code_obj);
                await _context.SaveChangesAsync();
                return Ok("New PromoCode Successfully Added");
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("promocode/view")]
        public async Task<ActionResult<PromotionCode>> ViewPromoCodes()
        {
            var p_codes = await _context.PromotionCodes.ToListAsync();
            if (p_codes != null && p_codes.Count > 0)
            {
                return Ok(p_codes);
            }
            else
            {
                return NotFound("No PromoCodes Found");
            }
        }

        [HttpDelete("promocode/delete")]
        public async Task<ActionResult> DeletePromoCode(int id)
        {
            var p_code = await _context.PromotionCodes.FindAsync(id);
            if (p_code != null)
            {
                _context.PromotionCodes.Remove(p_code);
                await _context.SaveChangesAsync();
                return Ok("PromoCode Deleted Successfully");
            }
            else
            {
                return NotFound("No PromoCode Found");
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Actions on Administrators
        [HttpGet("Admin/View")]
        public async Task<ActionResult<IEnumerable<AdminModel>>> GetAllAdmins()
        {
            var admins = await _context.Administrators.ToListAsync();
            if (admins != null && admins.Count > 0)
            {
                return Ok(admins);
            }
            else
            {
                return NotFound("No Admins Found");
            }
        }

        [HttpPost("Admin/Add")]
        public async Task<ActionResult<AdminModel>> AddAdmin(AdminModel admin_obj, string username, string password)
        {
            if (admin_obj != null)
            {
                var user = new User
                {
                    Username = username,
                    Password = password, // Ideally hash this
                    Role = "admin"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                admin_obj.UserId = user.UserId;
                await _context.Administrators.AddAsync(admin_obj);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Administrator added successfully" });
            }

            return NoContent();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Actions on Locations  
        [HttpPost("Location/Add")]
        public async Task<ActionResult> AddLocation(Location loc_obj)
        {
            if (loc_obj != null)
            {
                await _context.Locations.AddAsync(loc_obj);
                await _context.SaveChangesAsync();
                return Ok(new { message = "New Location Successfully Added" });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("Location/View")]
        public async Task<IActionResult> ViewLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            if (locations != null && locations.Count > 0)
            {
                return Ok(locations);
            }
            return NoContent();
        }

        [HttpDelete("Location/Delete")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var loc = await _context.Locations.FindAsync(id);
            if (loc != null)
            {
                _context.Locations.Remove(loc);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Location Deleted Successfully" });
            }
            else
            {
                return NotFound("No Location Found");
            }
        }

        [HttpPut("Location/Modify")]
        public async Task<ActionResult> ModifyLocation(Location loc_obj, int id)
        {
            var loc = await _context.Locations.FindAsync(id);
            if (loc != null && loc_obj != null)
            {
                loc.City = loc_obj.City;
                loc.State = loc_obj.State;
                loc.Description = loc_obj.Description;
                _context.Locations.Update(loc);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Location Modified Successfully" });
            }
            else
            {
                return NotFound("No Location Found");
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
                return NotFound(new { message = "Location not found" });

            return Ok(location);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Actions on Speakers
        [HttpPost("Speakers/Add")]
        public async Task<ActionResult> AddSpeaker(Speaker speaker_obj, string username, string password)
        {
            if (speaker_obj == null || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Missing speaker data or credentials.");

            var user = new User
            {
                Username = username,
                Password = password,
                Role = "Speaker"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            speaker_obj.UserId = user.UserId;

            _context.Speakers.Add(speaker_obj);
            await _context.SaveChangesAsync();

            return Ok(new { message = "New Speaker Successfully Added" });
        }

        [HttpGet("Speakers/View")]
        public async Task<ActionResult> ViewSpeakers()
        {
            var speakers = await _context.Speakers.ToListAsync();
            if (speakers != null && speakers.Count > 0)
            {
                return Ok(speakers);
            }
            return NoContent();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Actions on Topics
        [HttpGet("Topic/view")]
        public async Task<ActionResult> GetTopics()
        {
            var topics_list = await _context.Topics.ToListAsync();
            return Ok(topics_list);
        }

        [HttpPost("Topic/Add")]
        public async Task<ActionResult<Topic>> CreateTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Topic Successfully Added" });
        }

        [HttpPut("Topic/Modify")]
        public async Task<IActionResult> ModifyTopic(int id, Topic updatedTopic)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
                return NotFound("No Topic Found");

            topic.TopicCode = updatedTopic.TopicCode;
            topic.TopicName = updatedTopic.TopicName;
            topic.Category = updatedTopic.Category;
            topic.Description = updatedTopic.Description;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Topic updated successfully" });
        }

        //--------------------------------------------------------------------------------------------------------------
        // Actions on Events
        [HttpGet("Events/View")]
        public async Task<ActionResult> ViewAllEvents()
        {
            var events = await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Topic)
                .ToListAsync();

            if (events != null && events.Count > 0)
            {
                return Ok(events);
            }

            return NoContent();
        }

        [HttpPost("Event/Add")]
        public async Task<IActionResult> AddEvent([FromBody] Event eventObj)
        {
            if (eventObj == null)
                return BadRequest("Invalid event data.");

            var topicExists = await _context.Topics.AnyAsync(t => t.TopicId == eventObj.TopicId);
            var locationExists = await _context.Locations.AnyAsync(l => l.LocationId == eventObj.LocationId);
            var speakerExists = await _context.Speakers.AnyAsync(s => s.SpeakerId == eventObj.SpeakerId);

            if (!topicExists || !locationExists || !speakerExists)
                return BadRequest("Invalid TopicId, LocationId, or SpeakerId.");

            _context.Events.Add(eventObj);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event added successfully" });
        }

        [HttpPut("Events/Modify")]
        public async Task<ActionResult> UpdateEvent(int id, Event evnt)
        {
            if (id != evnt.EventId)
                return BadRequest("Event ID mismatch.");

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
                return NotFound(new { message = $"Event with ID {id} not found." });

            existingEvent.CourseTitle = evnt.CourseTitle;
            existingEvent.TopicId = evnt.TopicId;
            existingEvent.LocationId = evnt.LocationId;
            existingEvent.SpeakerId = evnt.SpeakerId;
            existingEvent.ClassSize = evnt.ClassSize;
            existingEvent.NumberOfDays = evnt.NumberOfDays;
            existingEvent.StartDate = evnt.StartDate;
            existingEvent.EndDate = evnt.EndDate;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Event updated successfully" });
        }

        [HttpDelete("Events/Delete")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt == null)
                return NotFound(new { message = $"Event with ID {id} not found." });

            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Event deleted successfully" });
        }

        [HttpGet("Event/{id}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var evnt = await _context.Events
                .Include(e => e.Topic)
                .Include(e => e.Location)
                .Include(e => e.Speaker)
                .Include(e => e.Participants)
                .Include(e => e.Feedbacks)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (evnt == null)
                return NotFound(new { message = $"Event with ID {id} not found." });

            return Ok(evnt);
        }
    }
}
