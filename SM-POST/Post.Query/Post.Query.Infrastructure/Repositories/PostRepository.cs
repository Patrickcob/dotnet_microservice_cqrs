using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.CMD.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepositories
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public PostRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Add(post);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);

            if (post == null)
            {
                return;
            }

            context.Posts.Remove(post);
            await context.SaveChangesAsync();

        }

        public async Task<List<PostEntity>> GetAllAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PostEntity>> GetByAuthorAsync(string author)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .Where(x => x.Author.Contains(author))
                .ToListAsync();
        }

        public async Task<List<PostEntity>> GetByCommentsAsync()
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .Where(x => x.Comments != null && x.Comments.Any())
                .ToListAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<List<PostEntity>> GetByLikesAsync(int likes)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(x => x.Likes)
                .AsNoTracking()
                .Where(x => x.Likes >= likes )
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Posts.Update(post);

            await context.SaveChangesAsync();
        }
    }
}