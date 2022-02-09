using Zropbox.Exceptions;
using Zropbox.Models;

namespace Zropbox.Services
{
    public class InternalConfiguration
    {
        private string _auditLogWebhookUrl = string.Empty;
        private string _jwtKey = string.Empty;
        private string _fileRootPath = string.Empty;
        private string _serviceBaseUrl = string.Empty;
        private ulong _maxFilesize = ulong.MaxValue;
        private int _defaultShareDurationHours = 24;
        private int _loginDurationHours = 24;

        public InternalConfiguration() { }

        public void Init()
        {
            string? value;
            if (string.IsNullOrEmpty(value = Environment.GetEnvironmentVariable("JWT_KEY")))
            {
                throw new InvalidConfigurationException("JWT_KEY not found.");
            }
            _jwtKey = value;

            if (string.IsNullOrEmpty(value = Environment.GetEnvironmentVariable("ABSOLUTE_PATH_TO_FILE_UPLOAD")))
            {
                throw new InvalidConfigurationException("ABSOLUTE_PATH_TO_FILE_UPLOAD not found.");
            }
            _fileRootPath = value;

            if (string.IsNullOrEmpty(value = Environment.GetEnvironmentVariable("META_SERVICE_BASE_URL")))
            {
                throw new InvalidConfigurationException("META_SERVICE_BASE_URL not found.");
            }
            _serviceBaseUrl = value;

            if (!int.TryParse(Environment.GetEnvironmentVariable("LOGIN_DURATION"), out _loginDurationHours))
            {
                _loginDurationHours = 24;
            }
        }

        public void ApplyAppSetting(AppSetting setting)
        {
            _auditLogWebhookUrl = setting.AuditWebhookUrl;
            _maxFilesize = setting.MaxFilesize;
            _defaultShareDurationHours= setting.DefaultShareDurationHours;
        }

        public string GetAuditLogWebhook()
        {
            return _auditLogWebhookUrl;
        }

        public string GetJwtKey()
        {
            return _jwtKey;
        }

        public string GetFileRootPath()
        {
            return _fileRootPath;
        }

        public string GetServiceBaseUrl()
        {
            return _serviceBaseUrl;
        }

        public long GetMaxFilesize()
        {
            return (long)_maxFilesize;
        }

        public int GetDefaultShareDurationHours()
        {
            return _defaultShareDurationHours;
        }

        public int GetLoginDurationHours()
        {
            return _loginDurationHours;
        }
    }
}
