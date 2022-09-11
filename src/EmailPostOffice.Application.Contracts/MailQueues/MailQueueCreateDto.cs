using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EmailPostOffice.MailQueues
{
    public class MailQueueCreateDto
    {
        [Required]
        [EmailAddress]
        [StringLength(MailQueueConsts.RecipientMaxLength)]
        public string Recipient { get; set; }
        [StringLength(MailQueueConsts.RecipientNameMaxLength)]
        public string RecipientName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(MailQueueConsts.SenderMaxLength)]
        public string Sender { get; set; }
        [StringLength(MailQueueConsts.SenderNameMaxLength)]
        public string SenderName { get; set; }
        [Required]
        [StringLength(MailQueueConsts.SubjectMaxLength)]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        public int Retry { get; set; }
        public bool Success { get; set; }
        public bool Suspend { get; set; }
        public DateTime FreezeTime { get; set; }
    }
}