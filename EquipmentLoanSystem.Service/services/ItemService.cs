using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Core.Interfaces;

namespace EquipmentLoanSystem.Service.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<PagedResult<ItemDto>> GetItemsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await _itemRepository.GetCountAsync(cancellationToken);
            var items = await _itemRepository.GetPagedAsync(page, pageSize, cancellationToken);

            var itemDtos = items.Select(i => new ItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description
            }).ToList();

            return new PagedResult<ItemDto>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = itemDtos
            };
        }
    }
}