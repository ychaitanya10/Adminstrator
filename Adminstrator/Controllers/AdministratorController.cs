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
            if ( p_codes!= null && p_codes.Count > 0)
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

    }
}
