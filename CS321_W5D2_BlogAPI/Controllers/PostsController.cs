using System;
using CS321_W5D2_BlogAPI.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CS321_W5D2_BlogAPI.Core.Services;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CS321_W5D2_BlogAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [AllowAnonymous]
        // GET /api/blogs/{blogId}/posts
        [HttpGet("/api/blogs/{blogId}/posts")]
        public IActionResult Get(int blogId)
        {
            var posts = _postService.GetBlogPosts(blogId);
            if (posts == null) return NotFound();
            return Ok(posts.ToApiModels());
        }

        
        [AllowAnonymous]
        // GET api/blogs/{blogId}/posts/{postId}
        [HttpGet("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Get(int blogId, int postId)
        {
            var post = _postService.GetBlogPosts(blogId).FirstOrDefault(p => p.Id == postId);
            if (post == null) return NotFound();
            return Ok(post.ToApiModel());
        }

        // POST /api/blogs/{blogId}/post
        [HttpPost("/api/blogs/{blogId}/posts")]
        public IActionResult Post(int blogId, [FromBody]PostModel postModel)
        {
            try
            {
                postModel.BlogId = blogId;
                var post = _postService.Add(postModel.ToDomainModel());
                return Ok(post);
            }
            catch (SystemException ex)
            {
                ModelState.AddModelError("AddPost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // PUT /api/blogs/{blogId}/posts/{postId}
        [HttpPut("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Put(int blogId, int postId, [FromBody]PostModel postModel)
        {
            try
            {
                var updatedPost = _postService.Update(postModel.ToDomainModel());
                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdatePost", ex.Message);
                return BadRequest(ModelState);
            }
        }

        // DELETE /api/blogs/{blogId}/posts/{postId}
        [HttpDelete("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Delete(int blogId, int postId)
        {
            _postService.Remove(postId);
            return NoContent();

        }
    }
}
