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
    public class SuaThongTinHocSinhViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private SuaThongTinHocSinhViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new SuaThongTinHocSinhViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.ImagePath = "/ABC";
            Assert.AreEqual("/ABC", viewModel.ImagePath);
            viewModel.SuaThongTinHocSinhWD = null;
            Assert.IsNull(viewModel.SuaThongTinHocSinhWD);
            viewModel.HocSinhHienTai = null;
            Assert.IsNull(viewModel.HocSinhHienTai);
        }


        [TestMethod]
        public void TienHanhSuaThongTinHS_TestViPhamTuoiNhoHon()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaThongTinHocSinhViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(2010,1,1) };
                sut.HocSinhHienTai = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1,
                };
                sut.ImagePath = null;
                var result = sut.TienHanhSuaThongTinHocSinh(date, "ABC", true, "Home Street", "abc@gmal.com");
                Assert.AreEqual(result, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void TienHanhSuaThongTinHS_TestViPhamTuoiLonHon()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaThongTinHocSinhViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(2000, 1, 1) };
                sut.ImagePath = null;
                var result = sut.TienHanhSuaThongTinHocSinh(date, "ABC", true, "Home Street", "abc@gmal.com");
                Assert.AreEqual(result, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void TienHanhSuaThongTinHS_TestHopLeKhongCoAnh()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaThongTinHocSinhViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(2007, 1, 1) };
                sut.HocSinhHienTai = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1
                };
                sut.ImagePath = null;
                var result = sut.TienHanhSuaThongTinHocSinh(date, "ABC", true, "Home Street", "abc@gmal.com");
                Assert.AreEqual(result, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void TienHanhSuaThongTinHS_TestHopLeCoAnh()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaThongTinHocSinhViewModel(fakeSqlConnection.Object);

            try
            {
                DatePicker date = new DatePicker { SelectedDate = new DateTime(2007, 1, 1) };
                sut.HocSinhHienTai = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1
                };
                var projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                string filePath = System.IO.Path.Combine(projectPath, "Resources", "Images", "elaina-and-saya-flight-training.jpg");
                sut.ImagePath = filePath;
                var result = sut.TienHanhSuaThongTinHocSinh(date, "ABC", true, "Home Street", "abc@gmal.com");
                Assert.AreEqual(result, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void ConvertDateTimeTest()
        {
            DatePicker dt = new DatePicker() { SelectedDate = new DateTime(2023, 1, 1) };
            var result = viewModel.ToShortDateTime(dt);
            Assert.AreEqual(result, "2023-1-1");
        }
    }
}
