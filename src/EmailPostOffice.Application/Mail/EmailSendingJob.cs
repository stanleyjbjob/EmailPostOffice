//using AutoMapper.Internal.Mappers;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using EmailPostOffice.MailQueues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.ObjectMapping;

namespace EmailPostOffice.Mail
{
    public class EmailSendingJob
         : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly IMailQueueRepository _mailQueueRepository;
        private readonly IMapper _mapper;

        public EmailSendingJob(IEmailSender emailSender, IMailQueueRepository mailQueueRepository)
        {
            _emailSender = emailSender;
            _mailQueueRepository = mailQueueRepository;
        }
        public EmailSendingJob(IEmailSender emailSender, IMailQueueRepository mailQueueRepository, IMapper mapper)
        {
            _emailSender = emailSender;
            _mailQueueRepository = mailQueueRepository;
            _mapper = mapper;
        }

        public override async Task ExecuteAsync(EmailSendingArgs args)
        {
            await _emailSender.SendAsync(
                args.EmailAddress,
                args.Subject,
                args.Body
            );

            var mail = await _mailQueueRepository.GetAsync(args.MailQueueID);
            mail.Success = true;

            await _mailQueueRepository.UpdateAsync(mail);
        }
    }
}
