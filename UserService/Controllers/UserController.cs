using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController: ControllerBase
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/<UserController>
    [HttpGet]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Get()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<UserController>/id
    [Authorize(Policy = "EmployeeOnly")]
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _userService.GetAsync(id);
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
    public async Task<ActionResult> Post([FromBody] UserCreateDTO users)
    {
        var result = await _userService.Add(users);
        return Ok(result);
    }

    // PUT api/<UserController>/id
    [Authorize(Policy = "EmployeeOnly")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] UserUpdateDTO users)
    {
        var result = await _userService.Update(users);
        return Ok(result);
    }

    // DELETE api/<UserController>/
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _userService.Delete(id);

        return Ok();
    }
}
