using Zropbox.Exceptions;
using Zropbox.Models;

namespace Zropbox.Services
{
    public class InternalConfiguration
    {
        private string _auditLogWebhookUrl;
        private string _jwtKey;
        private string _fileRootPath;
        private string _serviceBaseUrl;
        private ulong _maxFilesize;
        private int _defaultShareDurationHours;

        public InternalConfiguration()
        {
            _auditLogWebhookUrl = string.Empty;
            _jwtKey = string.Empty;
            _serviceBaseUrl = string.Empty;
            _maxFilesize = ulong.MaxValue;
            _defaultShareDurationHours = 24;
        }

        public void Init()
        {
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (_jwtKey == null) throw new InvalidConfigurationException("JWT_KEY not found.");
            _fileRootPath = Environment.GetEnvironmentVariable("ABSOLUTE_PATH_TO_FILE_UPLOAD");
            if (_fileRootPath == null) throw new InvalidConfigurationException("ABSOLUTE_PATH_TO_FILE_UPLOAD not found.");
            _serviceBaseUrl = Environment.GetEnvironmentVariable("META_SERVICE_BASE_URL");
            if (_serviceBaseUrl == null) throw new InvalidConfigurationException("META_SERVICE_BASE_URL not found.");
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

        public ulong GetMaxFilesize()
        {
            return _maxFilesize;
        }

        public int GetDefaultShareDurationHours()
        {
            return _defaultShareDurationHours;
        }
    }
}
