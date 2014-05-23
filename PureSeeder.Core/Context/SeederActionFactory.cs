using System.Linq;
using System.Threading.Tasks;

namespace PureSeeder.Core.Context
{
    class SeederActionFactory : ISeederActionFactory
    {
        public Task<SeederAction> GetAction(IDataContext context)
        {
            return Task<SeederAction>.Factory.StartNew(() => { 
                if(!context.Session.SeedingEnabled)
                    return new SeederAction(SeederActionType.Noop, "Seeding is disabled.");

                // Iterate over servers where SeedingEnabled is true
                foreach (var serverStatus in context.Session.ServerStatuses.Where(status => status.SeedingEnabled))
                {
                    if(!context.IsSeeding() && serverStatus.CurPlayers < serverStatus.MinPlayers)
                        return new SeederAction(SeederActionType.Seed, "Start seeding on highest priority server.", serverStatus);

                    if (!context.IsSeeding() && serverStatus.CurPlayers >= serverStatus.MinPlayers)
                        continue;

                    if(context.IsSeeding() && !IsCurrentServer(serverStatus, context) && serverStatus.CurPlayers < serverStatus.MinPlayers)
                        return new SeederAction(SeederActionType.Stop, "Not seeding the highest priority server that needs seeding.");

                    if(context.IsSeeding() && IsCurrentServer(serverStatus, context) && serverStatus.CurPlayers >= serverStatus.MaxPlayers)
                        return new SeederAction(SeederActionType.Stop, "Seeding no longer needed on this server.");

                    if(context.IsSeeding() && IsCurrentServer(serverStatus, context) && serverStatus.CurPlayers < serverStatus.MaxPlayers)
                        return new SeederAction(SeederActionType.Noop, "Continue seeding this server.");
                }

                // After iterating through all the servers, if nothing has been returned, then no seeding is needed
                return new SeederAction(SeederActionType.Noop, "No seeding needed.");
            });
        }

        private bool IsCurrentServer(ServerStatus thisServer, IDataContext context)
        {
            try
            {
                return thisServer.Id == context.Session.CurrentServer.Id;
            }
            catch
            {
                return false;
            }
        }
    }
}