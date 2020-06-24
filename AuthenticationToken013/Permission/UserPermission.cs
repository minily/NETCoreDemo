using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationToken013.Permission
{
    public class UserPermission
    {
        /// <summary>
        /// 可以是 UserName or RoleName
        /// </summary>
        public string Name
        { get; set; }
        public string Url
        { get; set; }
    }
}
