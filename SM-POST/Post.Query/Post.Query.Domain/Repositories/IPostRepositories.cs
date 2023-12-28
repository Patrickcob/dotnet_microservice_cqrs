using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.CMD.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface IPostRepositories
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(Guid postId);
        Task<PostEntity> GetByIdAsync(Guid postId);
        Task<List<PostEntity>> GetAllAsync();
        Task<List<PostEntity>> GetByAuthorAsync(string author);
        Task<List<PostEntity>> GetByLikesAsync(int likes);
        Task<List<PostEntity>> GetByCommentsAsync();
    }
}