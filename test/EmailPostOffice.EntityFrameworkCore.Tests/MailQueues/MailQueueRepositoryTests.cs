using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using EmailPostOffice.MailQueues;
using EmailPostOffice.EntityFrameworkCore;
using Xunit;

namespace EmailPostOffice.MailQueues
{
    public class MailQueueRepositoryTests : EmailPostOfficeEntityFrameworkCoreTestBase
    {
        private readonly IMailQueueRepository _mailQueueRepository;

        public MailQueueRepositoryTests()
        {
            _mailQueueRepository = GetRequiredService<IMailQueueRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _mailQueueRepository.GetListAsync(
                    recipient: "54bb7934dc13498cbfa6c6@97057004d92c404f8b3591.com",
                    recipientName: "04e162d5a40d4735a84856dec8bec8aff43934e34f4c4f6aa56a2a00e35eeb55db061b36891c461c8afeb0b6e80e3a9effbf",
                    sender: "47cb43ed62e24a319884b4@9b3fb408bfdc4557b1663c.com",
                    senderName: "3b62648ed71e40639abad90e1c64e6b91f58a44eee1d47b18d5046a2b9c68bba761a790bf3e44420966dc995acd0e6f0a570",
                    subject: "306ddbc6dd6340f3a4e4550e08e3a122726325b225164569a1cbdabe20b05324f25cbde60e2e4637a0e8670f101559330137bd53235d42e7ac23ddf3d9f4964246af44ff3013488b87935fcc8b3d2201129de157edd44aa78c703243d63cd3362dbdec03849546a491a3dab17a7ecc0e843c6a22406c466e85999ba0dc7898a6449fa39306c34f25b280fcbe1dd25347efac5fec9c7443f9af7532cd31ffa8a6a8cfd11f2fb742ee9e2800a26f1f83b34249a3daf95e4d279c8b09bd0025d8dce1fcefbbec9d43ce97448c605bd159ea74899030151a496782b4eef1459622a0f6961881257c4aba8d1e356300dc591624c6fbf00e5c431686b8",
                    content: "86bcf01e6155480d94b6bc3bb051af70400e",
                    success: true,
                    suspend: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("fa57bb60-3af5-42e7-8926-2e4b6257cff6"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _mailQueueRepository.GetCountAsync(
                    recipient: "ec1dbfe5b66b4f53a4d3a6@8354c6e060134582a9d6b6.com",
                    recipientName: "12eb85ea70b047dea9a4b955b2c844e3bda25eff9c8d441db78b3c36674d48a966398e9712e24ff29ad85a4c027057c84681",
                    sender: "dd531e490e3d410ca233bb@8be95bfb7b214cbca89f69.com",
                    senderName: "e134d1cbbc874a2ea2df978de2f17e6188a8480c0efb404f84a609465f568a75b7fb2f7e12d340b8ba3b43ea3d62050ae27f",
                    subject: "698138ce85b7465a9127f1079770d4305035a30e81724e729476c3c27bb7ddf1ded52d009509411cb6240bee60289316749d4c692d9c40c0a22c0b7bc1c35ea8ce1239800e894811beafa095923cb65c919c44053ecd4e7db63f9460b09c6823068e4bb56d9e400690d30ce0f273a41fffefdeba7e7343a5a89b4d716505521604f6ea5e6e954e4abec47e8270dbd51f357356ae72a1447cb8842ecaf9d41ab8792c7ee943c84bfd8a994c486afb03e736b75758bf0e4a009ae67ba64b80ae42ba1afbdf9bfd4663b947cdb340ca8a63a66ee63e048b494492eed64f879150d499d08aa699b54feab98939529fe97ea29fe122eb46cf495aa0fe",
                    content: "050589667cd34cf98a608b7f0079ea4c74158",
                    success: true,
                    suspend: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}