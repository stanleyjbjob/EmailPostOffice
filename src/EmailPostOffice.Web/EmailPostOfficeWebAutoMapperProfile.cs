using Volo.Abp.AutoMapper;
using EmailPostOffice.MailQueues;
using AutoMapper;

namespace EmailPostOffice.Web;

public class EmailPostOfficeWebAutoMapperProfile : Profile
{
    public EmailPostOfficeWebAutoMapperProfile()
    {
        //Define your object mappings here, for the Web project

        CreateMap<MailQueueDto, MailQueueUpdateDto>();
    }
}