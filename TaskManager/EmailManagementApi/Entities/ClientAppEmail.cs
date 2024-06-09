namespace TaskManager.EmailManagementApi.Entities
{
    public class ClientAppEmail
    {
        #region encrypted
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
        public List<EmailRecipient> Recipients { get; set; }
        public string EmailManagementApiKey { get; set; } = default!;
        public string? UserSenderEmailAddress { get; set; }
        #endregion
        public string ConfirmUrl { get; set; } = default!;
        public List<IncomingEmailAttachment> Attachments { get; set; }

        public ClientAppEmail()
        {

        }
    }
}
