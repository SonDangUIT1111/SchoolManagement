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
    public class LoginViewModelTests
    {
        LoginViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new LoginViewModel();
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
        public void CheckInvalidRoleTest(int a)
        {
            if (a >= 0 && a <= 2)
            {
                Assert.IsTrue(viewModel.CheckInvalidRole(a));
            }
            else Assert.IsFalse(viewModel.CheckInvalidRole(a));
        }
    }
}