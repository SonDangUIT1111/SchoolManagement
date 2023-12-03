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
    public class ThemLopHocViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThemLopHocViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThemLopHocViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.MaKhoi = "/ABC";
            Assert.AreEqual("/ABC", viewModel.MaKhoi);
            viewModel.ThemLopHocWD = null;
            Assert.IsNull(viewModel.ThemLopHocWD);
            viewModel.KhoiCmb = null;
            Assert.IsNull(viewModel.KhoiCmb);
        }

        [TestMethod]
        public void LoadNienKhoa_Test()
        {
            var result = viewModel.LoadNienKhoa(new DateTime(2020,1,1));
            Assert.AreEqual(result, "2019-2020");
        }

        [TestMethod]
        public void LoadKhoi_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new ThemLopHocViewModel();

            try
            {
                sut.KhoiCmb = new System.Collections.ObjectModel.ObservableCollection<Khoi> { };
                sut.LoadKhoiCmb();
                Assert.AreEqual(sut.MaKhoi, "1");
                Assert.AreEqual(sut.KhoiCmb[0].MaKhoi, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void ThemLopMoi_TenLopDaTonTaiTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new ThemLopHocViewModel();

            try
            {
                sut.NienKhoa = "2023-2024";
                sut.MaKhoi = "1";
                var result = sut.ThemLopMoi("10A1", new Khoi() { MaKhoi = 1});
                Assert.AreEqual(result, -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void ThemLopMoi_DuDieuKien()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new ThemLopHocViewModel();

            try
            {
                sut.NienKhoa = "2023-2024";
                sut.MaKhoi = "1";
                Random rnd = new Random();
                string lop = rnd.Next(100000, 999999).ToString();
                var result = sut.ThemLopMoi(lop, new Khoi() { MaKhoi = 2 });
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
