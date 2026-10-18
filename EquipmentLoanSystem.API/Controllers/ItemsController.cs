using AutoMapper;
using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentLoanSystem.API.Controllers
{
    [ApiController]
    [Route("api/items")] // הגדרה מפורשת של הנתיב כדי למנוע שגיאות 404
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        public ItemsController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _itemService.GetItemsAsync(page, pageSize);
            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(result.Items);
             return Ok(new
            {
                totalCount = result.TotalCount,
                page,
                pageSize,
                items = itemDtos
            });
        }
    }
}


