using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Core.Entities;
using EquipmentLoanSystem.Core.Enums;
using EquipmentLoanSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EquipmentLoanSystem.Service.Services
{
    public class LoanService : ILoanService
    {
        private readonly IItemCopyRepository _itemCopyRepository;

        public LoanService(IItemCopyRepository itemCopyRepository)
        {
            _itemCopyRepository = itemCopyRepository;
        }

        public async Task<Result<int>> CreateLoanAsync(LoanRequestDto request, CancellationToken cancellationToken = default)
        {
            var copy = await _itemCopyRepository.GetByIdAsync(request.ItemCopyId, cancellationToken);
            
            if (copy == null)
            {
                return Result<int>.Fail("העותק לא נמצא.", 404);
            }

            if (copy.Status != CopyStatus.Available && (int)copy.Status != 0)
            {
                return Result<int>.Fail("העותק כבר מושאל או בתיקון.", 400);
            }

            copy.Status = CopyStatus.Loaned;
            
            var newLoan = new Loan
            {
                ItemCopyId = copy.Id,
                MemberId = request.MemberId,
                LoanDate = DateTime.UtcNow,
                Status = LoanStatus.Active
            };

            copy.Loans.Add(newLoan);

            try
            {
                await _itemCopyRepository.UpdateAsync(copy, cancellationToken);
                await _itemCopyRepository.SaveChangesAsync(cancellationToken);
                
                return Result<int>.Success(newLoan.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<int>.Conflict("העותק נתפס על ידי משתמש אחר ברגע זה ממש. נסה שוב.");
            }
        }
    }
}