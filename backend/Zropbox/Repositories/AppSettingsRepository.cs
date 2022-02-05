using Microsoft.EntityFrameworkCore;
using Zropbox.Models;

namespace Zropbox.Repositories
{
    public class AppSettingsRepository : BaseRepository<AppSettingsRepository>
    {
        private AppSettingsRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public static AppSettingsRepository CreateDefault(IServiceProvider serviceProvider)
        {
            return new AppSettingsRepository(serviceProvider);
        }

        public async Task<AppSetting> GetAppSettings()
        {
            AppSetting settings = await Context.AppSettings.AsQueryable().FirstOrDefaultAsync();
            if (settings == null)
            {
                return AppSetting.CreateDefault();
            }
            return settings;
        }

        public async Task<AppSetting> UpdateAppSettings(AppSetting appSettings)
        {
            AppSetting existing = await GetAppSettings();

            existing.AuditWebhookUrl = appSettings.AuditWebhookUrl;
            existing.MaxFilesize = appSettings.MaxFilesize;
            existing.DefaultShareDurationHours = appSettings.DefaultShareDurationHours;

            Context.AppSettings.Update(existing);
            await Context.SaveChangesAsync();

            Config.ApplyAppSetting(existing);

            return existing;
        }

        public async Task ApplyAppSettings()
        {
            Config.ApplyAppSetting(await GetAppSettings());
        }
    }
}
