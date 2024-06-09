namespace TaskManager.EmailManagementApi.Entities
{
    public class IncomingClientApp
    {
        public string ClientAppName { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string EmailManagementApiKey { get; set; } = default!;
        public string ConfirmUrl { get; set; } = default!;
        public string ClientRsaPublicKey { get; set; } = default!;
    }
}
