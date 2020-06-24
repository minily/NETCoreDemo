using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationToken013.Permission
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public List<UserPermission> UserPermissions { get; private set; }

        public string DeniedAction { get; set; }

        /// <summary>
        /// 基于Sid还是基于Role进行认证
        /// </summary>
        public string ClaimType { internal get; set; }

        public string LoginPath { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Token过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }

        /// <summary>
        /// 凭据
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }


        public PermissionRequirement(string deniedAction, List<UserPermission> userPermissions,string claimType,string issuer,string audience,SigningCredentials signingCredentials, TimeSpan expiration)
        {
            ClaimType = claimType;
            Issuer = issuer;
            Audience = audience;
            SigningCredentials = signingCredentials;
            DeniedAction = deniedAction;
            UserPermissions = userPermissions;
            Expiration = expiration;
        }

    }
}
