using EShop.Application.Service;
using Microsoft.AspNetCore.Mvc;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace EShopService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
	private readonly IProductService _productService = productService;

	// GET: api/<ProductController>
	[HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _productService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<ProductController>/id
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _productService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(result);
        }
		}

		// POST api/<ProductController>
		[Authorize(Policy = "EmployeeOnly")]
		[HttpPost]
    public async Task<ActionResult> Post([FromBody]Product product)
    {
        var result = await _productService.AddAsync(product);
        return Ok(result);
		}

		// PUT api/<ProductController>/id
		[Authorize(Policy = "EmployeeOnly")]
		[HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody]Product product)
    {
        var result = await _productService.UpdateAsync(product);
			return Ok(result);
	}

	// DELETE api/<ProductController>/
	[Authorize(Policy = "EmployeeOnly")]
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(int id)
	{
		var product = await _productService.GetAsync(id);
		product.Deleted = true;
		var result = await _productService.UpdateAsync(product);

		return Ok(result);
	}
}
