using CDN.Data;
using CDN.Services;

namespace CDN.Repositories
{
    public class BaseRepository<T>
    {
        protected ILogger<T> Logger { get; set; }
        protected DataContext Context { get; set; }
        protected readonly InternalConfiguration Config;

        public BaseRepository(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<T>>();
            Context = serviceProvider.GetRequiredService<DataContext>();
            Config = serviceProvider.GetRequiredService<InternalConfiguration>();
        }
    }
}
