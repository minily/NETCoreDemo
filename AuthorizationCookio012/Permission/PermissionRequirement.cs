using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationCookio012.Permission
{
    /// <summary>
    /// 验证许可
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement  // 空接口
    {
        /// <summary>
        /// 可授权访问的用户集合（从数据库获取）
        /// 防止外部对授权用户改变，可以定义为私有 - private
        /// </summary>
        public List<UserPermission> UserPermissions { get; private set; }

        // 禁止访问
        public string DeniedAction { get; set; }

        public PermissionRequirement(string deniedAction, List<UserPermission> userPermissions)
        {
            DeniedAction = deniedAction;
            UserPermissions = userPermissions;
        }
    }
}
