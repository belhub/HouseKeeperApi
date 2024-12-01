using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface ITrasnsactionService
    {
        Task<int> CreateTransaction(TransactionDto transactionDto, int userId);
        Task<bool> DeleteTransaction(int transactionId);
        Task<List<TransactionDto>> GetAllTtransactionsFormUserId(int userId, DateTime startDate, DateTime endDate);
        Task<bool> UpdateTransaction(int transactionId, TransactionDto transactionDto);
    }
}