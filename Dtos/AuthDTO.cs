namespace DotnetAPI.Dtos {
    public class AuthDTO {
        public string Email {get;set;} = "";
        public byte[] PasswordHash {get;set;} = new byte[0];
        public byte[] PasswordSalt {get;set;} = new byte[0];
    }
}