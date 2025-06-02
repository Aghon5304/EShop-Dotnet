using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Models.Entities;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IManageUserService manageUserService) : ControllerBase
{
    private readonly IManageUserService _manageUserService = manageUserService;

    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _manageUserService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<UserController>/id
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _manageUserService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(result);
        }
    }

    // POST api/<UserController>
    [Authorize(Policy = "EmployeeOnly")]
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] User users)
    {
        var result = await _manageUserService.Add(users);
        return Ok(result);
    }

    // PUT api/<UserController>/id
    [Authorize(Policy = "EmployeeOnly")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] User users)
    {
        var result = await _manageUserService.Update(users);
        return Ok(result);
    }

    // DELETE api/<UserController>/
    [Authorize(Policy = "EmployeeOnly")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var users = await _manageUserService.GetAsync(id);
        users.IsActive = false;
        var result = await _manageUserService.Update(users);

        return Ok(result);
    }
}
