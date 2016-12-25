using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Prototype.Interfaces.Analytics
{
    public interface IRealtimeAnalyticsStoreGrain : IGrainWithIntegerKey
    {
        Task SaveActions(IReadOnlyCollection<string> messages);
    }
}
