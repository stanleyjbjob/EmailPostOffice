using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EmailPostOffice
{
    public class ConnectionService : ApplicationService
    {
        public ConnectionService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string GetConnectionString()
        {
            return Configuration.GetConnectionString("Default");
        }
    }
}
