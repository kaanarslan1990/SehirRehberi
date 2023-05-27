using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.API.Data;

namespace SehirRehberi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private DataContext _context;
        public ValuesController(DataContext context)
        {
            _context= context;
        }

        // GET api/values

        [HttpGet]
        public async Task<ActionResult> GetValues()
        {
           var values =await _context.Values.ToListAsync();
            return Ok(values);
        }

        //GET api/values/5

        [HttpGet("id")]
        public async Task<ActionResult> GetValueAsync(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(v => v.Id == id);
            return Ok(value);
        }




    }
}
