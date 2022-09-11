using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace EmailPostOffice.MailQueues
{
    public class MailQueueManager : DomainService
    {
        private readonly IMailQueueRepository _mailQueueRepository;

        public MailQueueManager(IMailQueueRepository mailQueueRepository)
        {
            _mailQueueRepository = mailQueueRepository;
        }

        public async Task<MailQueue> CreateAsync(
        string recipient, string recipientName, string sender, string senderName, string subject, string content, int retry, bool success, bool suspend, DateTime freezeTime)
        {
            var mailQueue = new MailQueue(
             GuidGenerator.Create(),
             recipient, recipientName, sender, senderName, subject, content, retry, success, suspend, freezeTime
             );

            return await _mailQueueRepository.InsertAsync(mailQueue);
        }

        public async Task<MailQueue> UpdateAsync(
            Guid id,
            string recipient, string recipientName, string sender, string senderName, string subject, string content, int retry, bool success, bool suspend, DateTime freezeTime, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _mailQueueRepository.GetQueryableAsync();
            var query = queryable.Where(x => x.Id == id);

            var mailQueue = await AsyncExecuter.FirstOrDefaultAsync(query);

            mailQueue.Recipient = recipient;
            mailQueue.RecipientName = recipientName;
            mailQueue.Sender = sender;
            mailQueue.SenderName = senderName;
            mailQueue.Subject = subject;
            mailQueue.Content = content;
            mailQueue.Retry = retry;
            mailQueue.Success = success;
            mailQueue.Suspend = suspend;
            mailQueue.FreezeTime = freezeTime;

            mailQueue.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _mailQueueRepository.UpdateAsync(mailQueue);
        }

    }
}