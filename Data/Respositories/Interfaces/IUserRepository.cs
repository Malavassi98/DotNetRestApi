using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Data {
    public interface IUserRepository {
        public IEnumerable<UserComplete>GetUsers(int? userId, bool? active);

        public bool UpsertUser(UserComplete user);

        public bool DeleteUser(int userId);

        //-------------------------------- Entity Framework --------------------------------------

        // public User GetSingleUser(int userId);

        // public UserSalary GetSingleUserSalary(int userId);

        // public UserJobInfo GetSingleUserJobInfo(int userId);

        // public void EditValues<T>(T dbValue, T dbValueToSet);
    }
}