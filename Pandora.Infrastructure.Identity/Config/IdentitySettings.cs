/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.Config
{
    using Pandora.NetStdLibrary.Base.Common;

    public class IdentitySettings : BaseSettings<IdentitySettings>
    {
        public string DatabaseUrl { get; set; }
        public string LoggerServerUrl { get; set; }
        public string JwtSecretKey { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtValidAudience { get; set; }
        public ExternalIdp ExternalIdp { get; set; }
        public EmailSender EmailSender { get; set; }
    }

    public class ExternalIdp
    {
        public string SocialFacebookAppId { get; set; }
        public string SocialFacebookAppSecret { get; set; }
        public string SocialGoogleClientId { get; set; }
        public string SocialGoogleAppSecret { get; set; }
    }

    public class EmailSender
    {
        public string SendGridUser { get; set; }
        public string SendGridApiKey { get; set; }
        public string SendGridFromName { get; set; }
        public string SendGridSubject { get; set; }
        public string SendGridFromEmail { get; set; }
    }

}
