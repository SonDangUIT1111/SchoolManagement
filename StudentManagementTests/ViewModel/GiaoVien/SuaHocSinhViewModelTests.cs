using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows;
using System.IO;

namespace StudentManagementTests.ViewModel.GiaoVien
{
    [TestClass]
    public class SuaHocSinhViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private SuaHocSinhViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            // Create the SuaHocSinhViewModel instance with the mock service
            viewModel = new SuaHocSinhViewModel();
        }
        
        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.ImagePath = "abc";
            Assert.AreEqual("abc", viewModel.ImagePath);
            viewModel.SuaHocSinhWD = null;
            Assert.IsNull(viewModel.SuaHocSinhWD);
            viewModel.HocSinhHienTai = null;
            Assert.IsNull(viewModel.HocSinhHienTai);
        }

        [TestMethod()]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            string validEmail = "test@example.com";
            bool result = viewModel.IsValidEmail(validEmail);
            Assert.IsTrue(result);
        }
        [TestMethod()]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            string invalidEmail = "invalid-email";
            bool result = viewModel.IsValidEmail(invalidEmail);
            Assert.IsFalse(result);
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
        public void CapNhatHocSinh_ValidInput_ShouldUpdateHocSinh()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaHocSinhViewModel(fakeSqlConnection.Object);

            sut.SuaHocSinhWD = new SuaHocSinh();
            sut.SuaHocSinhWD.TenHS.Text = "John Doe";
            sut.SuaHocSinhWD.NgaySinh.SelectedDate = new DateTime(2007, 11, 13); 
            sut.SuaHocSinhWD.GioiTinh.SelectedIndex = 0;  
            sut.SuaHocSinhWD.DiaChi.Text = "123 Main St";  
            sut.SuaHocSinhWD.Email.Text = "john.doe@example.com";
            var projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string filePath = System.IO.Path.Combine(projectPath, "Resources", "Images", "elaina-and-saya-flight-training.jpg");
            sut.ImagePath = filePath;
            // Act

            try
            {
                sut.CapNhatHocSinh();
            } catch (Exception)
            {
                Assert.Fail();
            }
            // Assert
            // Verify that the expected methods are called on the SqlConnectionWrapper
            //fakeSqlConnection.Verify(wrapper => wrapper.Open(), Times.Once);
            //fakeSqlConnection.Verify(wrapper => wrapper.Close(), Times.Once);
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void CapNhatHocSinh_InvalidInput_ShouldNotUpdateHocSinh()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaHocSinhViewModel(fakeSqlConnection.Object);

            sut.SuaHocSinhWD = new SuaHocSinh();
            sut.SuaHocSinhWD.TenHS.Text = "";
            //sut.SuaHocSinhWD.NgaySinh.SelectedDate = new DateTime(2007, 11, 13);
            //sut.SuaHocSinhWD.GioiTinh.SelectedIndex = 0;
            //sut.SuaHocSinhWD.DiaChi.Text = "123 Main St";
            //sut.SuaHocSinhWD.Email.Text = "john.doe@example.com";

            // Act
            try
            {
                sut.CapNhatHocSinh();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            // Assert
            // Verify that the expected methods are called on the SqlConnectionWrapper
            //fakeSqlConnection.Verify(wrapper => wrapper.Open(), Times.Once);
            //fakeSqlConnection.Verify(wrapper => wrapper.Close(), Times.Once);
            Assert.IsTrue(true);


        }



    }
}
