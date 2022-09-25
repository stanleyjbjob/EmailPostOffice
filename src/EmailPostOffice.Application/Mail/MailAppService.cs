using EmailPostOffice.MailQueues;
using EmailPostOffice.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Settings;

namespace EmailPostOffice.Mail
{
    [Authorize(EmailPostOfficePermissions.MailQueues.Default)]
    public class MailAppService : ApplicationService, IMailAppService
    {
        private IBackgroundJobManager _backgroundJobManager;
        private IMailQueuesAppService _mailQueuesAppService;
        private readonly ISettingProvider settingProvider;

        public MailAppService(IBackgroundJobManager backgroundJobManager, IMailQueuesAppService mailQueuesAppService, ISettingProvider settingProvider)
        {
            _backgroundJobManager = backgroundJobManager;
            _mailQueuesAppService = mailQueuesAppService;
            this.settingProvider = settingProvider;
        }
        [Authorize(EmailPostOfficePermissions.MailQueues.Default)]
        public async Task<MailQueueDto> SendAsync(EmailSendingArgs emailSendingArgs)
        {
            var sender = await settingProvider.GetOrNullAsync("Abp.Mailing.DefaultFromAddress");
            var senderName = await settingProvider.GetOrNullAsync("Abp.Mailing.DefaultFromDisplayName");
            var mail = new MailQueueCreateDto
            {
                Content = emailSendingArgs.Body,
                FreezeTime = DateTime.Now,//不用
                Recipient = emailSendingArgs.EmailAddress,
                Retry = 0,//不用
                RecipientName = emailSendingArgs.Name,
                Sender = sender,
                SenderName = senderName,
                Subject = emailSendingArgs.Subject,
                Success = false,
                Suspend = false,
            };
            var result = await _mailQueuesAppService.CreateAsync(mail);

            emailSendingArgs.MailQueueID = result.Id;

            if (emailSendingArgs.FreezeTime is null)
                await _backgroundJobManager.EnqueueAsync(emailSendingArgs);
            else
            {
                if (emailSendingArgs.FreezeTime > DateTime.Now)
                {
                    var timeSpan = emailSendingArgs.FreezeTime - DateTime.Now;
                    await _backgroundJobManager.EnqueueAsync(emailSendingArgs, delay: timeSpan);
                }
                else
                    await _backgroundJobManager.EnqueueAsync(emailSendingArgs);
            }
            return await Task.FromResult(result);
        }
    }
}
