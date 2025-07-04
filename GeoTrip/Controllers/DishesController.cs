using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoTrip.Data;
using GeoTrip.Models;
using GeoTrip.DTO; // Add your DTOs namespace

namespace GeoTrip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly GeoVoyageDbContext _context;

        public DishesController(GeoVoyageDbContext context)
        {
            _context = context;
        }

        // GET: api/dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetDishes()
        {
            try
            {
                var dishes = await _context.Dishes
                    .OrderBy(d => d.Name)
                    .Select(d => new DishDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        ImageUrl = d.ImageUrl,
                        Category = d.Category,
                        Difficulty = d.Difficulty,
                        Rating = d.Rating,
                        CreatedAt = d.CreatedAt,
                        HasRecipe = d.Recipe != null
                    })
                    .ToListAsync();

                return Ok(dishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/dishes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DishDto>> GetDish(int id)
        {
            try
            {
                var dish = await _context.Dishes
                    .Where(d => d.Id == id)
                    .Select(d => new DishDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        ImageUrl = d.ImageUrl,
                        Category = d.Category,
                        Difficulty = d.Difficulty,
                        Rating = d.Rating,
                        CreatedAt = d.CreatedAt,
                        HasRecipe = d.Recipe != null
                    })
                    .FirstOrDefaultAsync();

                if (dish == null)
                {
                    return NotFound(new { message = $"Dish with ID {id} not found." });
                }

                return Ok(dish);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/dishes/category/{category}
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetDishesByCategory(string category)
        {
            try
            {
                var dishes = await _context.Dishes
                    .Where(d => d.Category.ToLower() == category.ToLower())
                    .Select(d => new DishDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        ImageUrl = d.ImageUrl,
                        Category = d.Category,
                        Difficulty = d.Difficulty,
                        Rating = d.Rating,
                        CreatedAt = d.CreatedAt,
                        HasRecipe = d.Recipe != null
                    })
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                return Ok(dishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/dishes/{dishId}/recipe
        [HttpGet("{dishId}/recipe")]
        public async Task<ActionResult<RecipeDto>> GetDishRecipe(int dishId)
        {
            try
            {
                var recipe = await _context.Recipes
                    .Include(r => r.RecipeIngredients)
                    .Where(r => r.DishId == dishId)
                    .Select(r => new RecipeDto
                    {
                        Id = r.Id,
                        DishId = r.DishId,
                        PrepTime = r.PrepTime,
                        CookTime = r.CookTime,
                        Servings = r.Servings,
                        Instructions = r.Instructions,
                        Tips = r.Tips,
                        DifficultyLevel = r.DifficultyLevel,
                        Ingredients = r.RecipeIngredients
                            .OrderBy(i => i.OrderIndex)
                            .Select(i => new RecipeIngredientDto
                            {
                                Id = i.Id,
                                IngredientName = i.IngredientName,
                                Quantity = i.Quantity,
                                Unit = i.Unit,
                                OrderIndex = i.OrderIndex
                            }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (recipe == null)
                {
                    return NotFound(new { message = "Recipe not found for this dish" });
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/dishes/{dishId}/has-recipe
        [HttpGet("{dishId}/has-recipe")]
        public async Task<ActionResult<object>> HasRecipe(int dishId)
        {
            try
            {
                var hasRecipe = await _context.Recipes.AnyAsync(r => r.DishId == dishId);
                return Ok(new { hasRecipe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/dishes
        [HttpPost]
        public async Task<ActionResult<DishDto>> CreateDish(CreateDishDto createDishDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dish = new Dish
                {
                    Name = createDishDto.Name,
                    Description = createDishDto.Description,
                    ImageUrl = createDishDto.ImageUrl,
                    Category = createDishDto.Category,
                    Difficulty = createDishDto.Difficulty,
                    Rating = createDishDto.Rating
                };

                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();

                var dishDto = new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    ImageUrl = dish.ImageUrl,
                    Category = dish.Category,
                    Difficulty = dish.Difficulty,
                    Rating = dish.Rating,
                    CreatedAt = dish.CreatedAt,
                    HasRecipe = false
                };

                return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dishDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/dishes/{dishId}/recipe
        [HttpPost("{dishId}/recipe")]
        public async Task<ActionResult<RecipeDto>> CreateRecipe(int dishId, CreateRecipeDto createRecipeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dish = await _context.Dishes.FindAsync(dishId);
                if (dish == null)
                {
                    return NotFound(new { message = "Dish not found" });
                }

                var existingRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.DishId == dishId);
                if (existingRecipe != null)
                {
                    return BadRequest(new { message = "Recipe already exists for this dish" });
                }

                var recipe = new Recipe
                {
                    DishId = dishId,
                    PrepTime = createRecipeDto.PrepTime,
                    CookTime = createRecipeDto.CookTime,
                    Servings = createRecipeDto.Servings,
                    Instructions = createRecipeDto.Instructions,
                    Tips = createRecipeDto.Tips,
                    DifficultyLevel = createRecipeDto.DifficultyLevel
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                // Add ingredients
                if (createRecipeDto.Ingredients != null && createRecipeDto.Ingredients.Any())
                {
                    var ingredients = createRecipeDto.Ingredients.Select((ing, index) => new RecipeIngredient
                    {
                        RecipeId = recipe.Id,
                        IngredientName = ing.IngredientName,
                        Quantity = ing.Quantity,
                        Unit = ing.Unit,
                        OrderIndex = ing.OrderIndex > 0 ? ing.OrderIndex : index + 1
                    }).ToList();

                    _context.RecipeIngredients.AddRange(ingredients);
                    await _context.SaveChangesAsync();
                }

                // Return the created recipe
                var recipeDto = await GetDishRecipe(dishId);
                return recipeDto;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/dishes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<DishDto>> UpdateDish(int id, UpdateDishDto updateDishDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dish = await _context.Dishes.FindAsync(id);
                if (dish == null)
                {
                    return NotFound(new { message = $"Dish with ID {id} not found." });
                }

                // Update properties
                dish.Name = updateDishDto.Name ?? dish.Name;
                dish.Description = updateDishDto.Description ?? dish.Description;
                dish.ImageUrl = updateDishDto.ImageUrl ?? dish.ImageUrl;
                dish.Category = updateDishDto.Category ?? dish.Category;
                dish.Difficulty = updateDishDto.Difficulty ?? dish.Difficulty;
                dish.Rating = updateDishDto.Rating ?? dish.Rating;

                _context.Entry(dish).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var dishDto = new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    ImageUrl = dish.ImageUrl,
                    Category = dish.Category,
                    Difficulty = dish.Difficulty,
                    Rating = dish.Rating,
                    CreatedAt = dish.CreatedAt,
                    HasRecipe = await _context.Recipes.AnyAsync(r => r.DishId == dish.Id)
                };

                return Ok(dishDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound(new { message = $"Dish with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // DELETE: api/dishes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(id);
                if (dish == null)
                {
                    return NotFound(new { message = $"Dish with ID {id} not found." });
                }

                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Dish '{dish.Name}' deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // DELETE: api/dishes/{dishId}/recipe
        [HttpDelete("{dishId}/recipe")]
        public async Task<IActionResult> DeleteRecipe(int dishId)
        {
            try
            {
                var recipe = await _context.Recipes
                    .Include(r => r.RecipeIngredients)
                    .FirstOrDefaultAsync(r => r.DishId == dishId);

                if (recipe == null)
                {
                    return NotFound(new { message = "Recipe not found for this dish" });
                }

                _context.Recipes.Remove(recipe); // This will cascade delete ingredients
                await _context.SaveChangesAsync();

                return Ok(new { message = "Recipe deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // Helper method
        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}