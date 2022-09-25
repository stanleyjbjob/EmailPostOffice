using EmailPostOffice.Localization;
using EmailPostOffice.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
    [HttpPost("TestApi")]
    public async Task<TestResultDto> test132(string test)
    {
        var result = new TestResultDto();
        result.Result = test;
        return await Task.FromResult(result);

    }
  
}
