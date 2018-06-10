using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DataContext context;
        public ValuesController(DataContext context) {
            this.context = context;

        }

        
        /* 
           This method is asynchronous that means the
           thread of the application is not blocked when
           the request to the database is made, but awaits
           until the request is done always letting 
           open the thread to make more tasks
        */
        [HttpGet] // GET api/values
        public async Task<IActionResult> GetValues() {
            var values = await this.context.Values.ToListAsync();
            return Ok(values);
        }
    
        [HttpGet("{id}")] // GET api/values/5
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await this.context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
