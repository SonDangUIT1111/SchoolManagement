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

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass()]
    public class QuanLiDiemSoViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private QuanLiDiemSoViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new QuanLiDiemSoViewModel();
        }
        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.IdUser = 1;
            Assert.AreEqual(1, viewModel.IdUser);
            viewModel.everLoaded = true;
            Assert.AreEqual(true, viewModel.everLoaded);
            viewModel.NienKhoaQueries = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries);
            viewModel.HocKyQueries = 1;
            Assert.AreEqual(1, viewModel.HocKyQueries);
            viewModel.KhoiQueries = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries);
            viewModel.LopQueries = "abc";
            Assert.AreEqual("abc", viewModel.LopQueries);
            viewModel.MonHocQueries = "abc";
            Assert.AreEqual("abc", viewModel.MonHocQueries);
            viewModel.NienKhoaQueries2 = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries2);
            viewModel.HocKyQueries2 = 1;
            Assert.AreEqual(1, viewModel.HocKyQueries2);
            viewModel.KhoiQueries2 = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries2);
            viewModel.LopQueries2 = "abc";
            Assert.AreEqual("abc", viewModel.LopQueries2);
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
            viewModel.QuanLiDiemSoWD = null;
            Assert.IsNull(viewModel.QuanLiDiemSoWD);
            viewModel.DanhSachDiem = null;
            Assert.IsNull(viewModel.DanhSachDiem);
            viewModel.DanhSachThanhTich = null;
            Assert.IsNull(viewModel.DanhSachThanhTich);
            viewModel.DanhSachBaoCaoMon = null;
            Assert.IsNull(viewModel.DanhSachBaoCaoMon);
            viewModel.DanhSachBaoCaoHocKy = null;
            Assert.IsNull(viewModel.DanhSachBaoCaoHocKy);
            viewModel.NienKhoaCmb = null;
            Assert.IsNull(viewModel.NienKhoaCmb);
            viewModel.LopDataCmb = null;
            Assert.IsNull(viewModel.LopDataCmb);
            viewModel.KhoiDataCmb = null;
            Assert.IsNull(viewModel.KhoiDataCmb);
            viewModel.MonDataCmb = null;
            Assert.IsNull(viewModel.MonDataCmb);
            viewModel.NienKhoaCmb2 = null;
            Assert.IsNull(viewModel.NienKhoaCmb2);
            viewModel.LopDataCmb2 = null;
            Assert.IsNull(viewModel.LopDataCmb2);
            viewModel.KhoiDataCmb2 = null;
            Assert.IsNull(viewModel.KhoiDataCmb2);
        }

        [TestMethod()]
        public void CountPercentageTest()
        {
            var result = viewModel.CountPercentage(1, 2);
            Assert.AreEqual("50%", result);
        }

        [TestMethod]
        public async Task LoadDanhSachBangDiem_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachDiem = new System.Collections.ObjectModel.ObservableCollection<HeThongDiem> { new HeThongDiem() };
                sut.MonHocQueries = "120";
                sut.LopQueries = "151";
                sut.HocKyQueries = 1;
                sut.NienKhoaQueries = "2021-2022"; 
                await sut.LoadDanhSachBangDiem();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void LoadDanhSachMon_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.MonDataCmb = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.MonHoc> { new StudentManagement.Model.MonHoc() };
                sut.MonHocQueries = "";
                sut.LoadDanhSachMon();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LoadDanhSachNienKhoa_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.NienKhoaCmb = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.NienKhoaCmb2 = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.NienKhoaQueries = "";
                sut.NienKhoaQueries2 = "";
                sut.LoadDanhSachNienKhoa();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LoadDanhSachKhoi_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.KhoiDataCmb = new System.Collections.ObjectModel.ObservableCollection<Khoi> { };
                sut.KhoiDataCmb2 = new System.Collections.ObjectModel.ObservableCollection<Khoi> { };
                sut.KhoiQueries = "";
                sut.KhoiQueries2 = "";
                sut.LoadDanhSachKhoi();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LoadDanhSachLop_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.LopDataCmb = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.LopDataCmb2 = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.LopQueries = "";
                sut.LopQueries2 = "";
                sut.NienKhoaQueries = "2023-2024";
                sut.NienKhoaQueries2 = "2023-2024";
                sut.KhoiQueries = "1";
                sut.KhoiQueries2 = "1";
                sut.LoadDanhSachLop1();
                sut.LoadDanhSachLop2();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMoKhoa()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.LopQueries2 = "1";
                sut.HocKyQueries2 = 1;
                sut.MoKhoa();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestKhoa()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.LopQueries2 = "151";
                sut.HocKyQueries2 = 1;
                sut.DanhSachThanhTich = new System.Collections.ObjectModel.ObservableCollection<ThanhTich> { };
                sut.DanhSachBaoCaoMon = new System.Collections.ObjectModel.ObservableCollection<BaoCaoMon> { };
                sut.DanhSachBaoCaoHocKy = new System.Collections.ObjectModel.ObservableCollection<BaoCaoHocKy> { };
                sut.Khoa();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void LocLop_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new QuanLiDiemSoViewModel(fakeSqlConnection.Object);
            try
            {
                sut.LopDataCmb = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.LopDataCmb2 = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.NienKhoaQueries2 = "2023-2024";
                sut.KhoiQueries = "1";
                sut.KhoiQueries2 = "1";
                sut.LocLop();
                sut.LocLop2();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
    }
}
