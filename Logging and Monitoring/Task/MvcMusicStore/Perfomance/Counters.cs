
using System.Diagnostics;
using PerformanceCounterHelper;

namespace MvcMusicStore.Perfomance
{
    [PerformanceCounterCategory("MvcMusicStore", PerformanceCounterCategoryType.MultiInstance, "MvcMusicStore perfomance counter")]
    public enum Counters
    {
        [PerformanceCounter("LogInCounter", "Counter of success log in operation", PerformanceCounterType.NumberOfItems32)]
        LogIn,

        [PerformanceCounter("LogOfCounter", "Counter of success log of operation", PerformanceCounterType.NumberOfItems32)]
        LogOff,

        [PerformanceCounter("RegistrationCounter", "Counter of success registration operation", PerformanceCounterType.NumberOfItems32)]
        Registration
    }
}