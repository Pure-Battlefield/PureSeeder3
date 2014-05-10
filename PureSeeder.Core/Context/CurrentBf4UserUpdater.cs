using System.Text.RegularExpressions;
using PureSeeder.Core.Configuration;

namespace PureSeeder.Core.Context
{
    class CurrentBf4UserUpdater : IDataContextUpdater
    {
        public void Update(IDataContext context, string pageData)
        {
            // Todo: Make regex pattern a global setting so it can more easily be changed
            var curUserRegEx = new Regex(@"class=""username""\W*href=""/bf4/user/(.*?)/");

            var curUser = curUserRegEx.Match(pageData);

            if (!curUser.Success)
            {
                context.Session.CurrentLoggedInUser = Constants.NotLoggedInUsername;
                return;
            }

            context.Session.CurrentLoggedInUser = curUser.Groups[1].Value;
        }
    }
}