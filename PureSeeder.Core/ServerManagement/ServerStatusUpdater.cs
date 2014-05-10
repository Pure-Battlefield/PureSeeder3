using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

                var json = JObject.Parse(response);

                if (json == null)
                    return;

                serverStatus.CurPlayers = json["slots"]["2"]["current"].Value<int>();
                serverStatus.ServerMax = json["slots"]["2"]["max"].Value<int>();
            }
        }

        public async Task UpdateServerStatuses(IDataContext context)
        {
            await _serverIdUpdater.Update(context);

            var allTasks = context.Session.ServerStatuses.Select(UpdateServerStatus);

            await Task.WhenAll(allTasks);
        }
    }
}