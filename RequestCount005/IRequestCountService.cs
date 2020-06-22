using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestCount005
{
    /// <summary>
    /// 计数器
    /// </summary>
    public interface IRequestCountService
    {
        Dictionary<string, bool> RequestList { get; set; }
    }

    public class RequestCountService : IRequestCountService
    {
        public Dictionary<string, bool> RequestList { get; set; }

        public RequestCountService()
        {
            RequestList = new Dictionary<string, bool>();
        }
    }
}
