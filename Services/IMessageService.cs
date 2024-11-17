using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IMessageService
    {
        Task<int> CreateMessage(MessageDto messageDto);
        Task<List<MessageDto>> GetAllMessagesByIssueId(int issueId);
    }
}