using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TakeMyTime.DAL;

namespace TakeMyTime.BLL.Tests
{
    public class TestUtility
    {
        TakeMyTimeDbContext testContext;

        public static void CreateTestContext()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<TakeMyTimeDbContext>()
                .UseSqlite(connection)
                .Options;
            }
            catch (Exception e)
            {
            }
            finally
            {
                connection.Close();
            }
        }

        private static void CreateDatabase()
        {

        }
    }
}
