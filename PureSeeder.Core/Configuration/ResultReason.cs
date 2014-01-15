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

        public ResultReason(bool result, T reason, string stringReason)
        {
            Result = result;
            Reason = reason;
            StringReason = stringReason;
        }
        public bool Result { get; set; }
        public T Reason { get; set; }
        public string StringReason { get; set; }
    }
}
