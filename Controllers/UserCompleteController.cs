using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly IUserRepository _userRepository;
    public UserCompleteController(IConfiguration configuration, IUserRepository userRepository)
        {
            _dapper = new DataContextDapper(configuration);
            _userRepository = userRepository;
        }

    [HttpGet("GetUsers/{userId?}/{isActive?}")]
    public IEnumerable<UserComplete>GetUsers(int? userId= null, bool? isActive= null)
    {
        return _userRepository.GetUsers(userId,isActive); 
    }

    [HttpPut("EditUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
        
        if(_userRepository.UpsertUser(user)) return Ok(); 
        
        throw new Exception("failed to update user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {

        if(_userRepository.DeleteUser(userId)) return Ok(); 
        
        throw new Exception("failed to Delete user");
    }
}
