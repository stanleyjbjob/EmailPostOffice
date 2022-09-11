using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EmailPostOffice.Permissions;
using EmailPostOffice.MailQueues;

namespace EmailPostOffice.MailQueues
{

    [Authorize(EmailPostOfficePermissions.MailQueues.Default)]
    public class MailQueuesAppService : ApplicationService, IMailQueuesAppService
    {
        private readonly IMailQueueRepository _mailQueueRepository;
        private readonly MailQueueManager _mailQueueManager;

        public MailQueuesAppService(IMailQueueRepository mailQueueRepository, MailQueueManager mailQueueManager)
        {
            _mailQueueRepository = mailQueueRepository;
            _mailQueueManager = mailQueueManager;
        }

        public virtual async Task<PagedResultDto<MailQueueDto>> GetListAsync(GetMailQueuesInput input)
        {
            var totalCount = await _mailQueueRepository.GetCountAsync(input.FilterText, input.Recipient, input.RecipientName, input.Sender, input.SenderName, input.Subject, input.Content, input.RetryMin, input.RetryMax, input.Success, input.Suspend, input.FreezeTimeMin, input.FreezeTimeMax);
            var items = await _mailQueueRepository.GetListAsync(input.FilterText, input.Recipient, input.RecipientName, input.Sender, input.SenderName, input.Subject, input.Content, input.RetryMin, input.RetryMax, input.Success, input.Suspend, input.FreezeTimeMin, input.FreezeTimeMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<MailQueueDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<MailQueue>, List<MailQueueDto>>(items)
            };
        }

        public virtual async Task<MailQueueDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<MailQueue, MailQueueDto>(await _mailQueueRepository.GetAsync(id));
        }

        [Authorize(EmailPostOfficePermissions.MailQueues.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _mailQueueRepository.DeleteAsync(id);
        }

        [Authorize(EmailPostOfficePermissions.MailQueues.Create)]
        public virtual async Task<MailQueueDto> CreateAsync(MailQueueCreateDto input)
        {

            var mailQueue = await _mailQueueManager.CreateAsync(
            input.Recipient, input.RecipientName, input.Sender, input.SenderName, input.Subject, input.Content, input.Retry, input.Success, input.Suspend, input.FreezeTime
            );

            return ObjectMapper.Map<MailQueue, MailQueueDto>(mailQueue);
        }

        [Authorize(EmailPostOfficePermissions.MailQueues.Edit)]
        public virtual async Task<MailQueueDto> UpdateAsync(Guid id, MailQueueUpdateDto input)
        {

            var mailQueue = await _mailQueueManager.UpdateAsync(
            id,
            input.Recipient, input.RecipientName, input.Sender, input.SenderName, input.Subject, input.Content, input.Retry, input.Success, input.Suspend, input.FreezeTime, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<MailQueue, MailQueueDto>(mailQueue);
        }
    }
}