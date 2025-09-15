using Adminstrator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
        //---------------------------------------------------------------------------------------------------------------------------
        //Actions On Events
        [HttpPost("PromoCode/add")]
        public async Task<ActionResult> AddPromoCode(PromotionCode code_obj)
        {
            if (code_obj != null)
            {
                await _context.PromotionCodes.AddAsync(code_obj);
                await _context.SaveChangesAsync();
                return Ok("New PromoCode Sucessfully Added");
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("promocode/view")]

        public async Task<ActionResult<PromotionCode>> ViewPromoCodes()
        {
            var p_codes = await _context.PromotionCodes.ToListAsync<PromotionCode>();
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
                return Ok("PromoCode Deleted Sucessfully");
            }
            else
            {
                return NotFound("No PromoCode Found");
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------

        //Action on Administrators
        [HttpGet("Admin/View")]
        public async Task<ActionResult<Administrator>> GetAllAdmins()
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
        public async Task<ActionResult<Administrator>> AddAdmin(Administrator admin_obj)
        {
            if (admin_obj != null)
            {
                await _context.Administrators.AddAsync(admin_obj);
                await _context.SaveChangesAsync();
                return Ok("Administrator added sucessfully");
            }

            return NoContent();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------

        //Actions on Locations  
        [HttpPost("Location/Add")]
        public async Task<ActionResult> AddLocation(Location loc_obj)
        {
            if (loc_obj != null)
            {
                await _context.Locations.AddAsync(loc_obj);
                await _context.SaveChangesAsync();
                return Ok("New Location Sucessfully Added");
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
        //--------------------------------------------------------------------------------------------------------------
        //Action on events
        [HttpGet("Event/View")]
        public async Task<ActionResult> ViewEvents()
        {
            var events = await _context.Events.ToListAsync();
            if (events != null && events.Count > 0)
            {
                return Ok(events);
            }
            return NoContent();
        }

        [HttpPost("Event/Add")]
        public async Task<ActionResult> AddEvent(Event event_obj)
        {
            await _context.Events.AddAsync(event_obj);
            await _context.SaveChangesAsync();
            return Ok("Added new Event");
        }
        //--------------------------------------------------------------------------------------------------------------
        //Action on Speakers
        [HttpPost("Speakers/Add")]
        public async Task<ActionResult> AddSpeaker(Speaker speaker_obj, string username, string password)
        {
            if (speaker_obj == null || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Missing speaker data or credentials.");

            // Create User for Speaker
            var user = new User
            {
                Username = username,
                Password = password, // Always hash!
                Role = "Speaker"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Link Speaker to User
            speaker_obj.UserId = user.UserId;

            _context.Speakers.Add(speaker_obj);
            await _context.SaveChangesAsync();

            return Ok("New Speaker Successfully Added");
        }

        [HttpGet("Speakers/View")]
        public async Task<ActionResult> ViewSpeakers()
        {
            var spea = await _context.Speakers.ToListAsync();
            if (spea != null && spea.Count > 0)
            {
                return Ok(spea);
            }
            return NoContent();

        }
    }
}
