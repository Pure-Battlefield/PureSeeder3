using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PureSeeder.Core.Context;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.ServerManagement
{
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
}