using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace EmailPostOffice.Pages;

public class Index_Tests : EmailPostOfficeWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
