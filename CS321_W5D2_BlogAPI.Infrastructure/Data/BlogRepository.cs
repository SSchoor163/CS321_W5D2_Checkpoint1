using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Blog> GetAll()
        {
            var blogs = _dbContext.Blogs.Include(u => u.User);
            if (blogs == null) return null;
            return blogs;
        }

        public Blog Get(int id)
        {
            var blog = _dbContext.Blogs.Include(b => b.User)
                .FirstOrDefault(b => b.Id == id);
            if (blog == null) return null;
            return blog;
        }

        public Blog Add(Blog blog)
        {
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return blog;
        }

        public Blog Update(Blog updatedItem)
        {
            var existingItem = _dbContext.Blogs.Find(updatedItem.Id);
            if (existingItem == null) return null;
            _dbContext.Entry(existingItem)
               .CurrentValues
               .SetValues(updatedItem);
            _dbContext.Blogs.Update(existingItem);
            _dbContext.SaveChanges();
            return existingItem;
        }

        public void Remove(int id)
        { 
            var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null) throw new SystemException("Blog not found.");
            _dbContext.Blogs.Remove(blog);
            _dbContext.SaveChanges();
        }
    }
}
