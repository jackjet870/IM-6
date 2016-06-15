using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IM.Common;

namespace IM.UnitTest
{
    [TestClass]
    public class DeviceTest
    {
        [TestMethod]
        public void TestNetworkAvaiable()
        {
            Assert.AreEqual(true, DeviceHelper.IsNetWorkAvaiable(), "网络不可连接");
        }

        [TestMethod]
        public void TestGetLocalIpAddress()
        {
            var _ip = DeviceHelper.GetLocalHost();
            Assert.IsFalse(string.IsNullOrEmpty(_ip), "找不到本机IP地址");
        }

        [TestMethod]
        public void TestGetAvaiablePort()
        {
            var _port = DeviceHelper.GetAvaiablePort();
            Assert.IsTrue(_port > 0 && _port < 65535, "找不到合适的端口号");
        }
    }
}
