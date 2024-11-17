using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IIssueService
    {
        Task<int> CreateIssue(IssueDto issueId);
        Task<bool> DeleteIssue(int issueId);
        Task<IssueDto> GetIssueById(int issueId);
        Task<List<IssueDto>> GetIssuesByUserId(int userId);
        Task<bool> UpdateIssue(int issueId, IssueDto issueDto);
    }
}