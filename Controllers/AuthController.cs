using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controller {

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase {

        private readonly DataContextDapper _dapper;    
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _reusablesql;

        private readonly IMapper _mapper;
        public AuthController (IConfiguration config) {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
            _reusablesql = new ReusableSql(config);
            _mapper = new Mapper( new MapperConfiguration(cfg =>{
                cfg.CreateMap<UserForRegistrationDTO, UserComplete>();
            }));
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDTO userForRegistration)
        {
            if(userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUser = $"SELECT Email FROM TutorialAppSchema.Auth Where Email = '{userForRegistration.Email}'";
                string? existingUser = _dapper.loadDataSingle<String>(sqlCheckUser);
                if(existingUser is null)
                {
                    UserForLoginDTO userForSetPassword = new UserForLoginDTO {
                        Email= userForRegistration.Email,
                        Password= userForRegistration.Password
                    };

                    _authHelper.CreatePassword(userForSetPassword);

                   UserComplete userToAdd = _mapper.Map<UserComplete>(userForRegistration);
                   userToAdd.Active = true;
                    
                    if(_reusablesql.UpsertUser(userToAdd)) return Ok();
                    throw new Exception("User failed to registered.");
                }
                throw new Exception("User with this email already exists.");
            }
            throw new Exception("Passwords do not match");
        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserForLoginDTO userForSetPassword)
        {
            if(_authHelper.CreatePassword(userForSetPassword)) return Ok();
            throw new Exception("Passwords couldn't be updated");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDTO userForLogin)
        {
            byte[] passwordHash;
            string sqlForHashAndSalt = $"EXEC TutorialAppSchema.spLoginConfirmation_Get @Email='{userForLogin.Email}'";

            UserForLoginConfirmationDTO? userData = _dapper.loadDataSingle<UserForLoginConfirmationDTO>(sqlForHashAndSalt);

            if(userData is not null)
            {
                passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userData.PasswordSalt);

                // if(passwordHash == userData.PasswordHash) this don't work because is comparing object so it will compare there location
                for(int index = 0;index < passwordHash.Length;index++){
                    if(passwordHash[index] != userData.PasswordHash[index]){
                        return StatusCode(401, "password is incorrect");
                    }
                }

                string userIdSql = $"Select UserId from TutorialAppSchema.Users Where Email = '{userForLogin.Email}'";
                int userId = _dapper.loadDataSingle<int>(userIdSql);
                return Ok(new Dictionary<string,string> {
                    {"token", _authHelper.CreateToken(userId)}
                });
            }
            return StatusCode(401, "user does not exist"); 
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string sqlGetUserId = $"Select UserId from TutorialAppSchema.Users Where UserId = '{User.FindFirst("userId")?.Value ?? ""}'";
            int userId = _dapper.loadDataSingle<int>(sqlGetUserId);

            return _authHelper.CreateToken(userId);
        }

    }
}