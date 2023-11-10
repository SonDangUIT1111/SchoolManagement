using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class ThemHocSinhMoiViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThemHocSinhMoiViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThemHocSinhMoiViewModel();
        }
        [TestMethod]
        public void ToShortDateTimeString_ValidInput_ReturnsExpected()
        {
            // Create a DatePicker object with the date 2020-10-10
            DatePicker date = new DatePicker { SelectedDate = new DateTime(2020, 10, 10) };

            // Expected result in the "yyyy-MM-dd" format
            string expected = "2020-10-10";

            // Call the method and get the result
            string result = viewModel.ToShortDateTime(date);

            // Assert that the result matches the expected value
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void ToShortDateTimeString_InvalidInput_ReturnsEmptyString()
        {
            // Create a DatePicker object with no selected date (null).
            DatePicker date = new DatePicker { SelectedDate = null };

            // Expected result when the input is invalid (empty string).
            string expected = string.Empty;

            // Call the method and get the result.
            string result = viewModel.ToShortDateTime(date);

            // Assert that the result matches the expected value.
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Base64Encode_ValidInput_ReturnsExpected()
        {
            string input = "password";
            string expected = "cGFzc3dvcmQ=";
            string result = viewModel.Base64Encode(input);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Base64Encode_EncodesEmptyString()
        {
            string input = "";
            string expected = "";
            string result = viewModel.Base64Encode(input);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Create_MD5Encode_ValidInput_ReturnsExpected()
        {
            string input = "password";
            string expected = "5F4DCC3B5AA765D61D8327DEB882CF99";
            string result = viewModel.CreateMD5(input);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Create_MD5Encode_EncodesEmptyString()
        {
            string input = "";
            string expected = "D41D8CD98F00B204E9800998ECF8427E";
            string result = viewModel.CreateMD5(input);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void SendAccoutnByEmail_ValidInput_ReturnsExpected()
        {
            try
            {
                viewModel.SendAccountByEmail("ab", "abc", "a@gmail.com");
                Assert.IsTrue(true); ;
            } catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ThemhocSinhMoi_ValidInput_ShouldCreate()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThemHocSinhMoiViewModel(fakeSqlConnection.Object);

            sut.ThemHocSinhWD = new ThemHocSinhMoi();
            sut.ThemHocSinhWD.Hoten.Text = "John Doe";
            sut.ThemHocSinhWD.NgaySinh.SelectedDate = new DateTime(2007, 11, 13);
            sut.ThemHocSinhWD.Male.IsChecked = true;
            sut.ThemHocSinhWD.DiaChi.Text = "123 Main St";
            sut.ThemHocSinhWD.Email.Text = "john.doe@example.com";

            try
            {
                sut.ThemHocSinhMoi();
                Assert.IsTrue(true);

            } catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }
    }
}
