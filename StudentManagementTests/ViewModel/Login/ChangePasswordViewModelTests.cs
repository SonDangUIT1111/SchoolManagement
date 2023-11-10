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

namespace StudentManagement.ViewModel.Login.Tests
{
    [TestClass()]
    public class ChangePasswordViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ChangePasswordViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            // Create the SuaHocSinhViewModel instance with the mock service
            viewModel = new ChangePasswordViewModel();
        }


        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.Id = "1";
            Assert.AreEqual("1", viewModel.Id);
            viewModel.IsHS = true;
            Assert.AreEqual(true, viewModel.IsHS);
            viewModel.TaiKhoan = "abc";
            Assert.AreEqual("abc", viewModel.TaiKhoan);
            viewModel.MatKhau = "123";
            Assert.AreEqual("123", viewModel.MatKhau);
            viewModel.ChangePasswordWD = null;
            Assert.IsNull(viewModel.ChangePasswordWD);
            viewModel.HocSinhHienTai = null;
            Assert.IsNull(viewModel.HocSinhHienTai);
            viewModel.GiaoVienHienTai = null;
            Assert.IsNull(viewModel.GiaoVienHienTai);
        }



        [TestMethod()]
        [DataRow("","")]
        [DataRow(null,"A1")]
        [DataRow("1","A1bc")]
        [DataRow("A1bc","A1b")]
        [DataRow("a1","a1")]
        [DataRow("Aa1","Aa1")]
        [DataRow("A1",null)]
        public void ValidatePasswordTest(string newPass, string confirmPass)
        {
            var result = viewModel.ValidatePassword(newPass,confirmPass);
            if (String.IsNullOrEmpty(newPass)||String.IsNullOrEmpty(confirmPass))
            {
                Assert.IsFalse(result);
                return;
            }
            bool flagUpcase = false, flagNum = false;
            foreach (char c in newPass)
            {
                if (c >= 'A' && c < 'Z' + 1)
                    flagUpcase = true;
                if (c >= '0' && c < '9' + 1)
                    flagNum = true;
            }
            if (flagUpcase && flagNum && newPass == confirmPass)
                Assert.IsTrue(result);
            else Assert.IsFalse(result);
        }

        [TestMethod()]
        [DataRow("","a","")]
        [DataRow(null,"a","A1")]
        [DataRow("a","b","c")]
        public void CheckValidPasswordTest(string a,string b, string c)
        {
            var result = viewModel.CheckValidPassword(a,b,c);
            if (a == null || b == null || c == null || a== ""|| b==""||c=="")
            {
                Assert.IsFalse(result);
                return;
            }
            Assert.IsTrue(result);  
        }

        [TestMethod]
        public void GetMatKhauCu_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ChangePasswordViewModel(fakeSqlConnection.Object);

            try
            {
                sut.IsHS = true;
                sut.Id = "100046";
                sut.GetMatKhauCu();
                sut.IsHS = false;
                sut.Id = "100031";
                sut.GetMatKhauCu();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void UpdateMatKhauMoiTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ChangePasswordViewModel(fakeSqlConnection.Object);

            try
            {
                sut.IsHS = true;
                sut.Id = "1";
                sut.UpdateMatKhauMoi("123456");
                sut.IsHS = false;
                sut.UpdateMatKhauMoi("123456");
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
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
    }
}