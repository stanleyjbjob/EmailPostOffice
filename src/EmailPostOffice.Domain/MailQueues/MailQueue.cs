using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace EmailPostOffice.MailQueues
{
    public class MailQueue : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string Recipient { get; set; }

        [CanBeNull]
        public virtual string RecipientName { get; set; }

        [NotNull]
        public virtual string Sender { get; set; }

        [CanBeNull]
        public virtual string SenderName { get; set; }

        [NotNull]
        public virtual string Subject { get; set; }

        [NotNull]
        public virtual string Content { get; set; }

        public virtual int Retry { get; set; }

        public virtual bool Success { get; set; }

        public virtual bool Suspend { get; set; }

        public virtual DateTime FreezeTime { get; set; }

        public MailQueue()
        {

        }

        public MailQueue(Guid id, string recipient, string recipientName, string sender, string senderName, string subject, string content, int retry, bool success, bool suspend, DateTime freezeTime)
        {

            Id = id;
            Check.NotNull(recipient, nameof(recipient));
            Check.Length(recipient, nameof(recipient), MailQueueConsts.RecipientMaxLength, 0);
            Check.Length(recipientName, nameof(recipientName), MailQueueConsts.RecipientNameMaxLength, 0);
            Check.NotNull(sender, nameof(sender));
            Check.Length(sender, nameof(sender), MailQueueConsts.SenderMaxLength, 0);
            Check.Length(senderName, nameof(senderName), MailQueueConsts.SenderNameMaxLength, 0);
            Check.NotNull(subject, nameof(subject));
            Check.Length(subject, nameof(subject), MailQueueConsts.SubjectMaxLength, 0);
            Check.NotNull(content, nameof(content));
            Recipient = recipient;
            RecipientName = recipientName;
            Sender = sender;
            SenderName = senderName;
            Subject = subject;
            Content = content;
            Retry = retry;
            Success = success;
            Suspend = suspend;
            FreezeTime = freezeTime;
        }

    }
}