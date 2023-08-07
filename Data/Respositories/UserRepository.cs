using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Data 
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContextDapper _dapper;
        //private readonly DataContextEF _entity;
        private readonly ReusableSql _reusablesql;
        public UserRepository(IConfiguration configuration)
        {
                _dapper = new DataContextDapper(configuration);
                _reusablesql = new ReusableSql(configuration);
                //_entity = new DataContextEF(configuration);
        }

        public bool UpsertUser(UserComplete user) {
            return _reusablesql.UpsertUser(user);
        }

        public bool DeleteUser(int userId){
            string deleteSQL = @$"EXEC TutorialAppSchema.spUser_Delete @UserId = {userId}";
            return _dapper.ExecuteQuery(deleteSQL); 
        }

        public IEnumerable<UserComplete>GetUsers(int? userId, bool? isActive)
        {
            string selectStoreProcedure = @"EXEC TutorialAppSchema.spUsers_Get";
            string parameters = "";
            if(userId is not null && userId>0) parameters  += $", @UserId={userId}";
            if(isActive is not null) parameters  += $", @Active={isActive}";

            if(parameters.Length > 0 )selectStoreProcedure += parameters.Substring(1);
            return _dapper.loadData<UserComplete>(selectStoreProcedure); 
        }

        //--------------------------------------------- Entity Framework -----------------------------------------
        // public bool SaveChanges(){
        //     return _entity.SaveChanges() > 0;
        // }

        // public User GetSingleUser(int userId)
        // {
        //     return _entity.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();    
        // }

        // public void Remove<T>(T entityToRemove)
        // {
        //     if(entityToRemove is not null) _entity.Remove(entityToRemove);
        // }

        // public UserSalary GetSingleUserSalary(int userId)
        // {
        //     return _entity.UserSalary.Where(u => u.UserId == userId).FirstOrDefault<UserSalary>();    
        // }

        // public UserJobInfo GetSingleUserJobInfo(int userId)
        // {
        //     return _entity.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();    
        // }
        // public void EditValues<T>(T dbValue, T dbValueToSet)
        // {
        //     _entity.Entry(dbValue).CurrentValues.SetValues(dbValueToSet);    
        // }
    }
}