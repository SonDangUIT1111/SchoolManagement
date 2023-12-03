using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.Login;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.Login;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Text;

namespace StudentManagement.ViewModel.Login.Tests
{
    [TestClass()]
    public class LoginViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private LoginViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new LoginViewModel();
            Assert.AreEqual(viewModel.IndexRole, -1);
        }


        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.IndexRole = 1;
            Assert.AreEqual(1, viewModel.IndexRole);
            viewModel.Username = "hello";
            Assert.AreEqual("hello", viewModel.Username);
            viewModel.Password = "1234";
            Assert.AreEqual("1234", viewModel.Password);
            viewModel.LoginWindow = null;
            Assert.IsNull(viewModel.LoginWindow);
        }




        [TestMethod()]
        [DataRow("", "a1")]
        [DataRow("a1", "")]
        [DataRow("", "")]
        [DataRow("a1", "a1")]
        [DataRow(null, "")]
        [DataRow("", null)]
        [DataRow(null, null)]
        public void ValidateInfoTest(string a, string b)
        {
            var result = viewModel.ValidateInfo(a, b);
            if (a == "" || b == "" || a == null || b == null)
                Assert.IsFalse(result);
            else Assert.IsTrue(result);
        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-2)]
        [DataRow(4)]
        [DataRow(2)]
        public void CheckInvalidRoleTest(int a)
        {
            if (a >= 0 && a <= 2)
            {
                Assert.IsTrue(viewModel.CheckInvalidRole(a));
            }
            else Assert.IsFalse(viewModel.CheckInvalidRole(a));
        }

        [TestMethod]
        public void GetThongTin()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });


            var sut = new LoginViewModel();
            Assert.IsNotNull(sut);

            try
            {
                sut.Username = "hs100046";
                sut.IndexRole = 2;
                var result = sut.GetThongTin(CreateMD5(Base64Encode("123456")));
                Assert.AreEqual(result, 100046);
                sut.Username = "gv100031";
                sut.IndexRole = 1;
                var result2 = sut.GetThongTin(CreateMD5(Base64Encode("123456")));
                Assert.AreEqual(result2, 100031);
                sut.Username = "admin";
                sut.IndexRole = 0;
                var result3 = sut.GetThongTin(CreateMD5(Base64Encode("123456")));
                Assert.AreEqual(result3, 1);
                sut.Username = "aaaaa";
                sut.IndexRole = 0;
                var result4 = sut.GetThongTin(CreateMD5(Base64Encode("9")));
                Assert.AreEqual(result4, -1);
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void MD5Hash_Test()
        {
            var result = viewModel.CreateMD5("hello");
            Assert.AreEqual(result, "5D41402ABC4B2A76B9719D911017C592");
        }

        [TestMethod]
        public void Base4Hash_Test()
        {
            var result = viewModel.Base64Encode("password");
            Assert.AreEqual(result, "cGFzc3dvcmQ=");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                //return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}