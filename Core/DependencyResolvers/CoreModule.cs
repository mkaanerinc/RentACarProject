using Core.CrossCuttingConcerns.Caching.Abstract;
using Core.CrossCuttingConcerns.Caching.Concrete.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            serviceCollection.AddSingleton<Stopwatch>();
            serviceCollection.AddTransient<FileLogger>();
            serviceCollection.AddTransient<MsSqlLogger>();
        }
    }
}
