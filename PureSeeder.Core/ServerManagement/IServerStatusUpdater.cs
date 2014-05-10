using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.ServerManagement
{
    public interface IServerStatusUpdater
    {
        Task UpdateServerStatuses(IDataContext context);
    }

    public interface IUpdateServerIds
    {
        Task Update(IDataContext context, bool updateAll = false);
    }

    class UpdateServerIds : IUpdateServerIds
    {
        private async Task UpdateServerIdAsync(Server server)
        {
            using (var httpClient = new HttpClient())
            {
                var localServer = server;

                var response = await httpClient.GetAsync(localServer.Address).ConfigureAwait(true);
                
                var regex = new Regex("/pc/(.*?)/?$");

                var match = regex.Match(response.RequestMessage.RequestUri.ToString());

                if (!match.Success)
                    return;

                localServer.Id = match.Groups[1].Value;
            }
        }

        public async Task Update(IDataContext context, bool updateAll = false)
        {
            var servers = updateAll
                ? context.Settings.Servers
                : context.Settings.Servers.Where(x => String.IsNullOrEmpty(x.Id));

            var allTasks = servers.Select(UpdateServerIdAsync);

            await Task.WhenAll(allTasks);
        }
    }

//    class UpdateServerIds : IUpdateServerIds
//    {
//        public Task Update(IDataContext context, bool updateAll = false)
//        {
//            return Task.Factory.StartNew(async () =>
//            {
//                var servers = updateAll
//                    ? context.Settings.Servers
//                    : context.Settings.Servers.Where(x => String.IsNullOrEmpty(x.Id));
//
//                var httpClient = new HttpClient();
//                var tasks = new List<Task>();
//                foreach (var server in servers)
//                {
//                    var localServer = server;
//                        // Using foreach var in closure can cause unexpected results, so this is necessary
//                    tasks.Add(
//                        httpClient.GetAsync(localServer.Address)
//                            .ContinueWith(x => HandleResponse(localServer, x.Result)));
//                }
//
//                await Task.WhenAll(tasks.ToArray());
//            });
//        }
//
//        private void HandleResponse(Server server, HttpResponseMessage response)
//        {
//            if (response.RequestMessage.RequestUri == null)
//                return;
//
//            var regex = new Regex("/pc/(.*?)/");
//
//            var match = regex.Match(response.RequestMessage.RequestUri.ToString());
//
//            if (!match.Success)
//                return;
//
//            server.Id = match.Groups[1].Value;
//        }
//    }

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

//    public class ServerStatusUpdater : IServerStatusUpdater
//    {
//        private readonly IUpdateServerIds _serverIdUpdater;
//
//        public ServerStatusUpdater([NotNull] IUpdateServerIds serverIdUpdater)
//        {
//            if (serverIdUpdater == null) throw new ArgumentNullException("serverIdUpdater");
//            _serverIdUpdater = serverIdUpdater;
//        }
//
//        public Task UpdateServerStatuses(IDataContext context)
//        {
//            return Task.Factory.StartNew(async () =>
//            {
//                await _serverIdUpdater.Update(context);
//                var httpClient = new HttpClient();
//
//                var tasks = new List<Task>();
//                foreach (var server in context.Session.ServerStatuses)
//                {
//                    var address = String.Format(Constants.BattlelogUrlTemplates.ServerStatus, server.Id);
//                    tasks.Add(httpClient.GetAsync(address).ContinueWith(x => HandleResponse(context, address, x.Result)));
//                }
//
//                await Task.WhenAll(tasks.ToArray());
//            });
//        }
//
//        private void HandleResponse(IDataContext context, string address, HttpResponseMessage response)
//        {
//            if (response.StatusCode != HttpStatusCode.OK)
//            {
//                AddServerStatus(context, address, null, null);
//                return;
//            }
//
//            var stringContent = response.Content.ReadAsStringAsync().Result;
//
//            if (stringContent == null)
//            {
//                AddServerStatus(context, address, null, null);
//                return;
//            }
//
//            var json = JObject.Parse(stringContent);
//
//            if (json == null)
//            {
//                AddServerStatus(context, address, null, null);
//                return;
//            }
//
//            int? curPlayers = null;
//            int? serverMax = null;
//
//            try
//            {
//                curPlayers = json["slots"]["2"]["current"].Value<int>();
//                serverMax = json["slots"]["2"]["max"].Value<int>();
//            }
//            catch{}
//
//            AddServerStatus(context, address, curPlayers, serverMax);
//        }
//
//        private void AddServerStatus(IDataContext context, string address, int? curPlayers, int? serverMax)
//        {
//            context.Session.ServerStatuses.UpdateStatus(address, curPlayers, serverMax);
//        }
//    }

    // Deprecated
//    public class ServerStatusUpdater : IServerStatusUpdater
//    {
//        public Task UpdateServerStatuses(IDataContext context)
//        {
//            return Task.Factory.StartNew(() =>
//            {
//                var httpClient = new HttpClient();
//                var tasks = new List<Task>();
//
//                foreach (var serverStatus in context.Session.ServerStatuses)
//                {
//                    var address = serverStatus.Address;
//                    tasks.Add(httpClient.GetAsync(address).ContinueWith(x => HandleResponse(context, address, x.Result)));
//                }
//
//                Task.WaitAll(tasks.ToArray());
//            });
//        }
//
//        // Todo: This could be cleaned up, it's pretty sloppy
//        private void HandleResponse(IDataContext context, string address, HttpResponseMessage response)
//        {
//            if (response.StatusCode != HttpStatusCode.OK)
//            {
//                AddServerStatus(context, address, null, null);
//                return;
//            }
//
//            var stringContent = response.Content.ReadAsStringAsync().Result;
//
//            if (stringContent == null)
//            {
//                AddServerStatus(context, address, null, null);
//                return;
//            }
//
//            int? nCurPlayers = null, nMaxPlayers = null;
//
//            var curPlayersRegEx = new Regex(@"""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}");
//
//            var curPlayersMatch = curPlayersRegEx.Match(stringContent);
//
//            int curPlayers, maxPlayers;
//            
//            if(int.TryParse(curPlayersMatch.Groups[1].Value, out curPlayers))
//                nCurPlayers = curPlayers;
//            if (int.TryParse(curPlayersMatch.Groups[2].Value, out maxPlayers))
//                nMaxPlayers = maxPlayers;
//
//            var serverGuidRegex = new Regex(@"/servers/show/pc/(.*?)(?:/?$)");
//
//            var serverGuidMatch = serverGuidRegex.Match(response.RequestMessage.RequestUri.ToString());
//
//            string serverGuid = string.Empty;
//
//            if (serverGuidMatch.Success)
//            {
//                serverGuid = serverGuidMatch.Groups[1].Value;
//            }
//
//            AddServerStatus(context, address, nCurPlayers, nMaxPlayers);
//        }
//
//        private void AddServerStatus(IDataContext context, string address, int? curPlayers, int? serverMax)
//        {
//            context.Session.ServerStatuses.UpdateStatus(address, curPlayers, serverMax);
//        }
//    }

    
}