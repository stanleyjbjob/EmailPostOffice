using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using EmailPostOffice.EntityFrameworkCore;

namespace EmailPostOffice.MailQueues
{
    public class EfCoreMailQueueRepository : EfCoreRepository<EmailPostOfficeDbContext, MailQueue, Guid>, IMailQueueRepository
    {
        public EfCoreMailQueueRepository(IDbContextProvider<EmailPostOfficeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<MailQueue>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, recipient, recipientName, sender, senderName, subject, content, retryMin, retryMax, success, suspend, freezeTimeMin, freezeTimeMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? MailQueueConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, recipient, recipientName, sender, senderName, subject, content, retryMin, retryMax, success, suspend, freezeTimeMin, freezeTimeMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<MailQueue> ApplyFilter(
            IQueryable<MailQueue> query,
            string filterText,
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
            DateTime? freezeTimeMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Recipient.Contains(filterText) || e.RecipientName.Contains(filterText) || e.Sender.Contains(filterText) || e.SenderName.Contains(filterText) || e.Subject.Contains(filterText) || e.Content.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(recipient), e => e.Recipient.Contains(recipient))
                    .WhereIf(!string.IsNullOrWhiteSpace(recipientName), e => e.RecipientName.Contains(recipientName))
                    .WhereIf(!string.IsNullOrWhiteSpace(sender), e => e.Sender.Contains(sender))
                    .WhereIf(!string.IsNullOrWhiteSpace(senderName), e => e.SenderName.Contains(senderName))
                    .WhereIf(!string.IsNullOrWhiteSpace(subject), e => e.Subject.Contains(subject))
                    .WhereIf(!string.IsNullOrWhiteSpace(content), e => e.Content.Contains(content))
                    .WhereIf(retryMin.HasValue, e => e.Retry >= retryMin.Value)
                    .WhereIf(retryMax.HasValue, e => e.Retry <= retryMax.Value)
                    .WhereIf(success.HasValue, e => e.Success == success)
                    .WhereIf(suspend.HasValue, e => e.Suspend == suspend)
                    .WhereIf(freezeTimeMin.HasValue, e => e.FreezeTime >= freezeTimeMin.Value)
                    .WhereIf(freezeTimeMax.HasValue, e => e.FreezeTime <= freezeTimeMax.Value);
        }
    }
}