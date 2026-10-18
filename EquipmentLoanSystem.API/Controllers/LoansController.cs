using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentLoanSystem.API.Controllers
{
    /*[Authorize]*/
    [ApiController]
    [Route("api/loans")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanRequestDto dto, CancellationToken cancellationToken)
        {
            var result = await _loanService.CreateLoanAsync(dto, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(new { loanId = result.Value, message = "ההשאלה בוצעה בהצלחה" });
            }

            return StatusCode(result.ErrorCode, new { message = result.ErrorMessage });
        }
    }
}