using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using EmailPostOffice.MailQueues;
using EmailPostOffice.Shared;

namespace EmailPostOffice.Web.Pages.MailQueues
{
    public class IndexModel : AbpPageModel
    {
        public string RecipientFilter { get; set; }
        public string RecipientNameFilter { get; set; }
        public string SenderFilter { get; set; }
        public string SenderNameFilter { get; set; }
        public string SubjectFilter { get; set; }
        public string ContentFilter { get; set; }
        public int? RetryFilterMin { get; set; }

        public int? RetryFilterMax { get; set; }
        [SelectItems(nameof(SuccessBoolFilterItems))]
        public string SuccessFilter { get; set; }

        public List<SelectListItem> SuccessBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(SuspendBoolFilterItems))]
        public string SuspendFilter { get; set; }

        public List<SelectListItem> SuspendBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        public DateTime? FreezeTimeFilterMin { get; set; }

        public DateTime? FreezeTimeFilterMax { get; set; }

        private readonly IMailQueuesAppService _mailQueuesAppService;

        public IndexModel(IMailQueuesAppService mailQueuesAppService)
        {
            _mailQueuesAppService = mailQueuesAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}