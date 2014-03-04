using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    class ExpansionEnabledUpdater: IDataContextUpdater
    {
        public void UpdateContextData(IDataContext context, string pageData)
        {
            var expansionRegex = new Regex(@"common-gameexpansion");
            
            //Default to false incase the regex changes.
            context.Session.expansionEnabled = false;
            
            Match expansion = expansionRegex.Match(pageData);

            if (expansion.Success)
            {
                context.Session.expansionEnabled = true;
            }
        }
    }
}
