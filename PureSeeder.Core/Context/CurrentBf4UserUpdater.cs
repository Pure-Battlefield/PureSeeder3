using System.Text.RegularExpressions;

namespace PureSeeder.Core.Context
{
    class CurrentBf4UserUpdater : IDataContextUpdater
    {
        public void UpdateContextData(IDataContext context, string pageData)
        {
            // Todo: Make regex pattern a global setting so it can more easily be changed
            var curUserRegEx = new Regex(@"class=""username""\W*href=""/bf4/user/(.*?)/");

            var curUser = curUserRegEx.Match(pageData);

            if (!curUser.Success)
            {
                context.Session.CurrentLoggedInUser = "None";
                return;
            }

            context.Session.CurrentLoggedInUser = curUser.Groups[1].Value;
        }
    }
}