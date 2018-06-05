using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;

namespace Freedom.ElectricityManager.SocketCenter
{
    class MyReceiveFilter : FixedSizeReceiveFilter<StringRequestInfo>
    {
        public MyReceiveFilter()
            : base(16) //传入固定的请求大小
        {

        }

        protected override StringRequestInfo ProcessMatchedRequest(byte[] buffer, int offset, int length, bool toBeCopied)
        {
            //TODO: 通过解析到的数据来构造请求实例，并返回
            var bodyString = Encoding.UTF8.GetString(buffer);
            var areaCode = bodyString.Substring(0, 2);
            var detailAreaCode = bodyString.Substring(2, 2);
            var equipmentCode = bodyString.Substring(4, 8);
            var commandContent = bodyString.Substring(12, 4);
            return new StringRequestInfo("RECEVIE", bodyString, new[] { areaCode, detailAreaCode, equipmentCode, commandContent });
        }
    }
}
