using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class CommonResponse
    {
        public class HttpCode
        {
            public const int Ok = 200;
            public const int Fail = 500;
            /// <summary>
            /// DZW框架中ajax请求失败时错误返回码为300
            /// </summary>
            public const int Error = 300;
        }

        /// <summary>
        /// WebApi服务返回消息基类
        /// </summary>
        public class CommonServiceResponseMessage
        {
            public CommonServiceResponseMessage()
            {
                StatusCode = HttpCode.Ok;
            }

            /// <summary>
            /// 返回状态码
            /// </summary>
            public int StatusCode { get; set; }

            /// <summary>
            /// 是否成功
            /// </summary>
            public bool Success
            {
                get { return StatusCode == HttpCode.Ok; }
                set
                {
                    StatusCode = value ? HttpCode.Ok : HttpCode.Fail;
                }
            }

            /// <summary>
            /// 错误消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 设定是否成功
            /// </summary>
            /// <param name="bSuccess"></param>
            /// <param name="msg"></param>
            private void SetStatus(bool bSuccess, string msg = null)
            {
                if (msg != null) Message = msg;

                if (bSuccess)
                {
                    StatusCode = HttpCode.Ok;
                    return;
                }
                StatusCode = HttpCode.Fail;
            }

            /// <summary>
            /// 失败并传入错误信息
            /// </summary>
            /// <param name="msg"></param>
            public void Failed(string msg, int stateCode = HttpCode.Fail)
            {
                StatusCode = stateCode;
                SetStatus(false, msg);
            }

            /// <summary>
            /// 失败并记录Exception中的日志
            /// </summary>
            /// <param name="exp"></param>
            public void Failed(Exception exp)
            {
                SetStatus(false, exp.Message);
            }

            /// <summary>
            /// 成功并添加Message信息
            /// </summary>
            /// <param name="msg"></param>
            public void Succeed(string msg = "")
            {
                Success = true;
                Message = msg;
            }
        }

        public class ServiceResponseMessage<T> : CommonServiceResponseMessage
        {
            public T Data { get; set; }
        }
    }
}
