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
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiaoVien
{
    [TestClass]
    public class HeThongBangDiemViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private HeThongBangDiemViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new HeThongBangDiemViewModel();
        }

        [TestMethod]
        public void PropertiesTest()
        {
            viewModel.IdUser = 1;
            Assert.AreEqual(1, viewModel.IdUser);
            viewModel.everLoaded = true;
            Assert.AreEqual(true, viewModel.everLoaded);
            viewModel.JustReadOnly = true;
            Assert.AreEqual(true, viewModel.JustReadOnly);
            viewModel.CanUserEdit = true;
            Assert.AreEqual(true, viewModel.CanUserEdit);
            viewModel.DiemDat = 5;
            Assert.AreEqual(5, viewModel.DiemDat);
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
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
            viewModel.HeThongBangDiemWD = null;
            Assert.IsNull(viewModel.HeThongBangDiemWD);
            viewModel.DanhSachDiem = null;
            Assert.IsNull(viewModel.DanhSachDiem);
            viewModel.NienKhoaCmb = null;
            Assert.IsNull(viewModel.NienKhoaCmb);
            viewModel.LopDataCmb = null;
            Assert.IsNull(viewModel.LopDataCmb);
            viewModel.KhoiDataCmb = null;
            Assert.IsNull(viewModel.KhoiDataCmb);
            viewModel.MonDataCmb = null;
            Assert.IsNull(viewModel.MonDataCmb);
        }

        [TestMethod]
        public void KiemTraDiemHopLeTest()
        {
            viewModel.DanhSachDiem = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HeThongDiem>()
            {
         
            };
            StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem
            {
               Diem15Phut = 10,
               Diem1Tiet = 9,
               DiemTB = 8,
            };
            viewModel.DanhSachDiem.Add(diem);
            var result = viewModel.KiemTraDiemHopLe();
            Assert.IsTrue(result);
            StudentManagement.Model.HeThongDiem diem2 = new StudentManagement.Model.HeThongDiem
            {
                Diem15Phut = 12,
                Diem1Tiet = -4,
                DiemTB = 8,
            };
            viewModel.DanhSachDiem.Add(diem2);
            var result2 = viewModel.KiemTraDiemHopLe();
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void LoadDuLieuComboBoxTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new HeThongBangDiemViewModel(fakeSqlConnection.Object);

            try
            {
                
                sut.KhoiDataCmb = new System.Collections.ObjectModel.ObservableCollection<Khoi> { };
                sut.LopDataCmb = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.MonDataCmb = new System.Collections.ObjectModel.ObservableCollection<MonHoc> { };
                sut.NienKhoaCmb = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.LoadDuLieuComboBox();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LoadDanhSachBangDiemTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new HeThongBangDiemViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachDiem = new System.Collections.ObjectModel.ObservableCollection<HeThongDiem> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.LopQueries = "151";
                sut.HocKyQueries = 1;
                sut.MonHocQueries = "120";
                await sut.LoadDanhSachBangDiem();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FilterLopFromSelectionTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new HeThongBangDiemViewModel(fakeSqlConnection.Object);
            try
            {
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LopDataCmb = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.FilterLopFromSelection();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void XacDinhQuyenHanTest_KhongCoQuyenNhapDiem()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new HeThongBangDiemViewModel(fakeSqlConnection.Object);
            try
            {
                sut.IdUser = 100000;
                sut.LopQueries = "151";
                sut.MonHocQueries = "1";
                var result = sut.XacDinhQuyenHan();
                Assert.AreEqual(false, result);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
        [TestMethod]
        public void XacDinhQuyenHanTest_CoQuyenNhapDiem()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new HeThongBangDiemViewModel(fakeSqlConnection.Object);
            try
            {
                sut.IdUser = 100032;
                sut.LopQueries = "151";
                sut.MonHocQueries = "121";
                var result = sut.XacDinhQuyenHan();
                Assert.AreEqual(true, result);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LuuBangDiemTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new HeThongBangDiemViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachDiem = new System.Collections.ObjectModel.ObservableCollection<HeThongDiem> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.LopQueries = "151";
                sut.HocKyQueries = 1;
                sut.MonHocQueries = "120";
                await sut.LoadDanhSachBangDiem();
                await sut.LuuBangDiem();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

    }
}
