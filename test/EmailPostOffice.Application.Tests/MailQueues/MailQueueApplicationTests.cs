using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EmailPostOffice.MailQueues
{
    public class MailQueuesAppServiceTests : EmailPostOfficeApplicationTestBase
    {
        private readonly IMailQueuesAppService _mailQueuesAppService;
        private readonly IRepository<MailQueue, Guid> _mailQueueRepository;

        public MailQueuesAppServiceTests()
        {
            _mailQueuesAppService = GetRequiredService<IMailQueuesAppService>();
            _mailQueueRepository = GetRequiredService<IRepository<MailQueue, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _mailQueuesAppService.GetListAsync(new GetMailQueuesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("103bb4b0-94a9-4b45-9bc6-111e74420fef")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _mailQueuesAppService.GetAsync(Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new MailQueueCreateDto
            {
                Recipient = "4286fffa3cca4c67840f01@e903092607ef4d5f9222c1.com",
                RecipientName = "37c57b01da2e4681bbf23ca549dcd8838587d21a0e6c4caf88213f025feab1fc58446e49604c495a814775bf36df9b9a5f18",
                Sender = "7b422827b62a4a51b96740@3e84081d6e6d4682b180dc.com",
                SenderName = "93c66fb6d206492eb8b3c3383b7ceab6631a0b32ce9245e0a066b0ad746749a8f3c2a827505f447b872776067210a9dd082c",
                Subject = "e56638c4a265405a885943e9ad5798370c0f710cfac1494f9cec441bde6377ab3276032794ec4545989e6707389bd814155d40e70b964e4fac1838950a400dc1fb5f2ac2dee44cf4be840cecc40b949eacb39d45fd6f4f4fafdb46e94db01b9f85a536cba301403bb84f55633c5a1453d549b5eebe744adc8f59d1deeea1cd777c89396a75854170b18c8531367007f711f08ac30302460c8b74a3d60d7deed59457f3b190674edab86476f86c2ff2a8a4382b193bbc4bc8873a3e01c53224e13edf3a71ad454a9ba447ecb0af95905f397d4a64f9a24484b4a10c737e42555aad5eb97e27664f739bdc5f93aef05f7500350f2873394e469456",
                Content = "d1c1f46dc589488db1398271015d0e7",
                Retry = 1325163889,
                Success = true,
                Suspend = true,
                FreezeTime = new DateTime(2019, 1, 3)
            };

            // Act
            var serviceResult = await _mailQueuesAppService.CreateAsync(input);

            // Assert
            var result = await _mailQueueRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Recipient.ShouldBe("4286fffa3cca4c67840f01@e903092607ef4d5f9222c1.com");
            result.RecipientName.ShouldBe("37c57b01da2e4681bbf23ca549dcd8838587d21a0e6c4caf88213f025feab1fc58446e49604c495a814775bf36df9b9a5f18");
            result.Sender.ShouldBe("7b422827b62a4a51b96740@3e84081d6e6d4682b180dc.com");
            result.SenderName.ShouldBe("93c66fb6d206492eb8b3c3383b7ceab6631a0b32ce9245e0a066b0ad746749a8f3c2a827505f447b872776067210a9dd082c");
            result.Subject.ShouldBe("e56638c4a265405a885943e9ad5798370c0f710cfac1494f9cec441bde6377ab3276032794ec4545989e6707389bd814155d40e70b964e4fac1838950a400dc1fb5f2ac2dee44cf4be840cecc40b949eacb39d45fd6f4f4fafdb46e94db01b9f85a536cba301403bb84f55633c5a1453d549b5eebe744adc8f59d1deeea1cd777c89396a75854170b18c8531367007f711f08ac30302460c8b74a3d60d7deed59457f3b190674edab86476f86c2ff2a8a4382b193bbc4bc8873a3e01c53224e13edf3a71ad454a9ba447ecb0af95905f397d4a64f9a24484b4a10c737e42555aad5eb97e27664f739bdc5f93aef05f7500350f2873394e469456");
            result.Content.ShouldBe("d1c1f46dc589488db1398271015d0e7");
            result.Retry.ShouldBe(1325163889);
            result.Success.ShouldBe(true);
            result.Suspend.ShouldBe(true);
            result.FreezeTime.ShouldBe(new DateTime(2019, 1, 3));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new MailQueueUpdateDto()
            {
                Recipient = "2a63238baf88443d88653f@4b1ad30e4dd349f4b48362.com",
                RecipientName = "fd41c2b99394474fb98e4c6adb38d1c2b81546f6ddab49c5b07f90269df173409d9c12a9c9e841028887acbea178a7e4965b",
                Sender = "b9e577af9cfd480fa5e493@c01e5a1e238c4fbcb2d22d.com",
                SenderName = "f9bde448f0034b49abfade8cb11fb775844b3f175f334c4a89dce375eb5fe88e24437318b95541eb978baeff5546db92a802",
                Subject = "824d3da328e0458898598c61a448d823d799dfb0a3324722aade3edbb98791e166ffc3066fb34ba58afcebcb890dac747e0deb812f3f4ff6bcbec29f856d1a0b08fd7a70cd0b432ea4b83ee8ac77a78f565a78c7168d48068d7089b7d369b2c8fca8b4f6db124ca19c3c95cb97d85e51f9400bad90534feabebe0c572dbb0298c3f64f71a6124500a792a89057c336f1e5b1119313f94f5aa905a586e04b921e747f109e0ad8489cbf3bf95c856336d8f3415ea95ebd4fe38fe368421c2cf34df228ab93a7f246f1b8194b932b9f80ae545b782442be45188b7e1d60c0dae16563d424bf27824fe1a52fbd09c19b5f06ad69aa5fb15347259d88",
                Content = "ee52b781bbee47e",
                Retry = 1222767868,
                Success = true,
                Suspend = true,
                FreezeTime = new DateTime(2008, 8, 10)
            };

            // Act
            var serviceResult = await _mailQueuesAppService.UpdateAsync(Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6"), input);

            // Assert
            var result = await _mailQueueRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Recipient.ShouldBe("2a63238baf88443d88653f@4b1ad30e4dd349f4b48362.com");
            result.RecipientName.ShouldBe("fd41c2b99394474fb98e4c6adb38d1c2b81546f6ddab49c5b07f90269df173409d9c12a9c9e841028887acbea178a7e4965b");
            result.Sender.ShouldBe("b9e577af9cfd480fa5e493@c01e5a1e238c4fbcb2d22d.com");
            result.SenderName.ShouldBe("f9bde448f0034b49abfade8cb11fb775844b3f175f334c4a89dce375eb5fe88e24437318b95541eb978baeff5546db92a802");
            result.Subject.ShouldBe("824d3da328e0458898598c61a448d823d799dfb0a3324722aade3edbb98791e166ffc3066fb34ba58afcebcb890dac747e0deb812f3f4ff6bcbec29f856d1a0b08fd7a70cd0b432ea4b83ee8ac77a78f565a78c7168d48068d7089b7d369b2c8fca8b4f6db124ca19c3c95cb97d85e51f9400bad90534feabebe0c572dbb0298c3f64f71a6124500a792a89057c336f1e5b1119313f94f5aa905a586e04b921e747f109e0ad8489cbf3bf95c856336d8f3415ea95ebd4fe38fe368421c2cf34df228ab93a7f246f1b8194b932b9f80ae545b782442be45188b7e1d60c0dae16563d424bf27824fe1a52fbd09c19b5f06ad69aa5fb15347259d88");
            result.Content.ShouldBe("ee52b781bbee47e");
            result.Retry.ShouldBe(1222767868);
            result.Success.ShouldBe(true);
            result.Suspend.ShouldBe(true);
            result.FreezeTime.ShouldBe(new DateTime(2008, 8, 10));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _mailQueuesAppService.DeleteAsync(Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6"));

            // Assert
            var result = await _mailQueueRepository.FindAsync(c => c.Id == Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6"));

            result.ShouldBeNull();
        }
    }
}