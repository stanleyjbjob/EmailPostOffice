using EmailPostOffice.Localization;
using EmailPostOffice.Mail;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace EmailPostOffice.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestJbhrController : Controller
{
    //private IMailAppService _mailAppService;

    public TestJbhrController()
    {
        //_mailAppService = mailAppService;
    }
    [HttpPost("test132")]
    public async Task<string> test132(string test)
    {
       return "OK";
       
    }
}
