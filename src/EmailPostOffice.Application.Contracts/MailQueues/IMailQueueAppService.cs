using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EmailPostOffice.MailQueues
{
    public interface IMailQueuesAppService : IApplicationService
    {
        Task<PagedResultDto<MailQueueDto>> GetListAsync(GetMailQueuesInput input);

        Task<MailQueueDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<MailQueueDto> CreateAsync(MailQueueCreateDto input);

        Task<MailQueueDto> UpdateAsync(Guid id, MailQueueUpdateDto input);
    }
}