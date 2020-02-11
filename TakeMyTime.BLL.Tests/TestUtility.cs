using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TakeMyTime.DAL;
using TakeMyTime.DAL.uow;

namespace TakeMyTime.BLL.Tests
{
    public class TestUtility
    {
        TakeMyTimeDbContext testContext;

        public async static Task<UnitOfWork> CreateTestUnitOfWork()
        {
            var options = new DbContextOptionsBuilder<TakeMyTimeDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

            using (TakeMyTimeDbContext context = new TakeMyTimeDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();
            }

            return new UnitOfWork(options);
        }
    }
}
