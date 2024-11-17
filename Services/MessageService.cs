using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;
        public MessageService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }

        // Pobieranie wszystkich elementów Equipment na podstawie issueId
        public async Task<List<MessageDto>> GetAllMessagesByIssueId(int issueId)
        {
            try
            {
                var messages = await _houseContext.Messages
                    .Where(m => m.IssueId == issueId)
                    .OrderBy(m => m.SendDate)
                    .ToListAsync()
                    ?? throw new Exception($"Wiadomości dla wątku o Id = {issueId} nie zostały znalezione.");

                return _mapper.Map<List<MessageDto>>(messages);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas pobierania wiadomości.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas pobierania wiadomości.", ex); }
        }

        public async Task<int> CreateMessage(MessageDto messageDto)
        {
            try
            {
                var messageEntity = _mapper.Map<Message>(messageDto);

                await _houseContext.Messages.AddAsync(messageEntity);
                await _houseContext.SaveChangesAsync();

                return messageEntity.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas pobierania wiadomości.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas pobierania wiadomości.", ex); }
        }
    }
}
