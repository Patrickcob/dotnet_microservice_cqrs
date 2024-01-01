using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Post.CMD.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<PostEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();

        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new ArgumentException($"Handler for query {typeof(TQuery).Name} already registered.");
            }

            _handlers.Add(typeof(TQuery), query => handler((TQuery)query));
        }

        public async Task<List<PostEntity>> SendAsync(BaseQuery query)
        {
            if (!_handlers.TryGetValue(query.GetType(), out var handler) || handler == null)
            {
                throw new ArgumentException($"Handler for query {query.GetType().Name} not found.");
            }

            return await handler(query);
        }
    }
}