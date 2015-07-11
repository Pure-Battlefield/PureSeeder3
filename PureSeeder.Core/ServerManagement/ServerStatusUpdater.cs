using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;

namespace PureSeeder.Core.ServerManagement
{
    public class ServerStatusUpdater : IServerStatusUpdater
    {
        private readonly IUpdateServerIds _serverIdUpdater;

        public ServerStatusUpdater([NotNull] IUpdateServerIds serverIdUpdater)
        {
            if (serverIdUpdater == null) throw new ArgumentNullException("serverIdUpdater");
            _serverIdUpdater = serverIdUpdater;
        }

        private async Task UpdateServerStatus(ServerStatus serverStatus)
        {
            if (String.IsNullOrEmpty(serverStatus.Id))
                return;

            using (var httpClient = new HttpClient())
            {
                var address = String.Format(Constants.BattlelogUrlTemplates.ServerStatus, serverStatus.Id);
                var response = await httpClient.GetStringAsync(address).ConfigureAwait(true);

                if (String.IsNullOrEmpty(response))
                    return;

                
                ServerInfo serverInfo;

                try
                {
                    serverInfo = JsonConvert.DeserializeObject<ServerInfo>(response);
                }
                catch
                {
                    return;
                }

                if (serverInfo != null && serverInfo.Slots != null && serverInfo.Slots.Players != null)
                {
                    serverStatus.CurPlayers = serverInfo.Slots.Players.Current;
                    serverStatus.ServerMax = serverInfo.Slots.Players.Max;
                }
            }
        }

        public async Task UpdateServerStatuses(IDataContext context)
        {
            await _serverIdUpdater.Update(context);

            var allTasks = context.Session.CurrentTimesCollection.CurrentTimes.ServerStatuses.Select(UpdateServerStatus);

            await Task.WhenAll(allTasks);
        }
    }

    public class ServerInfo
    {
        public ServerSlots Slots { get; set; }
    }

    public class ServerSlots
    {
        [JsonProperty(PropertyName = "1")]
        public ServerSlot Queue { get; set; }
        [JsonProperty(PropertyName = "2")]
        public ServerSlot Players { get; set; }
        [JsonProperty(PropertyName = "4")]
        public ServerSlot Commanders { get; set; }
        [JsonProperty(PropertyName = "8")]
        public ServerSlot Spectators { get; set; }

    }

    public class ServerSlot
    {
        public int Current { get; set; }
        public int Max { get; set; }
    }
}