using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers {
    public class AuthHelper {

        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        public AuthHelper(IConfiguration config) {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        public byte[] GetPasswordHash(string password, byte[] passwordSalt) 
        {
            string passwordSaltPlusString = _config.GetSection("AppSetting:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );

            return passwordHash;
        }

        public string CreateToken(int userId) 
        {
            Claim[] claims = new Claim[]{
                new Claim("userId", userId.ToString())
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                        tokenKeyString ?? ""
                    )
            ); 

            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject= new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires= DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);

        }

        public bool CreatePassword(UserForLoginDTO userForSetPassword) {
            byte[] passwordSalt = new byte[128 / 8];
            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte [] passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);

            AuthDTO newUserAuth = new AuthDTO{Email = userForSetPassword.Email,PasswordHash = passwordHash,PasswordSalt = passwordSalt};

            string sqlAddAuth = @$"EXEC TutorialAppSchema.spRegistration_Upsert @Email,
            @PasswordHash, @PasswordSalt";

            return _dapper.ExecuteQuery(sqlAddAuth,newUserAuth);
        }
    }
}