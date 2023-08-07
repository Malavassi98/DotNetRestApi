using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controller {

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController: ControllerBase 
    {
        private readonly DataContextDapper _dapper;
        private readonly IPostRepository _postRepository;
        public PostController (IConfiguration config, IPostRepository postRepository) {
            _dapper = new DataContextDapper(config);
            _postRepository = postRepository;
        }

        [HttpGet("Posts/{postId?}/{userId?}/{searchParams?}")]
        public IEnumerable<Post> GetPosts(int? postId = 0, int? userId=0, string searchParams="") {
              
            return _postRepository.GetPosts(postId,userId,searchParams);
        }

        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(PostToAddDTO postToUpsert) {

            string? activeUser = this.User.FindFirst("userId")?.Value ?? "-1";
            if (_postRepository.UpsertPost(postToUpsert, activeUser)){
                return Ok();
            };

            throw new Exception("Failed to upsert new post");
        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId) {
            string? activeUser = this.User.FindFirst("userId")?.Value ?? "-1";
            if (_postRepository.DeletePost(postId, activeUser)){
                return Ok();
            };

            throw new Exception("Failed to delete post");
        }

    }
}