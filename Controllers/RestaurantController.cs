using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantRaterAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RestaurantRaterAPI.Controllers
{
    [ApiController]
    [Route("controller")]
    public class RestaurantController : Controller{
        private RestaurantDbContext _context;
        public RestaurantController(RestaurantDbContext context){
            _context = context;
        }

        public virtual List<Rating> Ratings{get;set;} = new List<Rating>();
        public double AverageRating{
            get{
                if(Ratings.Count == 0){
                    return 0;
                }
                double total = 0.0;
                foreach(Rating rating in Ratings){
                    total += rating.Score;
                }
                return total / Ratings.Count;
            }
        }
    [HttpPost]
    public async Task<IActionResult> PostRestaurant([FromForm] RestaurantEdit model){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

    _context.Restaurants.Add(new Restaurant(){
        Name = model.Name,
        Location = model.Location,
    });

    await _context.SaveChangesAsync();
    return Ok();
}
    [HttpPost]
    public async Task<IActionResult> GetAllRestaurants(){
        var restaurants = await _context.Restaurants.Include(r => r.Ratings).ToListAsync();
        return Ok(restaurants);
    }

    [HttpGet]
    public async Task<IActionResult> GetRestaurants(){
        var restaurants = await _context.Restaurants.ToListAsync();
        return Ok();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetRestaurantById(int id){
        var restaurant = await _context.Restaurants.Include(r => r.Ratings).FirstOrDefaultAsync(r => r.Id == id);
    return Ok();  

        if (restaurant == null){
            return NotFound();
        }

        return Ok(restaurant);

        await _context.SaveChangesAsync();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateRestaurant([FromForm] RestaurantEdit model, [FromRoute] int id){
        var oldRestaurant = await _context.Restaurants.FindAsync(id);

        if(oldRestaurant == null){
            return NotFound();
        }

        if(!ModelState.IsValid){
            return BadRequest();
        }

        if(!string.IsNullOrEmpty(model.Name)){
            oldRestaurant.Name = model.Name;
        }

        if(!string.IsNullOrEmpty(model.Location)){
            oldRestaurant.Location = model.Location;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id){
        var restaurant = await _context.Restaurants.FindAsync(id);
        if(restaurant == null){
            return NotFound();
        }

        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
}