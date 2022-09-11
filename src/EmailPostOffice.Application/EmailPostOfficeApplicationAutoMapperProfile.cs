using System;
using EmailPostOffice.Shared;
using Volo.Abp.AutoMapper;
using EmailPostOffice.MailQueues;
using AutoMapper;

namespace EmailPostOffice;

public class EmailPostOfficeApplicationAutoMapperProfile : Profile
{
    public EmailPostOfficeApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<MailQueue, MailQueueDto>();
    }
}