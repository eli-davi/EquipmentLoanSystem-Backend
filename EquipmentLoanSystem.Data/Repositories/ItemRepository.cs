using EquipmentLoanSystem.Core.Entities;
using EquipmentLoanSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EquipmentLoanSystem.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Items.CountAsync(cancellationToken);
        }

        public async Task<List<Item>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Items.AsNoTracking()
                .OrderBy(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}