using System.Text.RegularExpressions;

namespace PureSeeder.Core.Context
{
    class Bf4PlayerCountsUpdater : IDataContextUpdater
    {
        public void UpdateContextData(IDataContext context, string pageData)
        {
            //""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}
            // Todo: Make regex pattern a global setting so it can more easily be changed
            var curPlayersRegEx = new Regex(@"""slots"".*?""2"":{""current"":(.*?),""max"":(.*?)}");

            var curPlayers = curPlayersRegEx.Match(pageData);

            if (!curPlayers.Success)
            {
                context.Session.CurrentPlayers = null;
                context.Session.ServerMaxPlayers = null;
                return;
            }

            int currentPlayers, maxPlayers;

            int.TryParse(curPlayers.Groups[1].Value, out currentPlayers);
            int.TryParse(curPlayers.Groups[2].Value, out maxPlayers);

            context.Session.CurrentPlayers = currentPlayers;
            context.Session.ServerMaxPlayers = maxPlayers;
        }
    }
}