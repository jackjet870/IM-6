using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IM.Common;
using System.Runtime.InteropServices;

namespace IM.UnitTest
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void TestStringSearialization()
        {
            string _rawString = "Hello World!", _deserializedString;
            var _serializedBytes = SerializationHelper.Encode(_rawString);
            _deserializedString = SerializationHelper.Decode(_serializedBytes);
            Assert.AreEqual(_rawString, _deserializedString, string.Format("序列化和反序列化字符串失败，原字符串:{0}，反序列化后字符串:{1}", _rawString, _deserializedString));
        }

        [TestMethod]
        public void TestObjectSerialization()
        {
            var _user1 = new UserInfo
            {
                UserId = 1001,
                UserName = "Julia",
            };

            var _bytes = SerializationHelper.Encode(_user1);
            var _user2 = SerializationHelper.Decode<UserInfo>(_bytes);

            var _address1 = new Address
            {
                Province = "上海",
                City = "上海市",
                District = "闸北区",
                Street = "永和路118号",
            };
            var _bytes2 = SerializationHelper.Encode(_address1);
            var _address2 = SerializationHelper.Decode<Address>(_bytes2);

            Assert.AreEqual(_user1.ToString(), _user2.ToString(), string.Format("序列化和反序列化失败，原对象:{0}，序列化反序列化后对象:{1}", _user1.ToString(), _user2.ToString()));
            Assert.AreEqual(_address1.ToString(), _address2.ToString(), string.Format("序列化和反序列化失败，原对象:{0}，序列化反序列化后对象:{1}", _address1.ToString(), _address2.ToString()));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct UserInfo
    {
        public Int32 UserId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string UserName;

        public const int Length = 6;

        public override string ToString()
        {
            return string.Format("UserId:{0},UserName:{1}", UserId, UserName);
        }
    }

    [Serializable]
    class Address
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        public override string ToString()
        {
            return string.Format("Province:{0},City:{1},District:{2},Street:{3}", Province, City, District, Street);
        }
    }
}
