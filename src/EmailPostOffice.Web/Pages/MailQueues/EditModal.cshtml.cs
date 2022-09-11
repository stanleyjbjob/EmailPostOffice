using EmailPostOffice.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using EmailPostOffice.MailQueues;

namespace EmailPostOffice.Web.Pages.MailQueues
{
    public class EditModalModel : EmailPostOfficePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public MailQueueUpdateDto MailQueue { get; set; }

        private readonly IMailQueuesAppService _mailQueuesAppService;

        public EditModalModel(IMailQueuesAppService mailQueuesAppService)
        {
            _mailQueuesAppService = mailQueuesAppService;
        }

        public async Task OnGetAsync()
        {
            var mailQueue = await _mailQueuesAppService.GetAsync(Id);
            MailQueue = ObjectMapper.Map<MailQueueDto, MailQueueUpdateDto>(mailQueue);

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _mailQueuesAppService.UpdateAsync(Id, MailQueue);
            return NoContent();
        }
    }
}