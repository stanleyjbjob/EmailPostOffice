namespace EmailPostOffice.MailQueues
{
    public static class MailQueueConsts
    {
        private const string DefaultSorting = "{0}Recipient asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "MailQueue." : string.Empty);
        }

        public const int RecipientMaxLength = 50;
        public const int RecipientNameMaxLength = 100;
        public const int SenderMaxLength = 50;
        public const int SenderNameMaxLength = 100;
        public const int SubjectMaxLength = 500;
    }
}