using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagement.ViewModel.Login;
using StudentManagement.ViewModel.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModel.Login.Tests
{
    [TestClass()]
    public class ForgotPasswordViewModelTests
    {
        ForgotPasswordViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
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
        [DataRow(null)]
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
        [DataRow(null)]
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
    }
}