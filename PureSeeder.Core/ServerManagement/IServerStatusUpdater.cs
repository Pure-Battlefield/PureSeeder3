using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PureSeeder.Core.Context;
using PureSeeder.Core.Settings;

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
            return Task.Factory.StartNew(() =>
            {
                var httpClient = new HttpClient();
                var tasks = new List<Task>();

                foreach (var serverStatus in context.Session.ServerStatuses)
                {
                    var address = serverStatus.Address;
                    tasks.Add(httpClient.GetAsync(address).ContinueWith(x => HandleResponse(context, address, x.Result)));
                }

                Task.WaitAll(tasks.ToArray());
            });
        }

        // Todo: This could be cleaned up, it's pretty sloppy
        private void HandleResponse(IDataContext context, string address, HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                AddServerStatus(context, address, null, null, string.Empty);
                return;
            }

            var stringContent = response.Content.ReadAsStringAsync().Result;

            if (stringContent == null)
            {
                AddServerStatus(context, address, null, null, string.Empty);
                return;
            }

            int? nCurPlayers = null, nMaxPlayers = null;

            var curPlayersRegEx = new Regex(@"""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}");

            var curPlayersMatch = curPlayersRegEx.Match(stringContent);

            int curPlayers, maxPlayers;
            
            if(int.TryParse(curPlayersMatch.Groups[1].Value, out curPlayers))
                nCurPlayers = curPlayers;
            if (int.TryParse(curPlayersMatch.Groups[2].Value, out maxPlayers))
                nMaxPlayers = maxPlayers;

            var serverNameRegex = new Regex(@"""name"":""(.*?)"",");

            var serverPlayersMatch = serverNameRegex.Match(stringContent);

            string serverName = string.Empty;

            if (serverPlayersMatch.Success)
            {
                serverName = serverPlayersMatch.Groups[1].Value;
            }

            AddServerStatus(context, address, nCurPlayers, nMaxPlayers, serverName);
        }

        private void AddServerStatus(IDataContext context, string address, int? curPlayers, int? serverMax, string blServerName)
        {
            context.Session.ServerStatuses.UpdateStatus(address, curPlayers, serverMax, blServerName);
        }
    }

    
}