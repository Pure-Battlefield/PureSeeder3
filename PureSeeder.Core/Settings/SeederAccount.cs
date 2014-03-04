using System.ComponentModel;
using PureSeeder.Core.Context;

namespace PureSeeder.Core.Settings
{
    public class SeederAccount : BindableBase
    {
        private string _battleLogId;

        [Description("Battle Log ID of seeder account")]
        public string BattleLogId
        {
            get { return this._battleLogId; }
            set { SetField(ref _battleLogId, value); }
        }


    }
}
