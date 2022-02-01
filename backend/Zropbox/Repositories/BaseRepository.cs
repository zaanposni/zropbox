using Zropbox.Data;
using Zropbox.Services;

namespace Zropbox.Repositories
{
    public class BaseRepository<T>
    {
        protected ILogger<T> Logger { get; set; }
        protected DataContext Context { get; set; }
        protected readonly InternalConfiguration Config;
        protected IServiceProvider ServiceProvider { get; set; }

        public BaseRepository(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<T>>();
            Context = serviceProvider.GetRequiredService<DataContext>();
            Config = serviceProvider.GetRequiredService<InternalConfiguration>();
            ServiceProvider = serviceProvider;
        }
    }
}
