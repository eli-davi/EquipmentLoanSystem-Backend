using EquipmentLoanSystem.Core.Entities;

namespace EquipmentLoanSystem.Core.Interfaces
{
    public interface IItemRepository
    {
        Task<int> GetCountAsync(CancellationToken cancellationToken = default);
        Task<List<Item>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}