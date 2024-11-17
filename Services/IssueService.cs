using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class IssueService : IIssueService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;

        public IssueService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }

        // Pobieranie listy Issue po userId bez Messages
        public async Task<List<IssueDto>> GetIssuesByUserId(int userId)
        {
            try
            {
                var issues = await _houseContext.Issues
                    .Where(i => i.CreatorId == userId)
                    .Select(i => new IssueDto
                    {
                        Id = i.Id,
                        Title = i.Title,
                        HouseId = i.HouseId,
                        Priority = i.Priority,
                        ParticipantsName = i.ParticipantsName,
                        ParticipantsId = i.ParticipantsId,
                        Status = i.Status,
                        CreatorId = i.CreatorId,
                        CreationDate = i.CreationDate,
                        ViewedBy = i.ViewedBy
                    })
                    .ToListAsync()
                    ?? throw new Exception($"Wątki dla usera o Id = {userId} nie zostały znalezione.");

                return issues;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas pobierania wątków.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas pobierania wątków.", ex); }
        }

        // Pobieranie pojedynczego Issue po issueId razem z Messages
        public async Task<IssueDto> GetIssueById(int issueId)
        {
            try
            {
                var issue = await _houseContext.Issues
                    .Include(i => i.Messages)
                    .FirstOrDefaultAsync(i => i.Id == issueId)
                    ?? throw new Exception($"Wątek o Id = {issueId} nie został znaleziony.");

                return _mapper.Map<IssueDto>(issue);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas pobierania wątków.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas pobierania wątków.", ex); }
        }

        // Tworzenie nowego Issue na podstawie IssueDto
        public async Task<int> CreateIssue(IssueDto issueDto)
        {
            try
            {
                var issueEntity = _mapper.Map<Issue>(issueDto);
                await _houseContext.Issues.AddAsync(issueEntity);
                await _houseContext.SaveChangesAsync();

                return issueEntity.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas tworzenia wątku.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas tworzenia wątku.", ex); }

        }

        // Aktualizacja Issue na podstawie issueId i IssueDto
        public async Task<bool> UpdateIssue(int issueId, IssueDto issueDto)
        {
            try
            {
                var existingIssue = await _houseContext.Issues
                    .FirstOrDefaultAsync(i => i.Id == issueId)
                    ?? throw new Exception($"Wątek o Id = {issueId} nie został znaleziony.");

                _mapper.Map(issueDto, existingIssue);

                await _houseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas aktualizacji wątku.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas aktualizacji wątku.", ex); }

        }

        // Usuwanie wybranego Issue na podstawie issueId
        public async Task<bool> DeleteIssue(int issueId)
        {
            try
            {
                var issue = await _houseContext.Issues
                    .FirstOrDefaultAsync(i => i.Id == issueId);

                if (issue == null) return false;

                _houseContext.Issues.Remove(issue);
                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas usuwania usuwania.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas usuwania wątku.", ex); }
        }

    }
}
