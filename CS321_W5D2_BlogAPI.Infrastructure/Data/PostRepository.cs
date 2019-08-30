using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _dbContext;
        public PostRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Post Get(int id)
        {
            var post = _dbContext.Posts.Include(b => b.Blog).ThenInclude(b => b.User)
                .FirstOrDefault(p => p.Id == id);
            if (post == null) return null;
            return post;
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            var posts = _dbContext.Posts.Include(b => b.Blog).ThenInclude(b => b.User)
                .Where(p => p.BlogId == blogId).ToList();
            if (posts == null) return null;
            return posts;
        }

        public Post Add(Post Post)
        {
            _dbContext.Add(Post);
            _dbContext.SaveChanges();
            return Post;
        }

        public Post Update(Post Post)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == Post.Id);
            if (post == null) return null;
            _dbContext.Entry(post).CurrentValues.SetValues(Post);
            _dbContext.Update(Post);
            _dbContext.SaveChanges();
            return Post;
        }

        public IEnumerable<Post> GetAll()
        {
            var posts = _dbContext.Posts;
            return posts;
        }

        public void Remove(int id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) throw new SystemException("Can not find Post to delete");
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
        }

    }
}
