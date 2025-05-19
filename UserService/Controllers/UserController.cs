using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Models;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IManageUsersService manageUsersService) : ControllerBase
{
    private readonly IManageUsersService _manageUsersService = manageUsersService;

    // GET: api/<UserController>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _manageUsersService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<UserController>/id
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _manageUsersService.GetAsync(id);
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
    public async Task<ActionResult> Post([FromBody] Users users)
    {
        var result = await _manageUsersService.Add(users);
        return Ok(result);
    }

    // PUT api/<UserController>/id
    [Authorize(Policy = "EmployeeOnly")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Users users)
    {
        var result = await _manageUsersService.Update(users);
        return Ok(result);
    }

    // DELETE api/<UserController>/
    [Authorize(Policy = "EmployeeOnly")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var users = await _manageUsersService.GetAsync(id);
        users.IsActive = false;
        var result = await _manageUsersService.Update(users);

        return Ok(result);
    }
}
