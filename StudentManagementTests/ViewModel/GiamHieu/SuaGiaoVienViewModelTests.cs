using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.ViewModel.GiamHieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentManagement.ViewModel.Services;
using System.Threading.Tasks;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.GiamHieu;
using System.IO;
using System.Reflection;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class SuaGiaoVienViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private SuaGiaoVienViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new SuaGiaoVienViewModel();
        }
        [TestMethod]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            string validEmail = "test@example.com";
            bool result = viewModel.IsValidEmail(validEmail);
            Assert.IsTrue(result);
        }
        [TestMethod ]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            string invalidEmail = "invalid-email";
            bool result = viewModel.IsValidEmail(invalidEmail);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void CapNhatGiaoVien_ValidInput_ShouldUpdate()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new SuaGiaoVienViewModel(fakeSqlConnection.Object);

            sut.SuaGiaoVienWD = new SuaGiaoVien();
            sut.SuaGiaoVienWD.TenGV.Text = "John Doe";
            sut.SuaGiaoVienWD.NgaySinh.SelectedDate = new DateTime(2007, 11, 13);
            sut.SuaGiaoVienWD.GioiTinh.SelectedIndex = 0;
            sut.SuaGiaoVienWD.DiaChi.Text = "123 Main St";
            sut.SuaGiaoVienWD.Email.Text = "john.doe@example.com";

            try
            {
                sut.CapNhatGiaoVien();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public void CapNhatGiaoVien_InvalidInput_ShouldNotUpdate()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new SuaGiaoVienViewModel(fakeSqlConnection.Object);

            sut.SuaGiaoVienWD = new SuaGiaoVien();
            sut.SuaGiaoVienWD.TenGV.Text = "";

            try
            {
                sut.CapNhatGiaoVien();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void CapNhatGiaoVien_InvalidEmail_ShouldNotUpdate()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new SuaGiaoVienViewModel(fakeSqlConnection.Object);

            sut.SuaGiaoVienWD = new SuaGiaoVien();
            sut.SuaGiaoVienWD.Email.Text = "invalid-email";

            try
            {
                sut.CapNhatGiaoVien();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void CapNhatGiaoVien_ValidInputWithImagePicker_ShouldUpdate()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            string filePath = Path.Combine("Resources", "Images", "anh-the-demo.jpg");



            var sut = new SuaGiaoVienViewModel(fakeSqlConnection.Object);


            sut.SuaGiaoVienWD = new SuaGiaoVien();
            sut.ImagePath = filePath;
            sut.SuaGiaoVienWD.TenGV.Text = "John Doe";
            sut.SuaGiaoVienWD.NgaySinh.SelectedDate = new DateTime(2007, 11, 13);
            sut.SuaGiaoVienWD.GioiTinh.SelectedIndex = 0;
            sut.SuaGiaoVienWD.DiaChi.Text = "123 Main St";
            sut.SuaGiaoVienWD.Email.Text = "john.doe@example.com";

            try
            {
                sut.CapNhatGiaoVien();
            } catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
