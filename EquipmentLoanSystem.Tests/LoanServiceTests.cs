using System;
using System.Threading.Tasks;
using EquipmentLoanSystem.Core.Entities;
using EquipmentLoanSystem.Core.Enums;
using EquipmentLoanSystem.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EquipmentLoanSystem.Tests
{
    public class LoanConcurrencyTests : IDisposable
    {
        private readonly SqliteConnection _connection;

        public LoanConcurrencyTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
        }

        private DbContextOptions<AppDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;
        }

        [Fact]
        public async Task TwoDbContexts_ConcurrentUpdate_ThrowsConcurrencyException()
        {
            // Arrange
            var options = CreateOptions();

            using (var contextSetup = new AppDbContext(options))
            {
                await contextSetup.Database.EnsureCreatedAsync();

                // 1. יצירת והוספת הפריט (Item) כדי לקיים את אילוץ המפתח הזר
                var item = new Item
                {
                    Id = 1,
                    Name = "מצלמה לבדיקה",
                    Description = "תיאור בדיקה"
                };
                contextSetup.Items.Add(item);

                // 2. יצירת העותק הפיזי (ItemCopy) המקושר לפריט
                var itemCopy = new ItemCopy
                {
                    Id = 1,
                    ItemId = 1,
                    SerialNumber = "SN-CONCURRENCY-1",
                    Status = CopyStatus.Available,
                    Version = Guid.NewGuid()
                };

                contextSetup.ItemCopies.Add(itemCopy);
                await contextSetup.SaveChangesAsync();
            }

            // Act
            using var context1 = new AppDbContext(options);
            using var context2 = new AppDbContext(options);

            var copyUser1 = await context1.ItemCopies.FindAsync(1);
            var copyUser2 = await context2.ItemCopies.FindAsync(1);

            Assert.NotNull(copyUser1);
            Assert.NotNull(copyUser2);

            // משתמש 1 מעדכן ושומר
            copyUser1.Status = CopyStatus.Loaned;
            copyUser1.Version = Guid.NewGuid();
            await context1.SaveChangesAsync();

            // משתמש 2 מנסה לעדכן על סמך הגרסה הישנה
            copyUser2.Status = CopyStatus.Loaned;
            copyUser2.Version = Guid.NewGuid();

            // Assert - השמירה השנייה אמורה להיכשל בגלל התנגשות גרסאות
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await context2.SaveChangesAsync();
            });
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}