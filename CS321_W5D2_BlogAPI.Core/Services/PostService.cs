using System;
using System.Collections.Generic;
using CS321_W5D2_BlogAPI.Core.Models;

namespace CS321_W5D2_BlogAPI.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IUserService userService)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userService = userService;
        }

        public Post Add(Post newPost)
        {
            if (_userService.CurrentUserId == _blogRepository.Get(newPost.BlogId).User.Id)
            {
                newPost.DatePublished = DateTime.Now;
                return _postRepository.Add(newPost);
            }
            else throw new SystemException("You do not have authority to add a post to this blog");
        }

        public Post Get(int id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }
        
        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _postRepository.GetBlogPosts(blogId);
        }

        public void Remove(int id)
        {
            var post = this.Get(id);
            if (_userService.CurrentUserId == _blogRepository.Get(post.BlogId).User.Id)
                _postRepository.Remove(id);
            else throw new SystemException("You do not have authority to delete this post");
        }

        public Post Update(Post updatedPost)
        {
            if (_userService.CurrentUserId == _blogRepository.Get(updatedPost.BlogId).User.Id)
                return _postRepository.Update(updatedPost);
            else throw new SystemException("You do not have authority to change this blog post.");
        }

    }
}
