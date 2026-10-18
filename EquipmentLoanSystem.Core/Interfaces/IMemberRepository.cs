namespace EquipmentLoanSystem.Core.Interfaces
{
    public interface IMemberRepository
    {
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
