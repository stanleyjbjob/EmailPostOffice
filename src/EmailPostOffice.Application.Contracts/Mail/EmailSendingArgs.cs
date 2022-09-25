using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailPostOffice.Mail
{
    public class EmailSendingArgs
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<Guid> AttachmentList { get; set; }
        public DateTime? FreezeTime { get; set; }
        public Guid? MailQueueID { get; set; }
    }
}
