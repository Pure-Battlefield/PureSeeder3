using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.Configuration
{
    public class ResultReason<T>
    {
        public ResultReason(bool result)
        {
            Result = result;
        }

        public ResultReason(bool result, T reason)
        {
            Result = result;
            Reason = reason;
        }
        public bool Result { get; set; }
        public T Reason { get; set; }
    }
}
