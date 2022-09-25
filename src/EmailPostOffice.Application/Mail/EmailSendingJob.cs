//using AutoMapper.Internal.Mappers;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using EmailPostOffice.MailQueues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Smtp;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace EmailPostOffice.Mail
{
    public class EmailSendingJob
         : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly IMailQueueRepository _mailQueueRepository;

        public EmailSendingJob(IEmailSender emailSender, IMailQueueRepository mailQueueRepository)
        {
            _emailSender = emailSender;
            _mailQueueRepository = mailQueueRepository;
        }

        public override async Task ExecuteAsync(EmailSendingArgs args)
        {
            var mailQueue =await _mailQueueRepository.GetAsync(p=>p.Id== args.MailQueueID);

            var msg = new MailMessage();
            msg.Subject = args.Subject;
            msg.Body = args.Body;
            msg.Sender = new MailAddress(mailQueue.Sender,mailQueue.SenderName);
            msg.To.Add(new MailAddress(mailQueue.Recipient, mailQueue.Recipient));
            //await _smtpEmailSender.SendAsync(msg);
            await _emailSender.SendAsync(msg, true);

            var mail = await _mailQueueRepository.GetAsync(args.MailQueueID.Value);
            mail.Success = true;

            await _mailQueueRepository.UpdateAsync(mail);
        }
    }
}
