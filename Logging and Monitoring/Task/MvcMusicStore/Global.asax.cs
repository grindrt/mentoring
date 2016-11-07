using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcMusicStore.Perfomance;
using NLog;
using PerformanceCounterHelper;

namespace MvcMusicStore
{
    public class MvcApplication : HttpApplication
    {
    private readonly ILogger _logger;
    public static readonly CounterHelper<Counters> CounterHelper = PerformanceHelper.CreateCounterHelper<Counters>("Test counter for music store");

        public MvcApplication()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _logger.Info("Application started");
        }
    }
}