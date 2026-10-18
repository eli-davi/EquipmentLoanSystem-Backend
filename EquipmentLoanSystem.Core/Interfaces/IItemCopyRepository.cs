using EquipmentLoanSystem.Core.Entities;

namespace EquipmentLoanSystem.Core.Interfaces
{
    public interface IItemCopyRepository
    {
        Task<ItemCopy?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateAsync(ItemCopy itemCopy, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}