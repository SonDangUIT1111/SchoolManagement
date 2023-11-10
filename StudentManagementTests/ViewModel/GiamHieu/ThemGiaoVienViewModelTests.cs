using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System.IO;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class ThemGiaoVienViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThemGiaoVienViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThemGiaoVienViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.ImagePath = "/ABC";
            Assert.AreEqual("/ABC", viewModel.ImagePath);
            viewModel.ThemGiaoVienWD = null;
            Assert.IsNull(viewModel.ThemGiaoVienWD);
        }

        [TestMethod]
        public void ConvertDateTimeTest()
        {
            DatePicker dt = new DatePicker() { SelectedDate = new DateTime(2023, 1, 1) };
            var result = viewModel.ToShortDateTime(dt);
            Assert.AreEqual(result, "2023-1-1");
        }

        [TestMethod]
        public void VerifyFormatEmailTest()
        {
            var result = viewModel.IsValidEmail("A");
            Assert.IsFalse(result);
            result = viewModel.IsValidEmail("B@gmail.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SendMail_Test()
        {
            try
            {
                viewModel.SendAccountByEmail("ab", "abc", "a@gmail.com");
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

        [TestMethod]
        public void TienHanhThemGiaoVienTest_ChuaDuThongTin()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new ThemGiaoVienViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(1999, 1, 1) };
                var result = sut.ThemGiaoVienMoi("", date, null, null, "1");
                Assert.AreEqual(result, -2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void TienHanhThemGiaoVienTest_EmailSaiDinhDang()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new ThemGiaoVienViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(1999, 1, 1) };
                var result = sut.ThemGiaoVienMoi("abc", date, "abc", "a", "1");
                Assert.AreEqual(result, -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void TienHanhThemGiaoVien_DuDieuKien()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new ThemGiaoVienViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(1999, 1, 1) };
                var projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                string filePath = System.IO.Path.Combine(projectPath, "Resources", "Images", "elaina-and-saya-flight-training.jpg");
                sut.ImagePath = filePath;
                var result = sut.ThemGiaoVienMoi("Nguyen Van A", date, "Hoang gia", "a@gmail.com", "1");
                Assert.AreEqual(result, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }
    }
}
