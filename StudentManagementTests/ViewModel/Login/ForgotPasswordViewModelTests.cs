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
    public class ForgotPasswordViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ForgotPasswordViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            // Create the SuaHocSinhViewModel instance with the mock service
            viewModel = new ForgotPasswordViewModel();
        }


        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.IndexRole = 1;
            Assert.AreEqual(1, viewModel.IndexRole);
            viewModel.EmailProtected = "a@gmail.com";
            Assert.AreEqual("a@gmail.com", viewModel.EmailProtected);
            viewModel.NewPassword = "1234";
            Assert.AreEqual("1234", viewModel.NewPassword);
            viewModel.ConfirmNewPassword = "1234";
            Assert.AreEqual("1234", viewModel.ConfirmNewPassword);
            viewModel.Code = "1234";
            Assert.AreEqual("1234", viewModel.Code);
        }


        [TestMethod()]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("1")]
        [DataRow("A")]
        [DataRow("a1")]
        [DataRow("Aa")]
        [DataRow("A1")]
        public void CheckValidPasswordTest(string pass)
        {
            var result = viewModel.CheckValidPassword(pass);
            if (String.IsNullOrEmpty(pass))
            {
                Assert.IsFalse(result);
                return;
            }
            bool flagUpcase = false, flagNum = false;
            foreach (char c in pass)
            {
                if (c >= 'A' && c < 'Z' + 1)
                    flagUpcase = true;
                if (c >= '0' && c < '9' + 1)
                    flagNum = true;
            }
            if (flagUpcase && flagNum)
                Assert.IsTrue(result);
            else Assert.IsFalse(result);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow("gmail@gmail")]
        [DataRow("gmail.com")]
        [DataRow("gmail")]
        [DataRow("gmail@gmail.com")]
        public void CheckValidEmailTest(string email)
        {
            var result = viewModel.CheckValidEmail(email);
            if (String.IsNullOrEmpty(email))
                Assert.IsFalse(result);
            if (email.Contains("@") && email.Contains("."))
                Assert.IsTrue(result);
            else Assert.IsFalse(result);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow("1")]
        [DataRow("abc211")]
        [DataRow("abcdef")]
        [DataRow("123456")]
        public void CheckValidCodeTest(string code)
        {
            var result = viewModel.CheckValidCode(code);
            if (code.Length != 6 || String.IsNullOrEmpty(code))
            {
                Assert.IsFalse(result);
                return;
            }
            bool flag = true;
            try
            {
                Int32.Parse(code);
            }
            catch
            {
                flag = false;
            }
            Assert.AreEqual(flag, result);

        }


        [TestMethod]
        public void DoiMatKhauMoiTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ForgotPasswordViewModel(fakeSqlConnection.Object);

            try
            {
                sut.NewPassword = "";
                sut.IndexRole = 0;
                sut.EmailProtected = "";
                sut.DoiMatKhauMoi();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void KiemTraEmailCoTonTai()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ForgotPasswordViewModel(fakeSqlConnection.Object);

            try
            {
                sut.IndexRole = 0;
                sut.GetThongTin();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SendMail_Test()
        {
            try
            {
                viewModel.SendCodeByEmail("ab", "a@gmail.com");
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }
}