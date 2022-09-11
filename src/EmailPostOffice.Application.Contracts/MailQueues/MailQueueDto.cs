using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EmailPostOffice.MailQueues
{
    public class MailQueueDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int Retry { get; set; }
        public bool Success { get; set; }
        public bool Suspend { get; set; }
        public DateTime FreezeTime { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}