using System.ComponentModel.DataAnnotations;
using EquipmentLoanSystem.Core.Enums;

namespace EquipmentLoanSystem.Core.Entities
{
    public class ItemCopy
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public string SerialNumber { get; set; } = string.Empty;
        public CopyStatus Status { get; set; } = CopyStatus.Available;

        [ConcurrencyCheck]
        public Guid Version { get; set; } = Guid.NewGuid();

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}