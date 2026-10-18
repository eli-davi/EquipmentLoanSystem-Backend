using EquipmentLoanSystem.Core.DTOs;

namespace EquipmentLoanSystem.Core.Interfaces
{
    public interface IItemService
    {
        Task<PagedResult<ItemDto>> GetItemsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}