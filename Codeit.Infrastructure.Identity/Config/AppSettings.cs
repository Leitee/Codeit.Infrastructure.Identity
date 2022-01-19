/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Config
{
    using Codeit.NetStdLibrary.Base.Common;
    using Codeit.NetStdLibrary.Base.DataAccess;

    public class AppSettings : BaseSettings<AppSettings>
    {
        public bool? UseLoggerServer { get; set; }
        public string LoggerServerUrl { get; set; }
        public string JwtSecretKey { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtValidAudience { get; set; }
        public DALSettings DalSection { get; set; }
        public ExternalIdp ExternalProviderSection { get; set; }
        public EmailSender EmailSection { get; set; }
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
