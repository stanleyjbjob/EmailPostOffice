using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EmailPostOffice.Mail
{
    public interface IMailAppService: IApplicationService
    {
        Task<string> SendAsync(EmailSendingArgs emailSendingArgs);
    }
}
