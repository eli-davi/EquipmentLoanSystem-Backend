using EquipmentLoanSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EquipmentLoanSystem.Data.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return _context.Members.AnyAsync(member => member.Id == id, cancellationToken);
        }
    }
}
