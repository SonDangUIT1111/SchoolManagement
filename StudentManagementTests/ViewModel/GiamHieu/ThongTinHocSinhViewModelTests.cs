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
    [TestClass]
    public class ThongTinHocSinhViewModelTest
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThongTinHocSinhViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThongTinHocSinhViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.ThongTinHocSinhWD = null;
            Assert.IsNull(viewModel.ThongTinHocSinhWD);
            viewModel.DanhSachHocSinh = null;
            Assert.IsNull(viewModel.DanhSachHocSinh);
            viewModel.NienKhoaCmb = null;
            Assert.IsNull(viewModel.NienKhoaCmb);
            viewModel.KhoiCmb = null;
            Assert.IsNull(viewModel.KhoiCmb);
            viewModel.LopCmb = null;
            Assert.IsNull(viewModel.LopCmb);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.IsLoadAll = true;
            Assert.AreEqual(true, viewModel.IsLoadAll);
            viewModel.LopQueries = "ABC";
            Assert.AreEqual("ABC", viewModel.LopQueries);
            viewModel.KhoiQueries = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries);
            viewModel.NienKhoaQueries = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries);
            viewModel.everLoaded = false;
            Assert.AreEqual(false, viewModel.everLoaded);
        }

        [TestMethod]
        public void LoadThongTinComboBox_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThongTinHocSinhViewModel();

            try
            {
                sut.NienKhoaCmb = new System.Collections.ObjectModel.ObservableCollection<string>();
                sut.KhoiCmb = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.Khoi>();
                sut.LopCmb = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.Lop>();
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh>();
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LoadThongTinCmb();
                Assert.IsTrue(sut.NienKhoaCmb.Count > 0);
                Assert.IsTrue(sut.KhoiCmb.Count > 0);
                Assert.AreEqual(sut.KhoiQueries, "1");
                Assert.AreEqual(sut.LopCmb[0].MaLop,151);
                Assert.IsTrue(sut.LopCmb.Count > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }

        [TestMethod]
        public async Task LoadThongTinHocSinh_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel();
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.LopQueries = "151";
                await sut.LoadThongTinHocSinh();
                Assert.AreEqual(sut.DanhSachHocSinh[0].MaHocSinh, 100046);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void SearchHocSinhTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel();
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.LopQueries = "151";
                sut.SearchHocSinh("");
                Assert.IsTrue(sut.DanhSachHocSinh.Count > 0);
                Assert.AreEqual(sut.DanhSachHocSinh[0].MaHocSinh, 100046);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SearchHocSinhAllTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel();
            try
            {
                sut.IsLoadAll = true;
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.SearchHocSinhAll();
                Assert.IsTrue(sut.DanhSachHocSinh.Count > 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

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

            var sut = new ThongTinHocSinhViewModel();
            try
            {
                sut.LopCmb = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.KhoiQueries = "1";
                sut.NienKhoaQueries = "2023-2024";
                sut.FilterLopFromSelection();
                Assert.AreEqual(sut.LopCmb[0].MaLop, 151);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void XoaHocSinhTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel();
            try
            {
                StudentManagement.Model.HocSinh hs = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1
                };
                var res = sut.XoaHocSinh(hs);
                Assert.AreEqual(res, 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
    }
}
