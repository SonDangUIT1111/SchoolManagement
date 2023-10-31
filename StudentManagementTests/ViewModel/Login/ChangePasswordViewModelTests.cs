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
    public class ChangePasswordViewModelTests
    {
        ChangePasswordViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
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
    }
}