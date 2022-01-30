using CDN.Exceptions;
using CDN.Models;

namespace CDN.Services
{
    public class InternalConfiguration
    {
        private readonly ILogger<InternalConfiguration> _logger;
        private string _auditLogWebhookUrl;
        private string _jwtKey;

        public InternalConfiguration(ILogger<InternalConfiguration> logger)
        {
            _logger = logger;
            _auditLogWebhookUrl = string.Empty;
            _jwtKey = string.Empty;
        }

        public void Init()
        {
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (_jwtKey == null) throw new InvalidConfigurationException("JWT_KEY not found.");
        }

        public void ApplyAppSetting(AppSetting setting)
        {
            _auditLogWebhookUrl = setting.AuditWebhookUrl;
        }
        public string GetAuditLogWebhook()
        {
            return _auditLogWebhookUrl;
        }
    }
}
