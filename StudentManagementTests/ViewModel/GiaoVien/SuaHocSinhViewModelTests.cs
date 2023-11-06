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
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Initialize the WPF application for UI tests
            Application app = new Application();
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
        public void CapNhatHocSinh_ValidInput_ShouldUpdateHocSinh()
        {
            // Arrange
            // Mock the ISqlConnectionWrapper
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            // Set up your SuaHocSinhViewModel with the mock SqlConnectionWrapper
            var sut = new SuaHocSinhViewModel(fakeSqlConnection.Object);


            // Set up other required properties for your test
            sut.SuaHocSinhWD = new SuaHocSinh();
            sut.SuaHocSinhWD.TenHS.Text = "John Doe";  // Use the Text property to set the text of TextBox
            sut.SuaHocSinhWD.NgaySinh.SelectedDate = new DateTime(2007, 11, 13);  // Set the SelectedDate property of DatePicker
            sut.SuaHocSinhWD.GioiTinh.SelectedIndex = 0;  // Set the SelectedIndex property of ComboBox
            sut.SuaHocSinhWD.DiaChi.Text = "123 Main St";  // Use the Text property to set the text of TextBox
            sut.SuaHocSinhWD.Email.Text = "john.doe@example.com";  // Use the Text property to set the text of TextBox

            // Act
            var result = sut.CapNhatHocSinh();

            // Assert
            // Verify that the expected methods are called on the SqlConnectionWrapper
            //fakeSqlConnection.Verify(wrapper => wrapper.Open(), Times.Once);
            //fakeSqlConnection.Verify(wrapper => wrapper.Close(), Times.Once);

            // You can also assert other expectations, like checking if the MessageBoxOK or MessageBoxSuccessful dialogs are shown.
            Assert.IsTrue(result, "CapNhatHocSinh succeeded");
            //Assert.IsFalse(result, "CapNhatHocSinh failed");

        }

    }
}
