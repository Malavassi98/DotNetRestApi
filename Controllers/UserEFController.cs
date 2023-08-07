/* using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserEFController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>{
                cfg.CreateMap<UserToAddDto,User>();
            }));
        }


    [HttpGet("GetUsers/{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);   
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User>GetUsers()
    {
        return _userRepository.GetUsers();
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? dbUser = _userRepository.GetSingleUser(user.UserId);
        if(dbUser != null){
            _userRepository.EditValues<User>(dbUser, user);
            _userRepository.SaveChanges();
            return Ok(); 
        }       
        throw new Exception("failed to update user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    { 
        User newUser = _mapper.Map<User>(user);
        _userRepository.Add<User>(newUser);
        newUser.FirstName = user.FirstName;
        if(_userRepository.SaveChanges()){
            return Ok(); 
        } 
        
        throw new Exception("failed to add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? deletingUser = _userRepository.GetSingleUser(userId);

        _userRepository.Remove(deletingUser);

        if(_userRepository.SaveChanges()) return Ok(); 
        
        throw new Exception("failed to Delete user");
    }

    // User Salary Methods

    [HttpGet("UserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);    
    }

    [HttpPut("UserSalary")]
    public IActionResult EditUserSalary(UserSalary user)
    {
        UserSalary? usDb = _userRepository.GetSingleUserSalary(user.UserId);
        if(usDb != null){
            _userRepository.EditValues<UserSalary>(usDb,user);
            _userRepository.SaveChanges();
            return Ok(); 
        }       
        throw new Exception("failed to update user Salary");
    }

    [HttpPost("UserSalary")]
    public IActionResult AddUser(UserSalary user)
    { 
        _userRepository.Add<UserSalary>(user);
        if(_userRepository.SaveChanges()) {
            return Ok(); 
        } 
        
        throw new Exception("failed to add user");
    }

    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? deletingUser = _userRepository.GetSingleUserSalary(userId);

        _userRepository.Remove(deletingUser);

        if(_userRepository.SaveChanges()) return Ok(); 
        
        throw new Exception("failed to Delete user");
    }

    // User Info Methods

    [HttpGet("UserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        return _userRepository.GetSingleUserJobInfo(userId);    
    }

    [HttpPut("UserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo user)
    {
        UserJobInfo? ujiDb = _userRepository.GetSingleUserJobInfo(user.UserId);
        if(ujiDb != null){
            _userRepository.EditValues(ujiDb, user);
            _userRepository.SaveChanges();
            return Ok(); 
        }       
        throw new Exception("failed to update user Salary");
    }

    [HttpPost("UserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo user)
    { 
        _userRepository.Add<UserJobInfo>(user);
        if(_userRepository.SaveChanges()){
            return Ok(); 
        } 
        
        throw new Exception("failed to add user");
    }

    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        UserJobInfo? deletingUser = _userRepository.GetSingleUserJobInfo(userId);

        _userRepository.Remove(deletingUser);

        if(_userRepository.SaveChanges()) return Ok(); 
        
        throw new Exception("failed to Delete user");
    }
} */
