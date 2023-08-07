namespace DotnetAPI.Dtos {
    public partial class UserForLoginConfirmationDTO 
    {
        public byte[] PasswordHash {get;set;} = new Byte[0];
        public byte[] PasswordSalt {get;set;} = new Byte[0];
    }
}