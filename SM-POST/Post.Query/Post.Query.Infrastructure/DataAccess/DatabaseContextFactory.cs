using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public DatabaseContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            _configureDbContext(optionsBuilder);
            
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}