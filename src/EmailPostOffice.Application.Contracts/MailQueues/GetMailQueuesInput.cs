using Volo.Abp.Application.Dtos;
using System;

namespace EmailPostOffice.MailQueues
{
    public class GetMailQueuesInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int? RetryMin { get; set; }
        public int? RetryMax { get; set; }
        public bool? Success { get; set; }
        public bool? Suspend { get; set; }
        public DateTime? FreezeTimeMin { get; set; }
        public DateTime? FreezeTimeMax { get; set; }

        public GetMailQueuesInput()
        {

        }
    }
}