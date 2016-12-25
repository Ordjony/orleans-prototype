using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Prototype.Interfaces.Analytics;

namespace Prototype.Grains
{
    public class RealtimeAnalyticsStoreGrain : Grain, IRealtimeAnalyticsStoreGrain
    {
        public Task SaveActions(IReadOnlyCollection<string> messages)
        {
            GetLogger().Info($"Save action messages to store. Mesages count {messages.Count}");

            // TODO: Save to store actions.
            return Task.CompletedTask;
        }
    }
}
