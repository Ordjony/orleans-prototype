using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Prototype.Interfaces.Analytics;

namespace Prototype.Grains
{
    [StatelessWorker]
    public class RealtimeAnalyticsGrain : Grain, IRealtimeAnalyticsGrain
    {
        private IRealtimeAnalyticsStoreGrain _storeGrain;
        private readonly List<string> _actions = new List<string>();
        private IDisposable _disposableTimer;

        public override Task OnActivateAsync()
        {
            _storeGrain = GrainFactory.GetGrain<IRealtimeAnalyticsStoreGrain>(0);

            _disposableTimer = RegisterTimer(FlushActions, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));

            return base.OnActivateAsync();
        }

        public Task TrackAction(string message)
        {
            GetLogger().Info($"Put action to actions collection. Message: {message}");

            _actions.Add(message);
            return Task.CompletedTask;
        }

        private async Task FlushActions(object o)
        {
            GetLogger().Info($"Flush actions from stateless worker.");

            await _storeGrain.SaveActions(_actions.ToArray());
            _actions.Clear();
        }
    }
}
