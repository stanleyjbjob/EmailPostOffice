using EmailPostOffice.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmailPostOffice.MailQueues;

namespace EmailPostOffice.Web.Pages.MailQueues
{
    public class CreateModalModel : EmailPostOfficePageModel
    {
        [BindProperty]
        public MailQueueCreateDto MailQueue { get; set; }

        private readonly IMailQueuesAppService _mailQueuesAppService;

        public CreateModalModel(IMailQueuesAppService mailQueuesAppService)
        {
            _mailQueuesAppService = mailQueuesAppService;
        }

        public async Task OnGetAsync()
        {
            MailQueue = new MailQueueCreateDto();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _mailQueuesAppService.CreateAsync(MailQueue);
            return NoContent();
        }
    }
}