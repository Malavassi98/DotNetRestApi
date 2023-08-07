using DotnetAPI.Dtos;
using DotnetAPI.Models;

namespace DotnetAPI.Data 
{
    public class PostRepository: IPostRepository
    {
        private readonly DataContextDapper _dapper;
        //private readonly DataContextEF _entity;
        public PostRepository(IConfiguration configuration)
        {
                _dapper = new DataContextDapper(configuration);
                //_entity = new DataContextEF(configuration);
        }

        public IEnumerable<Post>GetPosts(int? postId = null, int? userId=null, string searchParams="")
        {
            string sqlGetPosts = "EXEC TutorialAppSchema.spPosts_Get";
            string parameters = "";

            if(postId.HasValue && postId > 0) parameters +=  $", @PostId={postId}";
            if(userId.HasValue && userId > 0) parameters +=  $", @UserId={userId}";
            if(!String.IsNullOrWhiteSpace(searchParams)) parameters += $", @SearchValue={searchParams}";

            if(parameters.Length >0) sqlGetPosts += parameters.Substring(1);
            return _dapper.loadData<Post>(sqlGetPosts);
        }

        public bool UpsertPost(PostToAddDTO postToUpsert, string userId) {
            string sqlUpsertPost = "TutorialAppSchema.spPosts_Upsert";
            string parameters = "";

            //Adding user from Claims.
            parameters+= $" @UserId={userId}";
            //Adding PostTitle
            parameters+= $", @PostTitle='{postToUpsert.PostTitle}'";
            //Adding PostContent
            parameters+= $", @PostContent='{postToUpsert.PostContent}'";
            //Adding PostID if exists
            if(postToUpsert.PostId > 0)parameters+= $", @PostId={postToUpsert.PostId}";

            sqlUpsertPost += parameters;

            return _dapper.ExecuteQuery(sqlUpsertPost);
        }

        public bool DeletePost(int postId, string userId){
            string sqlDelete = @$"EXEC TutorialAppSchema.spPost_Delete 
            @PostId = {postId} ,@UserId = {userId}";
            return _dapper.ExecuteQuery(sqlDelete); 
        }
    }
}