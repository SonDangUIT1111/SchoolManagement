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
    public class ForgotPasswordViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ForgotPasswordViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            // Create the SuaHocSinhViewModel instance with the mock service
            viewModel = new ForgotPasswordViewModel();
            Assert.IsNotNull(viewModel);
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
            code = "A";
            result = viewModel.CheckValidCode(code);
            Assert.AreEqual(false, result);
            code = "Z";
            result = viewModel.CheckValidCode(code);
            Assert.AreEqual(false, result);
            code = "1";
            result = viewModel.CheckValidCode(code);
            Assert.AreEqual(false, result);
            code = "9";
            result = viewModel.CheckValidCode(code);
            Assert.AreEqual(false, result);

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

            var sut = new ForgotPasswordViewModel();

            try
            {
                sut.NewPassword = "123456";
                sut.IndexRole = 0;
                sut.EmailProtected = "";
                var result = sut.DoiMatKhauMoi();
                Assert.AreEqual(result, 0);
                sut.IndexRole = 1;
                result = sut.DoiMatKhauMoi();
                Assert.AreEqual(result, 0);
                sut.IndexRole = 2;
                result = sut.DoiMatKhauMoi();
                Assert.AreEqual(result, 0);
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

            var sut = new ForgotPasswordViewModel();

            try
            {
                sut.IndexRole = 0;
                sut.EmailProtected = "";
                var result = sut.GetThongTin();
                Assert.AreEqual(result, 0);
                sut.IndexRole = 1;
                sut.EmailProtected = "thuyhang_edu_thuduc@gmail.com";
                result = sut.GetThongTin();
                Assert.AreEqual(result, 1);
                sut.IndexRole = 2;
                sut.EmailProtected = "thienthanh2007@gmail.com";
                result = sut.GetThongTin();
                Assert.AreEqual(result, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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