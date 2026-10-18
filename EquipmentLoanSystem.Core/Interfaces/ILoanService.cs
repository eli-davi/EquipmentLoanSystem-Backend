using EquipmentLoanSystem.Core.DTOs;

namespace EquipmentLoanSystem.Core.Interfaces
{
    public interface ILoanService
    {
        Task<Result<int>> CreateLoanAsync(LoanRequestDto request, CancellationToken cancellationToken = default);
    }
}