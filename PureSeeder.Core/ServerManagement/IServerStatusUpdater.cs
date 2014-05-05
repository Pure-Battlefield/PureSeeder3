using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PureSeeder.Core.Context;

namespace PureSeeder.Core.ServerManagement
{
    public interface IServerStatusUpdater
    {
        Task UpdateServerStatuses(IDataContext context);
    }

    public class ServerStatusUpdater : IServerStatusUpdater
    {
        public Task UpdateServerStatuses(IDataContext context)
        {
            return new Task(() =>
                {
                    // Clear out current statuses
                    context.Session.ServerStatuses.Clear();

                    var httpClient = new HttpClient();

                    foreach (var server in context.Settings.Servers)
                    {
                        var address = server.Address;
                        httpClient.GetAsync(server.Address).ContinueWith(x => HandleResponse(context, address, x.Result));
                    }
                });
        }

        // Todo: This could be cleaned up, it's pretty sloppy
        private void HandleResponse(IDataContext context, string address, HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                AddServerStatus(context, address, null, null);
                return;
            }

            var stringContent = response.Content.ReadAsStringAsync().Result;

            if (stringContent == null)
            {
                AddServerStatus(context, address, null, null);
                return;
            }

            var curPlayersRegEx = new Regex(@"""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}");

            var curPlayersMatch = curPlayersRegEx.Match(stringContent);

            if (!curPlayersMatch.Success)
            {
                AddServerStatus(context, address, null, null);
                return;
            }

            int curPlayers, maxPlayers;
            int? nCurPlayers = null, nMaxPlayers = null;

            if(int.TryParse(curPlayersMatch.Groups[1].Value, out curPlayers))
                nCurPlayers = curPlayers;
            if (int.TryParse(curPlayersMatch.Groups[2].Value, out maxPlayers))
                nMaxPlayers = maxPlayers;

            AddServerStatus(context, address, nCurPlayers, nMaxPlayers);
        }

        private void AddServerStatus(IDataContext context, string address, int? curPlayers, int? maxPlayers)
        {
            context.Session.ServerStatuses.Add(address, new ServerStatus() {CurPlayers = curPlayers, MaxPlayers = maxPlayers});
        }
    }

    
}