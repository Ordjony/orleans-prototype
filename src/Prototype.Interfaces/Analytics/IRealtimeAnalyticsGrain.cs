using System.Threading.Tasks;
using Orleans;

namespace Prototype.Interfaces.Analytics
{
    public interface IRealtimeAnalyticsGrain : IGrainWithIntegerKey
    {
        Task TrackAction(string message);
    }
}
