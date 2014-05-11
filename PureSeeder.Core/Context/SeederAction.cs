using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Context
{
    public class SeederAction
    {
        public SeederAction(SeederActionType actionType, string actionReason, ServerStatus serverStatus = null)
        {
            ActionType = actionType;
            ActionReason = actionReason;
            ServerStatus = serverStatus;
        }

        public SeederActionType ActionType { get; private set; }
        public string ActionReason { get; private set; }
        public ServerStatus ServerStatus { get; private set; }
    }

    public enum SeederActionType
    {
        Seed,
        Stop,
        Noop
    }
}
