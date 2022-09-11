using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EmailPostOffice.MailQueues
{
    public interface IMailQueueRepository : IRepository<MailQueue, Guid>
    {
        Task<List<MailQueue>> GetListAsync(
            string filterText = null,
            string recipient = null,
            string recipientName = null,
            string sender = null,
            string senderName = null,
            string subject = null,
            string content = null,
            int? retryMin = null,
            int? retryMax = null,
            bool? success = null,
            bool? suspend = null,
            DateTime? freezeTimeMin = null,
            DateTime? freezeTimeMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string recipient = null,
            string recipientName = null,
            string sender = null,
            string senderName = null,
            string subject = null,
            string content = null,
            int? retryMin = null,
            int? retryMax = null,
            bool? success = null,
            bool? suspend = null,
            DateTime? freezeTimeMin = null,
            DateTime? freezeTimeMax = null,
            CancellationToken cancellationToken = default);
    }
}