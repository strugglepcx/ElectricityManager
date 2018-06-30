using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freedom.ElectricityManager.SocketCenter.Models
{
    /// <summary>
    /// 添加包任务输入参数
    /// </summary>
    public class AddPacketTaskInput
    {
        /// <summary>
        /// 数据包
        /// </summary>
        public string PacketData { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public string DataContent { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 第一区域
        /// </summary>
        public string AreaOne { get; set; }

        /// <summary>
        /// 第二区域
        /// </summary>
        public string AreaTwo { get; set; }
    }
}
