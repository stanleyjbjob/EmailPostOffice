using EmailPostOffice.MailQueues;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;

namespace EmailPostOffice.Mail
{
    public class MailAppService : ApplicationService, IMailAppService
    {
        private IBackgroundJobManager _backgroundJobManager;
        private IMailQueuesAppService _mailQueuesAppService;

        public MailAppService(IBackgroundJobManager backgroundJobManager, IMailQueuesAppService mailQueuesAppService)
        {
            _backgroundJobManager = backgroundJobManager;
            _mailQueuesAppService = mailQueuesAppService;
        }
        public async Task<IAsyncResult> SendAsync(EmailSendingArgs emailSendingArgs)
        {
            var mail = new MailQueueCreateDto
            {
                Content = emailSendingArgs.Body,
                FreezeTime = DateTime.Now,
                Recipient = emailSendingArgs.EmailAddress,
                Retry = 0,
                RecipientName = "",
                Sender = emailSendingArgs.EmailAddress,
                SenderName = "",
                Subject = emailSendingArgs.Subject,
                Success = false,
                Suspend = false,
            };
            var result = await _mailQueuesAppService.CreateAsync(mail);

            emailSendingArgs.MailQueueID = result.Id;
            var result1 = await _backgroundJobManager.EnqueueAsync(emailSendingArgs);
            return Task.FromResult(result1);    
        }
    }
}
