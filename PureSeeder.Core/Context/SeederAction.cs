using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Context
{
    public class SeederAction
    {
        public SeederAction(SeederActionType actionType)
        {
            ActionType = actionType;
            ActionData = new Dictionary<string, object>();
        }

        public SeederActionType ActionType { get; private set; }
        public IDictionary<string, object> ActionData { get; private set; } 
    }

    public enum SeederActionType
    {
        Seed,
        Stop,
        Noop
    }
}
