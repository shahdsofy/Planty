using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.Models;

namespace Planty.Controllers
{
	[Route("api/admin/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class AdminPlantController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public AdminPlantController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/admin/plants
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
		{
			return await _context.Plants.ToListAsync();
		}

		// GET: api/admin/plants/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Plant>> GetPlant(int id)
		{
			var plant = await _context.Plants.FindAsync(id);

			if (plant == null)
				return NotFound();

			return plant;
		}

		// POST: api/admin/plants
		[HttpPost]
		public async Task<ActionResult<Plant>> CreatePlant([FromBody] Plant plant)
		{
			_context.Plants.Add(plant);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetPlant), new { id = plant.ID }, plant);
		}

		// PUT: api/admin/plants/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePlant(int id, [FromBody] Plant updatedPlant)
		{
			if (id != updatedPlant.ID)
				return BadRequest();

			var plant = await _context.Plants.FindAsync(id);
			if (plant == null)
				return NotFound();

			plant.Name = updatedPlant.Name;
			plant.Type = updatedPlant.Type;
			plant.Price = updatedPlant.Price;
			plant.ImagePath = updatedPlant.ImagePath;
			plant.Details = updatedPlant.Details;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/admin/plants/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePlant(int id)
		{
			var plant = await _context.Plants.FindAsync(id);
			if (plant == null)
				return NotFound();

			_context.Plants.Remove(plant);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
