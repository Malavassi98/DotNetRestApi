using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Helpers {
    public class ReusableSql
    {
        private readonly DataContextDapper _dapper;
        public ReusableSql(IConfiguration config){
            _dapper = new DataContextDapper(config);
        }

        public bool UpsertUser(UserComplete user) {
            string upsertSQL = @$"
            EXEC TutorialAppSchema.spUser_Upsert
                @FirstName,
                @LastName,
                @Email,
                @Gender,
                @JobTitle,
                @Department,
                @Salary,
                @Active,
                @UserId";
            return _dapper.ExecuteQuery(upsertSQL,user);
        }
    }
}