using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientDemo011
{
    /// <summary>
    /// 获取请求的上下文做一些处理
    /// 截取Request和Reponse，类似于Middlewara中间件
    /// 不同的是 Middlewara处理的是别人请求过来的时候
    /// </summary>
    public class MyHttpClientHandler : DelegatingHandler
    {
        /// <summary>
        /// 使用场景:对请求request 和 响应response 进行监控和处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
        {
            // 处理request
            // todo

            var response = base.SendAsync(request, cancellationToken);

            // 处理response
            // todo

            return response;
        }
    }
}
