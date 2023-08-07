using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Data {
    public interface IPostRepository {
        public IEnumerable<Post>GetPosts(int? postId = null, int? userId=null, string searchParams="");

        public bool UpsertPost(PostToAddDTO post, string userId);

        public bool DeletePost(int postId, string userId);

        //-------------------------------- Entity Framework --------------------------------------

        // public User GetSingleUser(int userId);

        // public UserSalary GetSingleUserSalary(int userId);

        // public UserJobInfo GetSingleUserJobInfo(int userId);

        // public void EditValues<T>(T dbValue, T dbValueToSet);
    }
}