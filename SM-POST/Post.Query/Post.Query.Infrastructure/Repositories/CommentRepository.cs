using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.CMD.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public CommentRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CreateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Add(comment);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);

            if (comment == null)
            {
                return;
            }

            context.Comments.Remove(comment);
            
            await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Update(comment);

            await context.SaveChangesAsync();
        }
    }
}