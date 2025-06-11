using EShop.Application.Service;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    // GET: api/<CategoryController>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<CategoryController>/id
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _categoryService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(result);
        }
    }

    // POST api/<CategoryController>
    [Authorize(Policy = "EmployeeOnly")]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Category category)
    {
        var result = await _categoryService.AddAsync(category);
        return Ok(result);
    }

    // PUT api/<CategoryController>/id
    [Authorize(Policy = "EmployeeOnly")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Category category)
    {
        var result = await _categoryService.Update(category);
        return Ok(result);
    }

    // DELETE api/<CategoryController>/
    [Authorize(Policy = "EmployeeOnly")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _categoryService.GetAsync(id);
        category.Deleted = true;
        var result = await _categoryService.Update(category);

        return Ok(result);
    }
}
