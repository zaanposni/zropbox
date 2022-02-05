namespace Zropbox.Models
{
    public class AppSetting
    {
        public int Id { get; set; }
        public string AuditWebhookUrl { get; set; }
        public ulong MaxFilesize { get; set; }
        public int DefaultShareDurationHours { get; set; }

        public static AppSetting CreateDefault()
        {
            return new AppSetting()
            {
                AuditWebhookUrl = string.Empty,
                MaxFilesize = ulong.MaxValue,
                DefaultShareDurationHours = 24
            };
        }
    }
}
