using EquipmentLoanSystem.Core.Entities;
using EquipmentLoanSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EquipmentLoanSystem.Data.Repositories
{
    public class ItemCopyRepository : IItemCopyRepository
    {
        private readonly AppDbContext _context;

        public ItemCopyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemCopy?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ItemCopies
                .Include(c => c.Item)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public Task UpdateAsync(ItemCopy itemCopy, CancellationToken cancellationToken = default)
        {
            _context.ItemCopies.Update(itemCopy);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}