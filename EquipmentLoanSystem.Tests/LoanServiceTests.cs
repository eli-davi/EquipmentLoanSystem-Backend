using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Core.Entities;
using EquipmentLoanSystem.Core.Enums;
using EquipmentLoanSystem.Core.Interfaces;
using EquipmentLoanSystem.Service.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EquipmentLoanSystem.Tests
{
    public class LoanServiceTests
    {
        [Fact]
        public async Task CreateLoanAsync_WhenCopyIsAvailable_ReturnsSuccess()
        {
            // Arrange
            var mockRepo = new Mock<IItemCopyRepository>();
            var itemCopy = new ItemCopy { Id = 1, Status = CopyStatus.Available };
            var request = new LoanRequestDto { ItemCopyId = 1, MemberId = 1 };

            mockRepo.Setup(repo => repo.GetByIdAsync(request.ItemCopyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(itemCopy);

            var service = new LoanService(mockRepo.Object);

            // Act
            var result = await service.CreateLoanAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(200, result.ErrorCode);
            mockRepo.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateLoanAsync_WhenCopyHasZeroStatus_ReturnsSuccess()
        {
            var mockRepo = new Mock<IItemCopyRepository>();
            var itemCopy = new ItemCopy { Id = 1, Status = (CopyStatus)0 };
            var request = new LoanRequestDto { ItemCopyId = 1, MemberId = 1 };

            mockRepo.Setup(repo => repo.GetByIdAsync(request.ItemCopyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(itemCopy);

            var service = new LoanService(mockRepo.Object);

            var result = await service.CreateLoanAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(CopyStatus.Loaned, itemCopy.Status);
            mockRepo.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateLoanAsync_WhenConcurrencyConflictOccurs_Returns409Conflict()
        {
            // Arrange
            var mockRepo = new Mock<IItemCopyRepository>();
            var itemCopy = new ItemCopy { Id = 1, Status = CopyStatus.Available };
            var request = new LoanRequestDto { ItemCopyId = 1, MemberId = 1 };

            mockRepo.Setup(repo => repo.GetByIdAsync(request.ItemCopyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(itemCopy);

            mockRepo.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new DbUpdateConcurrencyException());

            var service = new LoanService(mockRepo.Object);

            // Act
            var result = await service.CreateLoanAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(409, result.ErrorCode);
            Assert.Contains("נתפס על ידי משתמש אחר", result.ErrorMessage);
        }
    }
}