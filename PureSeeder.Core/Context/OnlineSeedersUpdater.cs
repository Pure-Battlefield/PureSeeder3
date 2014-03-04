using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using PureSeeder.Core.Settings;


namespace PureSeeder.Core.Context
{
    class OnlineSeedersUpdater : IDataContextUpdater
    {
        public void UpdateContextData(IDataContext context, string pageData)
        {
            // Todo: Make regex pattern a global setting so it can more easily be changed
            //This had to be done with all regex patterns because DICE's player JSON list is not valid JSON!?!?
            var playersOnServer = new Regex(@"players"":\[.*\]");
            var playersJsonRegex = new Regex(@"(personaId).*?(guid)");
            var isOnlineRegex = new Regex(@"""isOnline"":true");
            var userNameRegex = new Regex(@"(username).*?,");

            Match playerListJsonMatch = playersOnServer.Match(pageData);

            if (playerListJsonMatch.Success) {
                context.Session.SeedersOnCurrentServer = 0;

                MatchCollection playerJsonMatch = playersJsonRegex.Matches(playerListJsonMatch.ToString());

                if (playerJsonMatch.Count > 0)
                {
                    foreach (var player in playerJsonMatch)
                    {
                        if (isOnlineRegex.Match(player.ToString()).Success)
                        {
                            Match usernameJson = userNameRegex.Match(player.ToString());
                            String username = usernameJson.ToString();
                            username = username.Substring(11,username.Length - 13);

                            foreach (var seederAccount in context.Settings.SeederAccounts)
                            {
                                if (seederAccount.BattleLogId.Equals(username))
                                {
                                    Console.WriteLine("SeederFound:" + username);
                                    context.Session.SeedersOnCurrentServer++;
                                }
                            }
                        }
                    }
                    Console.WriteLine(context.Session.SeedersOnCurrentServer + " seeders found.");
                } 
                else {
                    Console.WriteLine("No player JSON");
                    context.Session.SeedersOnCurrentServer = null;
                }
            }
        }
    }
}