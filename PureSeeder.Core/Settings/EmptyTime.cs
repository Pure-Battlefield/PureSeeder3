using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Settings
{
    class EmptyTime : ITimes
    {
        public bool IsInTime()
        {
            return true;
        }
    }
}
